using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissileLookAtScript : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    public float speed;
    public float speedMultiplier = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);


        //Debug.Log(distance);
        if (distance > 80)
        {
            rb.AddForce(transform.forward * (speed * speedMultiplier));
        }
        else
        {
            rb.AddForce(transform.forward * speed);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
