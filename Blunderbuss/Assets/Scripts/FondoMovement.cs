using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovement : MonoBehaviour
{
    #region references
    private Camera _camera;
    private Rigidbody2D _rb;
    private CameraController _cameraController;
    #endregion

    #region parameters
    [SerializeField]
    private Vector2 _fondoVelocity;

    private Vector3 _offset;
    #endregion

    #region methods

    #endregion

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _rb = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        _cameraController = _camera.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_camera.transform.position.x != _cameraController.limitLeft && _camera.transform.position.x != _cameraController.limitRight)
        {
            _offset = (_rb.velocity.x * 0.1f) * -_fondoVelocity * Time.deltaTime;
            transform.position += _offset;
        }
    }
}
