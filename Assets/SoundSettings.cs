using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("MusicParam", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfx(float sliderValue)
    {
        mixer.SetFloat("SfxParam", Mathf.Log10(sliderValue) * 20);
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
