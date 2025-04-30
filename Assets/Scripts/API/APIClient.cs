using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APIClient : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1:5000/direction"; // Or your actual IP
    public TMP_InputField inputField;
    public TextMeshProUGUI resultText;
    public CubeController cubeController;

    public void SendTextToAPI()
    {
        string userInput = inputField.text;
        StartCoroutine(PostRequest(apiUrl, userInput));
    }

    IEnumerator PostRequest(string url, string inputText)
    {
        string jsonBody = "{\"text\":\"" + inputText + "\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Réponse : " + responseText);

            // ✅ Parse response using Unity's JsonUtility
            DirectionResponse response = JsonUtility.FromJson<DirectionResponse>(responseText);
            if (!string.IsNullOrEmpty(response.direction))
            {
                // Display only the direction in resultText
                resultText.text = response.direction;

                // Move the cube
                cubeController.MoveCube(response.direction);
            }
        }
        else
        {
            Debug.LogError("Erreur API : " + request.error);
            resultText.text = "Erreur : " + request.error;
        }
    }
}