using UnityEngine;

public class RollingAsteroid : MonoBehaviour
{
    [Header("Rolling Settings")]
    public Transform targetPoint;      // The point the asteroid moves toward
    public float moveSpeed = 10f;
    public float spinSpeed = 180f;

    private bool isRolling = false;
    private Vector3 moveDirection;     // fixed direction in world space

    void Update()
    {
        if (!isRolling || targetPoint == null)
            return;

        // Move in a fixed world direction, unaffected by rotation
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Spin visually
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);

        // Optional: stop when close enough
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            isRolling = false;
        }
    }

    public void StartRolling()
    {
        if (targetPoint == null)
        {
            Debug.LogWarning("RollingAsteroid: No target point assigned!");
            return;
        }

        isRolling = true;

        // Save direction *once* at the start — world space direction
        moveDirection = (targetPoint.position - transform.position).normalized;
    }
}
