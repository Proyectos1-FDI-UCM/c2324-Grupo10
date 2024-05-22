using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCorsoManager : MonoBehaviour
{
    #region references
    private BalasManager _balasManager;
    private PlayerVFX _vfx;

    public AudioSource _sfxCorsoAS;
    [SerializeField] public AudioClip _sfxDisparoAire;
    [SerializeField] public AudioClip _sfxDisparoSuelo;
    [SerializeField] public AudioClip _sfxPelotazo;
    [SerializeField] public AudioClip _sfxHeal;
    [SerializeField] public AudioClip _sfxCargaPelotazoA;
    [SerializeField] public AudioClip _sfxCargaPelotazoB;
    [SerializeField] public AudioClip _sfxDesliz;
    [SerializeField] public AudioClip _sfxRecarga;
    #endregion

    void Start()
    {
        _balasManager = GetComponent<BalasManager>();
        _vfx = GetComponent<PlayerVFX>();
    }

    public void DeslizSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxDesliz, 2f);
    }
    public void DisparoSFX(bool suelo)
    {
        if (_balasManager.BalaQuantity > 0)
        {
            if (suelo)
            {
                _sfxCorsoAS.PlayOneShot(_sfxDisparoSuelo, 0.6f);
            }
            else
            {
                _sfxCorsoAS.PlayOneShot(_sfxDisparoAire, 0.6f);
            }
        }
    }
    public void RecargaSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxRecarga, 0.7f);
    }
    public void HealSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxHeal, 0.8f);
    }
    public void PelotazoSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxPelotazo, 0.8f);
    }
    public void CargaASFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxCargaPelotazoA, 0.1f);
    }
    public void CargaBSFX()
    {
        _sfxCorsoAS.PlayOneShot(_sfxCargaPelotazoB, 0.75f);
    }
}


