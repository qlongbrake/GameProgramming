using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform mainCamera;
    private PlayerMovement player;

    private float horizontalTilt;
    private float verticalTilt;
    private float initialXRotation;

    [SerializeField]
    private float maxVerticalAngle;
    [SerializeField]
    private float maxHorizontalAngle;
    [SerializeField]
    private float tiltSpeed;

    [SerializeField]
    private float offset;

    [SerializeField]
    private bool useFloorNormal;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = transform.GetChild(0);                // Get Camera from child
        player = FindObjectOfType<PlayerMovement>();  // Find player

        initialXRotation = transform.eulerAngles.x;     // Store initial x rotation
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalTilt = Input.GetAxis("Vertical");
        horizontalTilt = Input.GetAxis("Horizontal");

        player.Move(verticalTilt, horizontalTilt, transform.right);
    }

    private void Update()
    {
        CameraTilt();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    void CameraTilt()
    {
        // Rotate camera container along the x axis when tilting the joystick up or down to give a forward and back tilt effect.
        // The further up the joystick is the higher the angle for target rotation will be and vice versa.
        float scaledVerticalTilt = initialXRotation - (verticalTilt * maxVerticalAngle);

        // Using floor normal adjust the rotation of the camera's x axis at rest.
        float angleBetweenFloorNormal = useFloorNormal ? Vector3.SignedAngle(Vector3.up, player.floorNormal, transform.right) : 0.0f;

        Quaternion targetXRotation = Quaternion.Euler(scaledVerticalTilt + angleBetweenFloorNormal, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetXRotation, tiltSpeed * Time.deltaTime);

        // Rotate camera along the z axis when tilting the joystick left or right to give a left and right tilt effect.
        // The further right the joystick is the higher the angle for target rotation will be and vice versa.
        float scaledHorizontalTilt = Input.GetAxis("Horizontal") * maxHorizontalAngle;

        Quaternion targetZRotation = Quaternion.Euler(mainCamera.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, scaledHorizontalTilt);

        mainCamera.rotation = Quaternion.RotateTowards(mainCamera.rotation, targetZRotation, tiltSpeed * Time.deltaTime);
    }

    void FollowTarget()
    {
        // Get forward vector minus the y component
        Vector3 vectorA = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

        // Get target's velocity vector minus the y component
        Vector3 vectorB = new Vector3(player.rb.velocity.x, 0.0f, player.rb.velocity.z);

        // Find the angle between vectorA and vectorB
        float rotateAngle = Vector3.SignedAngle(vectorA.normalized, vectorB.normalized, Vector3.up);

        // Get the target's speed (maginitude) without the y component
        // Only set speed factor when vector A and B are almost facing the same direction
        float speedFactor = Vector3.Dot(vectorA, vectorB) > 0.0f ? vectorB.magnitude : 1.0f;

        // Rotate towards the angle between vectorA and vectorB
        // Use speedFactor so camera doesn't rotatate at a constant speed
        // Limit speedFactor to be between 1 and 2
        transform.Rotate(Vector3.up, rotateAngle * Mathf.Clamp(speedFactor, 1.0f, 2.0f) * Time.deltaTime);

        // Position the camera behind target at a distance of offset
        transform.position = player.transform.position - (transform.forward * offset);
        transform.LookAt(player.transform.position);
    }
}
