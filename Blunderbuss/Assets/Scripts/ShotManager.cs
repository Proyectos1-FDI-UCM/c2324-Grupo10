using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    #region references
    private PlayerManager _playerManager;
    public Transform _myTransform;
    public GameObject _escopetaColliderAire;
    public GameObject _escopetaColliderSuperficie;

    #endregion

    #region parameters
    #endregion

    void Start()
    {
        _escopetaColliderAire.SetActive(false);
        _escopetaColliderSuperficie.SetActive(false);
    }


    void Update()
    {
        /*if (_playerManager.state == 2 || (_playerManager.state == 4 && dir == 1) || (_playerManager.state == 0 && dir == 1))
        {
            _escopetaColliderSuperficie.SetActive(true);
        }
        else
        {
            _escopetaColliderAire.SetActive(true);
        }*/
    }
}
