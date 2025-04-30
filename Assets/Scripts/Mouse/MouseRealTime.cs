using UnityEngine;

[System.Serializable]
public class MouseState
{
    public float x;
    public float y;
}

public class MouseRealTime : MonoBehaviour
{
    public GameObject GameObjectSquare;
    public GameObject GameObjectSphere;
    public GameObject GameObjectCylinder;

    public WebSocketHandler WebSocketHandler;

    private Vector3 lastMousePosition;

    void HandleReturn(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        GameObjectSquare.SetActive(false);
        GameObjectSphere.SetActive(false);
        GameObjectCylinder.SetActive(false);

        switch (message.ToLower())
        {
            case "cube":
                GameObjectSquare.SetActive(true);
                break;
            case "sphere":
                GameObjectSphere.SetActive(true);
                break;
            case "cylinder":
                GameObjectCylinder.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown shape received: " + message);
                break;
        }
    }

    void Start()
    {
        if (WebSocketHandler != null)
        {
            WebSocketHandler.OnMessageReceived += HandleReturn;
        }

        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;

        // Send every frame, or only if changed:
        if (currentMousePosition != lastMousePosition)
        {
            lastMousePosition = currentMousePosition;

            var mouseState = new MouseState
            {
                x = Input.mousePosition.x / Screen.width,
                y = Input.mousePosition.y / Screen.height
            };


            if (WebSocketHandler != null)
            {
                WebSocketHandler.SendState(mouseState);
            }
        }
    }

    void OnDestroy()
    {
        if (WebSocketHandler != null)
        {
            WebSocketHandler.OnMessageReceived -= HandleReturn;
        }
    }
}

