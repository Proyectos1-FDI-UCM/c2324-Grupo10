using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaunoAnimator : MonoBehaviour
{
    public Animator faunoAnim;
    // Start is called before the first frame update
    void Start()
    {
        faunoAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cuchillada()
    {
        faunoAnim.SetTrigger("Cuchillada");
    }

    public void CuchilladaSuelo()
    {
        faunoAnim.SetTrigger("CuchilladaSuelo");
    }

    public void Correr()
    {
        faunoAnim.SetTrigger("Correr");
    }

    public void Saltar()
    {
        faunoAnim.SetTrigger("Saltar");
    }

    public void SaltarEnd()
    {
        faunoAnim.SetTrigger("Saltar_End");
    }

    public void CorrerEnd()
    {
        faunoAnim.SetTrigger("Correr_End");
    }

    public void Aliento()
    {
        faunoAnim.SetTrigger("Aliento");
    }

    public void Camina()
    {
        faunoAnim.SetTrigger("Walk");
    }
}

