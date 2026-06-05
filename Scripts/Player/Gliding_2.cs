using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gliding_2 : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 5f;       // Minimum forward speed
    [SerializeField] private float maxSpeed = 60f;      // Maximum forward speed
    [SerializeField] private float thrustFactor = 20f;  // How much speed you gain when diving

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 1.5f;

        cameraTransform = Camera.main.transform;
        currentSpeed = minSpeed; // Start with some forward momentum
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
        //float mouseX = Input.GetAxis("Mouse X");
        float mouseX = Mouse.current.delta.x.ReadValue();

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

        // Corrected: diving increases speed
        float pitchMultiplier = Mathf.Clamp(pitch / 90f, -1f, 1f);

        currentSpeed += pitchMultiplier * thrustFactor * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        rb.linearVelocity = transform.forward * currentSpeed;
        rb.linearDamping = Mathf.Lerp(minDrag, dragFactor, 1f - Mathf.Abs(pitchMultiplier));
    }

    void ApplyGravityAndLift()
    {
        // Reduced gravity
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        // Optional lift based on speed
        rb.AddForce(Vector3.up * currentSpeed * liftMultiplier, ForceMode.Force);
    }

    public void AddSpeed(float amount)
    {
        currentSpeed += amount;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }
}
