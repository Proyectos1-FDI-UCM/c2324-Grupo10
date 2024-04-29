using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXFaunoManager : MonoBehaviour
{
    #region references

    public AudioSource _sfxFaunoAS;
    [SerializeField] public AudioClip _sfxCuchilla;
    [SerializeField] public AudioClip _sfxCuchillada;
    [SerializeField] public AudioClip _sfxMina;
    [SerializeField] public AudioClip _sfxRujido1;
    [SerializeField] public AudioClip _sfxRujido2;
    [SerializeField] public AudioClip _sfxPared;
    #endregion

    public void CuchillaSFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxCuchilla, 1);
    }
    public void CuchilladaSFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxCuchillada, 0.2f);
    }
    public void MinaSFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxMina, 0.7f);
    }
    public void Rugido1SFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxRujido1, 0.1f);
    }
    public void Rugido2SFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxRujido2, 0.4f);
    }

    public void ParedSFX()
    {
        _sfxFaunoAS.PlayOneShot(_sfxPared, 0.4f);
    }
}
