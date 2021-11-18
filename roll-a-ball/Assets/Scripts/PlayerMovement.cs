using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    private GameControl controller;
    //public float speed;
    private float sX = 0;
    private float sY = 2;
    private float sZ = 0;

    [HideInInspector]
    public Vector3 floorNormal;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float groundCheckRadius;
    public float maxSpeed;
    [SerializeField]
    private float moveForce;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            controller.collected++;
            controller.score += 100;
        }

        if (other.transform.CompareTag("Death"))
        {
            transform.position = new Vector3(sX, sY, sZ);
            //SceneManager.LoadScene(0);
        }
    }
    /*
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    */

    public void Move(float verticalTilt, float horizontalTilt, Vector3 right)
    {
        var tempMoveForce = moveForce;
        // Only apply movement when the player is grounded
        if (!OnGround()) 
        {
            tempMoveForce = tempMoveForce * .2f;
        }
        //{
            CalculateFloorNormal();

            // No input from player
            if (horizontalTilt == 0.0f && verticalTilt == 0.0f && rb.velocity.magnitude > 0.0f)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, tempMoveForce * 0.1f * Time.deltaTime); // Slow down
            }
            else
            {
                // Get a direction perpendicular to the camera's right vector and the floor's normal (The forward direction)
                Vector3 forward = Vector3.Cross(right, floorNormal);

                // Apply moveForce scaled by verticalTilt in the forward direction (Half the move force when moving backwards)
                Vector3 forwardForce = (verticalTilt > 0.0f ? 1.0f : 0.5f) * tempMoveForce * verticalTilt * forward;
                // Apply moveForce scaled by horizontalTilt in the right direction
                Vector3 rightForce = tempMoveForce * horizontalTilt * right;

                Vector3 forceVector = forwardForce + rightForce;

                rb.AddForce(forceVector);
            }
        //}
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(transform.position - (Vector3.up * 0.6f), groundCheckRadius, whatIsGround);
    }

    private void CalculateFloorNormal()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
        {
            floorNormal = hit.normal;
        }
    }


}
