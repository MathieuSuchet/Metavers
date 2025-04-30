using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;
using Newtonsoft.Json.Linq;

public class WebSocketClient_Native : MonoBehaviour
{
    public string serverUrl = "ws://localhost:8765";
    public GameObject playerPrefab;
    public float sendRate = 0.05f;

    private WebSocket ws;
    private string myID;
    private Dictionary<string, GameObject> players = new();

    async void Start()
    {
        ws = new WebSocket(serverUrl);

        ws.OnOpen += () =>
        {
            Debug.Log("WebSocket connected!");
        };

        ws.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            HandleMessage(message);
        };

        ws.OnError += (e) =>
        {
            Debug.LogError("WebSocket Error: " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("WebSocket closed.");
        };

        await ws.Connect();

        StartCoroutine(SendPositionLoop());
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws != null)
            ws.DispatchMessageQueue();
#endif

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * 5f);

        if (Input.GetKeyDown(KeyCode.Space))
            SendAction("jump");
        else if (Input.GetKeyDown(KeyCode.H))
            SendAction("wave");
    }

    void HandleMessage(string json)
    {
        JObject msg = JObject.Parse(json);
        string type = msg["type"]?.ToString();

        if (type == "welcome")
        {
            myID = msg["id"].ToString();
            players[myID] = this.gameObject;
            Debug.Log("Assigned ID: " + myID);
        }
        else if (type == "player_joined")
        {
            string id = msg["id"].ToString();
            if (!players.ContainsKey(id))
                players[id] = Instantiate(playerPrefab);
        }
        else if (type == "player_left")
        {
            string id = msg["id"].ToString();
            if (players.ContainsKey(id))
            {
                Destroy(players[id]);
                players.Remove(id);
            }
        }
        else if (type == "update")
        {
            string id = msg["id"].ToString();
            if (id == myID) return;

            if (!players.ContainsKey(id))
                players[id] = Instantiate(playerPrefab);

            GameObject p = players[id];

            Vector3 pos = new(
                (float)msg["position"][0],
                (float)msg["position"][1],
                (float)msg["position"][2]);

            Vector3 rot = new(
                (float)msg["rotation"][0],
                (float)msg["rotation"][1],
                (float)msg["rotation"][2]);

            p.transform.position = pos;
            p.transform.eulerAngles = rot;

            string action = msg["action"]?.ToString();
            if (action == "jump")
                Debug.Log($"{id} jumps!");
            else if (action == "wave")
                Debug.Log($"{id} waves!");
        }
    }

    IEnumerator SendPositionLoop()
    {
        while (true)
        {
            SendPosition();
            yield return new WaitForSeconds(sendRate);
        }
    }

    async void SendPosition()
    {
        if (ws == null || ws.State != WebSocketState.Open || string.IsNullOrEmpty(myID)) return;

        JObject msg = new JObject
        {
            ["type"] = "update",
            ["position"] = new JArray(transform.position.x, transform.position.y, transform.position.z),
            ["rotation"] = new JArray(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z),
            ["action"] = ""
        };

        Debug.Log(msg);

        await ws.SendText(msg.ToString());
    }

    async void SendAction(string action)
    {
        if (ws == null || ws.State != WebSocketState.Open || string.IsNullOrEmpty(myID)) return;

        JObject msg = new JObject
        {
            ["type"] = "update",
            ["position"] = new JArray(transform.position.x, transform.position.y, transform.position.z),
            ["rotation"] = new JArray(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z),
            ["action"] = action
        };

        await ws.SendText(msg.ToString());
    }

    async void OnApplicationQuit()
    {
        if (ws != null)
            await ws.Close();
    }
}
