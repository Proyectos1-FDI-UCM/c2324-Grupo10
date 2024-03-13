using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private Transform _myTransform;
    private Transform _targetTransform;
    #endregion

    #region parameters
    private float _offsetY = 0;
    private float _offsetZ = -10;

    public int camState;

    public float limitLeft;

    public float limitRight;

    private float _nextShake = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        camState = 1;
        _gameManager = GameManager.Instance;
        _targetTransform = _gameManager.Player.transform;
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (camState == 1)
            CameraFollow();
        else if (camState == 2)
            CameraShake(0.07f);
        else if (camState == 3)
            CameraShake(0.22f);
    }

    private void CameraFollow()
    {
        float _cameraPosX = Mathf.Clamp(_targetTransform.position.x, limitLeft, limitRight);  
        
        _myTransform.position = new Vector3 (_cameraPosX, _offsetY, _offsetZ);
    }

    private void CameraShake(float shakeAmt)
    {
        float _cameraPosX = Mathf.Clamp(_targetTransform.position.x, limitLeft, limitRight);

        float _shakeRX;
        float _shakeRY;

        float _frequencyShake = 0.006f;

        if (Time.time > _nextShake)
        {
            _nextShake = Time.time + _frequencyShake;

            _shakeRX = Random.Range(-1f, 1f) * shakeAmt;
            _shakeRY = Random.Range(-1f, 1f) * shakeAmt;

            _myTransform.position = new Vector3(_cameraPosX + _shakeRX, _offsetY + _shakeRY, _offsetZ);
        }
        else
            _myTransform.position = new Vector3(_cameraPosX, _offsetY, _offsetZ);

    }

    public IEnumerator ShakeBegin(int cam)
    {
        float timeShake = 0.3f;

        camState = cam;

        yield return new WaitForSeconds(timeShake);

        camState = 1;
    }
}