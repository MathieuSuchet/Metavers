using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float rotationSpeed = 360f; // degrees per second
    private Quaternion targetRotation;
    private bool isRotating = false;

    public WebSocketHandler handler;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void MoveCube(string direction)
    {
        direction = direction.ToLower();

        if (isRotating) return; // prevent input while rotating

        if (direction.Contains("left"))
        {
            targetRotation *= Quaternion.Euler(0, -90, 0);
        }
        else if (direction.Contains("right"))
        {
            targetRotation *= Quaternion.Euler(0, 90, 0);
        }
        else if (direction.Contains("up"))
        {
            targetRotation *= Quaternion.Euler(-90, 0, 0);
        }
        else if (direction.Contains("down"))
        {
            targetRotation *= Quaternion.Euler(90, 0, 0);
        }

        isRotating = true;
    }
}
