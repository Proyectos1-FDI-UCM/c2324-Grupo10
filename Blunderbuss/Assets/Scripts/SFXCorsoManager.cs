using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCorsoManager : MonoBehaviour
{
    #region references
    private BalasManager _balasManager;

    public AudioSource _sfxCorsoAS;
    [SerializeField] public AudioClip _sfxDisparoAire;
    [SerializeField] public AudioClip _sfxDisparoSuelo;
    [SerializeField] public AudioClip _sfxPelotazo;
    [SerializeField] public AudioClip _sfxHeal;
    [SerializeField] public AudioClip _sfxCargaPelotazoA;
    [SerializeField] public AudioClip _sfxCargaPelotazoB;
    [SerializeField] public AudioClip _sfxDesliz;
    #endregion

    void Start()
    {
        _balasManager = GetComponent<BalasManager>();
    }

    public void DeslizSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxDesliz, 1f);
    }
    public void DisparoSFX(bool suelo)
    {
        if (_balasManager.BalaQuantity > 0)
        {
            if (suelo)
            {
                _sfxCorsoAS.PlayOneShot(_sfxDisparoSuelo, 1f);
            }
            else
            {
                _sfxCorsoAS.PlayOneShot(_sfxDisparoAire, 1f);
            }
        }
    }
    public void HealSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxHeal, 1f);
    }
    public void PelotazoSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxPelotazo, 1f);
    }
    public void CargaASFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxCargaPelotazoA, 1f);
    }
    public void CargaBSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxCargaPelotazoB, 1f);
    }
}


