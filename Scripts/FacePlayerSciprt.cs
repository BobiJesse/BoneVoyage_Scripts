using Unity.VisualScripting;
using UnityEngine;

public class FacePlayerSciprt : MonoBehaviour
{
    public GameObject cam;
    [SerializeField] bool IsBalloon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").gameObject;
        if (IsBalloon)
        {
            if (GameObject.FindWithTag("Boss"))
            {
                cam = GameObject.FindWithTag("Boss").gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
    }
}
