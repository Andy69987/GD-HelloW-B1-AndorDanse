using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed for movement
    public float jumpForce = 5f; // Force for jumping
    public float mouseSensitivity = 100f; // Sensitivity for mouse look
    public bool isGrounded = true; // To check if the player is on the ground

    private Rigidbody rb;
    private float xRotation = 0f; // Keeps track of vertical rotation

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the Player.");
        }

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement on X and Z axes
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Player is airborne
        }

        // Mouse look (horizontal and vertical rotation)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // Adjust vertical rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y + mouseX, 0f); // Rotate player horizontally
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera vertically
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
