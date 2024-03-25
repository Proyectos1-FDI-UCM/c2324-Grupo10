using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SerpicolAnimator : MonoBehaviour
{
    #region references
    private SerpicolManager _serAttaks;
    public Animator serpicolAnimator;
    #endregion

    #region parameters
    #endregion

    #region methods
    public void IdleAnimation()
    {
        serpicolAnimator.SetInteger("AnimState", 0);
    }

    public void BocaoAnimation()
    {
        serpicolAnimator.SetInteger("AnimState", 4);
    }

    public void HipnosisAnimation()
    {
        serpicolAnimator.SetInteger("AnimState", 3);
    }

    public void GaposAnimation()
    {
        serpicolAnimator.SetInteger("AnimState", 2);
    }

    public void CaracolaAnimation()
    {
        serpicolAnimator.SetInteger("AnimState", 1);
    }

    public void Suelo(bool state)
    {
        serpicolAnimator.SetBool("suelo",state);
    }

    #endregion

    void Start()
    {
        _serAttaks = GetComponent<SerpicolManager>();
        serpicolAnimator = GetComponent<Animator>();

        if (_serAttaks == null || serpicolAnimator == null)
        {
            Debug.LogError("Comprueba componentes.");
        }

        serpicolAnimator.SetBool("suelo", value: true);
    }
}