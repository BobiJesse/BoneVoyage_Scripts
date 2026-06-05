using UnityEngine;

public class DestroyMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(BGAudioManager.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
