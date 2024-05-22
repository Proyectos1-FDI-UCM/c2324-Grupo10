using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    Slider musicaSlider;
    [SerializeField]
    Slider sfxSlider;

    void Start()
    {
        float musicaVolume;
        float sfxVolume;

        audioMixer.GetFloat("Music", out musicaVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        musicaSlider.value = musicaVolume;
        sfxSlider.value = sfxVolume;

        musicaSlider.onValueChanged.AddListener(SetMusicaVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicaVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
}