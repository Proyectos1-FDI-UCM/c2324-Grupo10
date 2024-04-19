using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSerpicolManager : MonoBehaviour
{
    #region references

    public AudioSource _sfxSerpicolAS;
    [SerializeField] public AudioClip _sfxBocao;
    [SerializeField] public AudioClip _sfxGolpePared;
    [SerializeField] public AudioClip _sfxPrerrayo;
    [SerializeField] public AudioClip _sfxRayo;
    [SerializeField] public AudioClip _sfxCaracola;
    #endregion

    public void BocaoSFX()
    {
        _sfxSerpicolAS.PlayOneShot(_sfxBocao, 0.7f);
    }
    public void GolpeParedSFX()
    {
        _sfxSerpicolAS.PlayOneShot(_sfxGolpePared, 0.2f);
    }
    public void PrerrayoSFX()
    {
        _sfxSerpicolAS.PlayOneShot(_sfxPrerrayo, 0.7f);
    }
    public void RayoSFX()
    {
        _sfxSerpicolAS.PlayOneShot(_sfxRayo, 0.7f);
    }
    public void CaracolaSFX()
    {
        _sfxSerpicolAS.PlayOneShot(_sfxCaracola, 0.7f);
    }
}
