using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    Transform playerTransform;

    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), step);
    }
}

[System.Serializable]
public class EnemyData
{
    public float x;
    public float y;
    public float z;
}