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
}
