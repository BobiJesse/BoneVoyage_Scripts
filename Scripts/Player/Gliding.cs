using UnityEngine;
using UnityEngine.SceneManagement;

public class Gliding : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 5f;       // Minimum forward speed
    [SerializeField] private float maxSpeed = 60f;      // Maximum forward speed
    [SerializeField] private float thrustFactor = 20f;  // How much speed you gain when diving

    [Header("Diving Settings")]
    [SerializeField] private float minDiveDegree = 30f;
    [SerializeField] private float maxDiveDegree = 90f;

    [Header("Drag Settings")]
    [SerializeField] private float dragFactor = 0.2f;   // Drag when gliding level
    [SerializeField] private float minDrag = 0.05f;     // Drag when diving

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;       // How fast the glider rotates to match camera
    [SerializeField] private float tiltStrength = 45f;       // Max tilt angle for banking
    [SerializeField] private float tiltReturnSpeed = 2f;     // How fast tilt returns to 0

    [Header("Gravity & Lift")]
    [SerializeField] private float gravityMultiplier = 0.5f; // 50% gravity
    [SerializeField] private float liftMultiplier = 0.1f;    // Small upward lift based on speed

    private float currentSpeed;
    private float tiltValue;
    private Transform cameraTransform;
    private Rigidbody rb;

    public AudioClip wooshSound;
    public AudioClip playerHitSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 1.5f;

        cameraTransform = Camera.main.transform;
        //currentSpeed = minSpeed; // DO NOT START WITH MOMENTUM, IT OVERRIDES ADDSPEED
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate()
    {
        HandleRotation();
        HandleGlidePhysics();
        ApplyGravityAndLift();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Smooth tilt/banking
        tiltValue = Mathf.Lerp(tiltValue, mouseX * -tiltStrength, Time.fixedDeltaTime * tiltReturnSpeed);
        tiltValue = Mathf.Clamp(tiltValue, -tiltStrength, tiltStrength);

        // Camera rotation
        Vector3 camAngles = cameraTransform.eulerAngles;
        float pitch = camAngles.x;
        if (pitch > 180) pitch -= 360f;

        Quaternion targetRotation = Quaternion.Euler(pitch, camAngles.y, tiltValue);
        Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothRotation);
    }

    void HandleGlidePhysics()
    {
        float pitch = transform.eulerAngles.x;
        if (pitch > 180) pitch -= 360f;

        // Convert pitch range: -90 (up) -> 0 (forward) -> +90 (down)
        float normalizedPitch = Mathf.Clamp(pitch / 90f, -1f, 1f);

        // --- DIVE ACCELERATION ---
        // Only start gaining speed after minDiveDegree down, full gain past maxDiveDegree
        float diveFactor = 0f;
        if (normalizedPitch > 0f)
        {
            diveFactor = Mathf.Clamp((normalizedPitch - minDiveDegree / 90f) / (maxDiveDegree / 90f - minDiveDegree / 90f), 0f, 1f);
        }

        // --- CLIMB SLOWDOWN ---
        // Softer loss of speed when looking up (half strength)
        float climbLoss = Mathf.Abs(Mathf.Min(0f, normalizedPitch)) * 0.5f;

        // --- LEVEL FLIGHT DRAG ---
        // Slight natural slowdown when looking straight forward
        float levelDragLoss = 0.05f;

        // --- COMBINE INTO FINAL SPEED CHANGE ---
        // Downward pitch increases speed, upward pitch decreases it
        float gainLossBalance = diveFactor - (climbLoss + levelDragLoss);

        // Apply acceleration/deceleration
        currentSpeed += gainLossBalance * thrustFactor * 0.5f * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // --- APPLY MOVEMENT ---
        rb.linearVelocity = transform.forward * currentSpeed;

        // Adjust drag dynamically — less drag when diving, more when climbing
        rb.linearDamping = Mathf.Lerp(minDrag, dragFactor, 1f - Mathf.Abs(normalizedPitch));
    }

    void ApplyGravityAndLift()
    {
        // Base gravity always applies
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        // If moving fast enough, generate lift; otherwise, fall faster
        float liftStrength = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);
        float effectiveLift = liftMultiplier * liftStrength;

        rb.AddForce(Vector3.up * currentSpeed * effectiveLift, ForceMode.Force);

        // If speed is very low, increase gravity effect (stall)
        if (currentSpeed <= minSpeed + 1f)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier * 1.5f, ForceMode.Acceleration);
        }
    }

    public void AddSpeed(float amount)
    {
        SFXAudioManager.instance.PlaySFX(wooshSound);
        currentSpeed += amount;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("dmg"))
        {
            SFXAudioManager.instance.PlaySFX(playerHitSound);
            Debug.Log("Player hit");
            GameManager.instance.RestartLevel();
        }
    }
}
