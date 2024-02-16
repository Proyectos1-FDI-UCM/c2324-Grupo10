using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    [SerializeField]
    private Transform _targetTransform;
    #endregion

    #region parameters
    private float _offsetY = 0;
    private float _offsetZ = -10;

    public int camState;

    [SerializeField]
    float limitLeft = 3.8f;
    [SerializeField]
    float limitRight = 10.8f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        camState = 0;
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (camState == 0)
            CameraFollow();
    }

    void CameraFollow()
    {
        float _cameraPosX = Mathf.Clamp(_targetTransform.position.x, limitLeft, limitRight);  
        
        _myTransform.position = new Vector3 (_cameraPosX, _offsetY, _offsetZ);

    }
}