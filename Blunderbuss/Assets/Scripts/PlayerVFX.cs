using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    #region references
    [SerializeField] GameObject _cura;
    [SerializeField] GameObject _recarga;
    [SerializeField] GameObject _pelotazo;
    [SerializeField] Animator _pelotazoAnim;

    private SFXCorsoManager _sfxCorso;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _sfxCorso = GetComponent<SFXCorsoManager>();
    }

    public IEnumerator CuraVFX()
    {
        _cura.SetActive(true);
        yield return new WaitForSeconds(1.58f);
        _cura.SetActive(false);
    }

    public IEnumerator RecargaVFX()
    {
        _recarga.SetActive(true);
        yield return new WaitForSeconds(0.31f);
        _recarga.SetActive(false);
    }

    public void ActivaLLama()
    {
        _pelotazo.SetActive(true);
        //_sfxCorso.CargaASFX();
    }

    public void DetonaLLama()
    {
        _pelotazoAnim.SetTrigger("CargadoFinal");
        _sfxCorso.CargaBSFX();
    }

    public void DesactivaLLama()
    {
        _pelotazo.SetActive(false);
    }
}
