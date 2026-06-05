using UnityEngine;

public class TwoPointMovement : MonoBehaviour
{
    private float time = 0f;
    public float period = 2f;
    public Vector3 positionOffset;
    private Vector3 positionInit;

    void Start()
    {
        positionInit = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        while (time > period) time -= period;

        transform.position = positionInit + positionOffset * Mathf.Pow(Mathf.Sin(time / period * Mathf.PI), 2.0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + positionOffset);
        Gizmos.DrawSphere(transform.position + positionOffset, 1.0f);
    }
}
