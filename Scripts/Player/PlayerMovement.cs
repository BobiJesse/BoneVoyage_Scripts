using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;

    [Header("Glide Settings")]
    public float forwardForce = 10f;
    public float glideGravity = 2f;
    public float diveBoost = 15f;

    private Rigidbody rb;
    private bool isGliding = false;
    private bool isGroundedCached = false;
    private float normalGravity;
    public float flyingGravity;

    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalGravity = Physics.gravity.y;
        flyingGravity = normalGravity / 2;
    }

    void Update()
    {
        // --- Input ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveInput = new Vector3(h, 0f, v).normalized;

        isGroundedCached = isGrounded();

        // --- Jump ---
        if (isGroundedCached && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // --- Start Glide ---
        if (Input.GetKeyDown(KeyCode.G) && !isGroundedCached)
        {
            StartGlide();
        }
    }

    void FixedUpdate()
    {
        if (isGliding)
        {
            // ---- GLIDING PHYSICS ----
            Vector3 forward = transform.forward;
            float pitch = Vector3.Dot(forward, Vector3.up);

            // Custom weaker gravity
            rb.AddForce(Vector3.up * (normalGravity + glideGravity), ForceMode.Acceleration);

            // Forward glide
            rb.AddForce(forward * forwardForce, ForceMode.Acceleration);

            // Dive boost
            rb.AddForce(forward * (-pitch * diveBoost), ForceMode.Acceleration);
        }
        else
        {
            // ---- NORMAL GROUND MOVEMENT ----
            if (isGroundedCached)
            {
                Vector3 moveDir = transform.TransformDirection(moveInput);
                Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(targetPos);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGliding && isGrounded())
        {
            StopGlide();
        }
    }

    private void StartGlide()
    {
        isGliding = true;
        rb.linearDamping = 0.5f;
        Physics.gravity = new Vector3 (0, flyingGravity, 0);
    }

    private void StopGlide()
    {
        isGliding = false;
        rb.linearDamping = 0f;
        Physics.gravity = new Vector3(0, normalGravity, 0);
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("dmg"))
        {
            Debug.Log("Player hit");
            Destroy(other.gameObject);
        }
    }
}
