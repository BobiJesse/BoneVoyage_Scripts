using System.Collections;
using UnityEngine;

public class StartBoost : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody playerRB;
    public Gliding gliderScript;
    public Vector3 boostUpForce = new Vector3(0f, 10f, 5f); // Upwards and slightly forward
    public float forwardBoost;
    public float delay = 3f;
    public float launchDuration = 1f; // How long the smooth launch lasts

    void Start()
    {
        // Disable gliding at the start
        if (gliderScript != null)
            gliderScript.enabled = false;

        // Call LaunchPlayer after 'delay' seconds
        Invoke(nameof(StartSmoothLaunch), delay);
    }

    void StartSmoothLaunch()
    {
        if (playerRB != null)
        {
            StartCoroutine(SmoothLaunch());
        }
    }

    IEnumerator SmoothLaunch()
    {
        float elapsed = 0f;
        Vector3 startVelocity = playerRB.linearVelocity;
        Vector3 targetVelocity = boostUpForce / launchDuration; // spread over duration

        while (elapsed < launchDuration)
        {
            elapsed += Time.deltaTime;
            // Lerp current velocity toward target
            playerRB.linearVelocity = Vector3.Lerp(startVelocity, targetVelocity, elapsed / launchDuration);
            yield return null;
        }

        // Ensure final velocity
        playerRB.linearVelocity = targetVelocity;

        yield return new WaitForSeconds(1f);
        // Enable gliding after launch
        if (gliderScript != null)
        {
            gliderScript.enabled = true;
            gliderScript.AddSpeed(forwardBoost);
        }
    }
}
