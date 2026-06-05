using UnityEngine;

public class ConstantRotating : MonoBehaviour
{
    public Vector3 rotatingDegreesPerSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotatingDegreesPerSecond * Time.deltaTime);
    }
}
