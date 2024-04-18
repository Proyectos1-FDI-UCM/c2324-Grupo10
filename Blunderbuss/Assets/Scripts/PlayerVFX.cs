using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] GameObject _cura;
    [SerializeField] GameObject _recarga;
    [SerializeField] GameObject _pelotazo;
    [SerializeField] Animator _pelotazoAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        yield return new WaitForSeconds(0.41f);
        _recarga.SetActive(false);
    }

    public void ActivaLLama()
    {
        _pelotazo.SetActive(true);
    }

    public void DetonaLLama()
    {
        _pelotazoAnim.SetTrigger("CargadoFinal");
    }

    public void DesactivaLLama()
    {
        _pelotazo.SetActive(false);
    }
}
