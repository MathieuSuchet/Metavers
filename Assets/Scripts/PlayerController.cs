using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float velocity;
    public float jumpImpulse = 20;
    public float gravitationalAddition = 0.1f;

    Rigidbody rb;
    bool jumping = false;

    public bool Dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Dead) return;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        float speedMultiplier = 1.0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier = 2.0f;
        }

        Vector3 displacement = new(horizontal, 0, vertical);
        displacement.Normalize();

        transform.Translate(Time.deltaTime * velocity * speedMultiplier * displacement);

        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            jumping = true;
            rb.AddForce(new Vector3(0, jumpImpulse, 0), ForceMode.Impulse);
        }

        if (jumping)
        {
            rb.AddRelativeForce(new Vector3(0, -gravitationalAddition, 0), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) jumping = false;
        if (collision.gameObject.CompareTag("Enemy")) Dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Destroy");
            other.GetComponent<Collectible>().dead = true;
            other.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public float x;
    public float y;
    public float z;
}