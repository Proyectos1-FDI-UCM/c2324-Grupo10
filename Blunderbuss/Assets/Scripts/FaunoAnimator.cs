using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaunoAnimator : MonoBehaviour
{
    private Animator _faunoAnim;
    // Start is called before the first frame update
    void Start()
    {
        _faunoAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cuchillada()
    {
        _faunoAnim.SetTrigger("Cuchillada");
    }

    public void CuchilladaSuelo()
    {
        _faunoAnim.SetTrigger("CuchilladaSuelo");
    }

    public void Correr()
    {
        _faunoAnim.SetTrigger("Correr");
    }

    public void Saltar()
    {
        _faunoAnim.SetTrigger("Saltar");
    }

    public void SaltarEnd()
    {
        _faunoAnim.SetTrigger("Saltar_End");
    }

    public void CorrerEnd()
    {
        _faunoAnim.SetTrigger("Correr_End");
    }

    public void Aliento()
    {
        _faunoAnim.SetTrigger("Aliento");
    }
}

