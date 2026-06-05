using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElytraGliding : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 30f;
    [SerializeField] private float maxThrustSpeed = 60f;
    [SerializeField] private float minThrustSpeed = 5f;
    [SerializeField] private float thrustFactor = 10f;

    [Header("Drag Settings")]
    [SerializeField] private float dragFactor = 1f;
    [SerializeField] private float minDrag = 0.2f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float tiltStrength = 45f;
    [SerializeField] private float tiltReturnSpeed = 2f;

    [Header("Pitch Influence")]
    [SerializeField] private float lowPercent = 0.1f;
    [SerializeField] private float highPercent = 1f;

    private float currentThrustSpeed;
    private float tiltValue;
    private Transform cameraTransform;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 2f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        cameraTransform = Camera.main.transform;
        currentThrustSpeed = baseSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate()
    {
        ApplyRotation();
        ApplyGlidePhysics();
    }

    void ApplyRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Smooth tilt based on mouse movement
        tiltValue = Mathf.Lerp(tiltValue, mouseX * -tiltStrength, Time.fixedDeltaTime * tiltReturnSpeed);
        tiltValue = Mathf.Clamp(tiltValue, -tiltStrength, tiltStrength);

        // Get camera pitch/yaw safely
        Vector3 camAngles = cameraTransform.eulerAngles;
        float pitch = camAngles.x;
        if (pitch > 180) pitch -= 360f; // normalize

        Quaternion targetRot = Quaternion.Euler(pitch, camAngles.y, tiltValue);

        // Smooth physics-friendly rotation
        Quaternion smoothRot = Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothRot);
    }

    void ApplyGlidePhysics()
    {
        float pitch = transform.eulerAngles.x;
        if (pitch > 180) pitch -= 360f;

        float pitchRatio = -Mathf.Sin(pitch * Mathf.Deg2Rad);
        float accelPercent = (pitch < -45f) ? highPercent : lowPercent;

        currentThrustSpeed += pitchRatio * accelPercent * thrustFactor * Time.fixedDeltaTime;
        currentThrustSpeed = Mathf.Clamp(currentThrustSpeed, minThrustSpeed, maxThrustSpeed);

        rb.AddRelativeForce(Vector3.forward * currentThrustSpeed, ForceMode.Force);

        float mappedDrag = Mathf.Lerp(minDrag, dragFactor, Mathf.Abs(Mathf.Cos(pitch * Mathf.Deg2Rad)));
        rb.linearDamping = mappedDrag;
    }
}
