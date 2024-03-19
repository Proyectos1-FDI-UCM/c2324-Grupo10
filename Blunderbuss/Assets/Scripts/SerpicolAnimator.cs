using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SerpicolAnimator : MonoBehaviour
{
    #region references
    private SerpicolAttacks _serAttaks;
    public Animator _serpicolAnimator;
    #endregion

    #region parameters;
    #endregion

    #region methods;
    #endregion
    void Start()
    {
        _serAttaks = GetComponent<SerpicolAttacks>();
        _serpicolAnimator = GetComponent<Animator>();

        if (_serAttaks == null || _serpicolAnimator == null)
        {
            Debug.LogError("Comprueba componentes.");
        }

        _serpicolAnimator.SetBool("suelo", value: true);
    }

    public void IdleAnimation()
    {
        _serpicolAnimator.SetInteger("AnimState", 0);
    }

    public void BocaoAnimation()
    {
        _serpicolAnimator.SetInteger("AnimState", 4);
    }

    public void HipnosisAnimation()
    {
        _serpicolAnimator.SetInteger("AnimState", 3);
    }

    public void GaposAnimation()
    {
        _serpicolAnimator.SetInteger("AnimState", 2);
    }

    public void CaracolaAnimation()
    {
        _serpicolAnimator.SetInteger("AnimState", 1);
    }
}