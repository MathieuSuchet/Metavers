using NativeWebSocket;
using System;
using UnityEngine;

public class WebSocketHandler : MonoBehaviour
{
    WebSocket websocket;

    // Public event to let external scripts subscribe to OnMessage
    public event Action<string> OnMessageReceived;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8765");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! " + message);

            // Invoke external handler if set
            OnMessageReceived?.Invoke(message);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    public void SendState(object state)
    {
        string json = JsonUtility.ToJson(state);
        websocket.SendText(json);
    }

}
