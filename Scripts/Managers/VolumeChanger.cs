using UnityEngine;
using UnityEngine.Audio;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    private void Start()
    {

    }


    public void SetVolume(float sliderValue)
    {
        myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}
