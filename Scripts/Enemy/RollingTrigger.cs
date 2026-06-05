using UnityEngine;

public class RollingTrigger : MonoBehaviour
{
    public RollingAsteroid asteroid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            asteroid.StartRolling();
            Debug.Log("Asteroid started rolling!");
        }
    }
}
