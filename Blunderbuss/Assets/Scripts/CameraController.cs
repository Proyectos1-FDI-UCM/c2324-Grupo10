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
    float limitLeft;
    [SerializeField]
    float limitRight;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        camState = 1;
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (camState == 1)
            CameraFollow();
        else if (camState == 2)
            CameraShake();
    }

    private void CameraFollow()
    {
        float _cameraPosX = Mathf.Clamp(_targetTransform.position.x, limitLeft, limitRight);  
        
        _myTransform.position = new Vector3 (_cameraPosX, _offsetY, _offsetZ);
    }

    private void CameraShake()
    {
        float _cameraPosX = Mathf.Clamp(_targetTransform.position.x, limitLeft, limitRight);
        float _shakeamtX = 0.05f;
        float _shakeamtY = 0.05f;


        float _shakeRX;
        float _shakeRY;

        float _nextShake = 0.0f;
        float _frequencyShake = 20f;

        print(Time.time);

        if (Time.time > _nextShake)
        {
            _nextShake = Time.time + _frequencyShake;

            _shakeRX = Random.Range(-1f, 1f) * _shakeamtX;
            _shakeRY = Random.Range(-1f, 1f) * _shakeamtY;

            _myTransform.position = new Vector3(_cameraPosX + _shakeRX, _offsetY + _shakeRY, _offsetZ);
        }
    }

    public IEnumerator ShakeBegin()
    {
        float timeShake = 0.3f;

        camState = 2;

        yield return new WaitForSeconds(timeShake);

        camState = 1;
    }
}