using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 15f;
    public float gravityMultiplier = 2.5f;
    public int maxJumps = 2;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float fieldOfView = 60f;

    private Rigidbody rb;
    private int jumpCount;
    private bool isGrounded;
    private bool isDashing;
    private float dashCooldownTimer;
    private Camera playerCamera;
    private float xRotation = 0f;

    [SerializeField] private LayerMask interactables;
    [SerializeField] private GameObject toolTip;
    public InventoryController inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 2f;
        jumpCount = 0;
        isDashing = false;
        dashCooldownTimer = 0f;

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = fieldOfView;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //if the player is not in menu: look around
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
        }
        HandleJump();
        HandleDash();
        CheckInteractable();
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (playerCamera != null && playerCamera.fieldOfView != fieldOfView)
        {
            playerCamera.fieldOfView = fieldOfView;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
            if (inventory.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMovement();
            ApplyCustomGravity();
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        Vector3 targetVelocity = movement.normalized * speed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if ((isGrounded || jumpCount < maxJumps) && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpCount++;
            isGrounded = false;
        }
    }

    void HandleDash()
    {
        if (dashCooldownTimer <= 0 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Dash")))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 dashDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        dashDirection = dashDirection.normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = dashDirection * dashForce;
            yield return null;
        }

        isDashing = false;
    }

    void ApplyCustomGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void CheckInteractable()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 4, interactables))
        {
            toolTip.SetActive(true);
        }
        else
        {
            toolTip.SetActive(false);
        }
    }
}
