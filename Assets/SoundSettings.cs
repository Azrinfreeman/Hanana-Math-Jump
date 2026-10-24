using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public AudioMixer mixer;

    [Header("Optional: sliders to show the saved volume")]
    public Slider musicSlider;
    public Slider sfxSlider;

    const float MinVolume = 0.0001f;

    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SfxVolume", 1f);
        if (musicSlider)
            musicSlider.SetValueWithoutNotify(music);
        if (sfxSlider)
            sfxSlider.SetValueWithoutNotify(sfx);
        SetMusic(music);
        SetSfx(sfx);
    }

    public void SetMusic(float sliderValue)
    {
        sliderValue = Mathf.Max(sliderValue, MinVolume);
        mixer.SetFloat("MusicParam", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSfx(float sliderValue)
    {
        sliderValue = Mathf.Max(sliderValue, MinVolume);
        mixer.SetFloat("SfxParam", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SfxVolume", sliderValue);
    }
}
