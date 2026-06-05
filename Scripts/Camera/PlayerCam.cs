using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float maxViewRange = 60f;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -4);

    private float yaw;
    private float pitch;

    private void Start()
    {
        // Start at current rotation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // Optional: lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
        FollowPlayer();
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxViewRange, maxViewRange);
    }

    private void FollowPlayer()
    {
        // Calculate desired rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate desired position
        Vector3 desiredPosition = playerTransform.position + rotation * offset;

        // Smoothly move to that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Apply rotation last (avoids jitter)
        transform.rotation = rotation;
    }
}
