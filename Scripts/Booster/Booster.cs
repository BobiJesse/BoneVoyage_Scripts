using UnityEngine;

public class Booster : MonoBehaviour
{
    public float boostAmount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected: " + other.name);
        Gliding glider = other.GetComponentInParent<Gliding>();
        if (glider != null)
        {
            glider.AddSpeed(boostAmount);
            Debug.Log("Boost applied!");
        }
        else
        {
            Debug.Log("No Gliding script found on " + other.name);
        }
    }
}
