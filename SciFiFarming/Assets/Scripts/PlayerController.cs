using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
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

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    public float health;
    public float defence;
    public int money;

    public Rigidbody rb;
    private int jumpCount;
    private bool isGrounded;
    private bool isDashing;
    private float dashCooldownTimer;
    private Camera playerCamera;
    private float xRotation = 0f;

    [SerializeField] private GameObject toolTip;
    [SerializeField] private GameObject damageFilter;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private HeaderInfo headerInfo;
    [SerializeField] private Transform handSpawnPos;
    public InventoryController inventory;
    public static PlayerController clientPlayer;

    //photon
    [HideInInspector]
    public int id;
    public Player photonPlayer;
    public static PhotonView playerPhotonView;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = fieldOfView;
        }
    }

    void Start()
    {
        rb.mass = 2f;
        jumpCount = 0;
        isDashing = false;
        dashCooldownTimer = 0f;


        Cursor.lockState = CursorLockMode.Locked;
        if (PersistentData.health == 0)
        {
            health = maxHealth;
        }
        else 
        {
            health = PersistentData.health;
            headerInfo.UpdateHealthBar(health / maxHealth);
        }
        //clientPlayer = this; //this is an identifier for other scripts, it will need to be initialized for multiplayer
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

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
        //picking up items
        /*
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            PickUpItem(currentInteractable);
        }
        */

        if (Input.GetKeyDown(KeyCode.I))
        {
            
            if (!inventory.gameObject.activeSelf && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                inventory.gameObject.SetActive(true);
            }
            else if(inventory.gameObject.activeSelf)
            {
                inventory.gameObject.SetActive(false);
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

    [PunRPC]
    public void Initialize(Player player)
    {
        //this allows up to referance this instance of this script to photon
        id = player.ActorNumber;
        photonPlayer = player;

        GameManager.instance.players[id - 1] = this;

        //headerInfo.Initialize(player.NickName, maxHp);
        headerInfo = GameManager.instance.GetHeader();
        inventory = GameManager.instance.GetPlayerInventory();
        damageFilter = GameManager.instance.GetDamageFilter();
        toolTip = GameManager.instance.GetToolTip();

        if (player.IsLocal)
        {
            //Debug.Log("ClientPlayerSet");
            ToolbarController.instance.SetHandSpawnPos(handSpawnPos);
            clientPlayer = this;
            playerPhotonView = photonView;
            if (player.NickName == "ProfS")
            {
                PersistentData.money += 10000;
            }
        }
        else
        {
            rb.isKinematic = true;
            playerCamera.gameObject.SetActive(false);
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


    [HideInInspector] public GameObject currentInteractable; // to track the focused item

    void CheckInteractable()
    {
        /*
        if(currentInteractable != null) {
            Debug.Log(clientPlayer);
            Debug.Log(currentInteractable.name);
        }
        */
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, GameManager.interactables))
        {
            //Debug.Log("Bingus");
            currentInteractable = hit.collider.gameObject; // track the interactable
            toolTip.SetActive(true);
            SetToolTip();
            //toolTip.transform.position = hit.collider.bounds.center + Vector3.up * 0.5f; // position tooltip
        }
        else
        {
            currentInteractable = null;
            toolTip.SetActive(false);
        }
    }

    private void SetToolTip()
    {
        TextMeshProUGUI toolTipText = toolTip.GetComponent<TextMeshProUGUI>();
        if (currentInteractable.CompareTag("Pickup"))
        {
            toolTipText.text = "E to pickup";
        }
        else if (currentInteractable.CompareTag("Goo"))
        {
            toolTipText.text = "E to collect goo";
        }
        else if (currentInteractable.CompareTag("NPC"))
        {
            toolTipText.text = "E to talk";
        }
        else if (currentInteractable.CompareTag("Bed"))
        {
            toolTipText.text = "E to sleep\n(All players must sleep to advance)";
        }
        else if (currentInteractable.CompareTag("GooSlot"))
        {
            toolTipText.text = "E to fill tank";
        }
        else if (currentInteractable.CompareTag("Rack"))
        {
            toolTipText.text = "E to open/close\nE with seed in hand to plant\nR to harvest";
        }
        else if (currentInteractable.CompareTag("SeedMaker"))
        {
            toolTipText.text = "E with plant in hand to make seeds";
        }
        else
        {
            toolTipText.text = "E";
        }
    }

    //the pickup action was moved to individual scripts to allow for more accurate control of actions taken
    /*
    void PickUpItem(GameObject item)
    {
        //idea is to add item to inventory or deactivate it
        item.SetActive(false);
        Debug.Log("Picked up: " + item.name);
    }
    */

    [PunRPC]
    public void TakeDamage(float damage)
    {
        //modify damage based on armor value
        //take damage
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (!photonView.IsMine)
            {
                return;
            }
            //update healthbar
            headerInfo.UpdateHealthBar(health / maxHealth);
            StartCoroutine(DamageFlash());

            IEnumerator DamageFlash()
            {
                damageFilter.SetActive(true);
                yield return new WaitForSeconds(0.05f);
                damageFilter.SetActive(false);
            }
        }
    }

    private void Die()
    {
        //this probably needs more functionality
        PersistentData.money /= 2;
        transform.position = spawnPoint;
        health = maxHealth;
        headerInfo.UpdateHealthBar(health / maxHealth);
    }

    [PunRPC]
    private void Heal(float healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);

        if (photonView.IsMine)
        {
            headerInfo.UpdateHealthBar(health / maxHealth);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Debug.Log("Write Defence");
            stream.SendNext(defence);
        }
        else //if (stream.IsReading)
        {
            //Debug.Log("Read Defence");
            defence = (float)stream.ReceiveNext();
        }
    }
}
