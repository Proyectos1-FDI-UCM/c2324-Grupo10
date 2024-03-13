using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovement : MonoBehaviour
{
    #region references
    private Camera _camera;
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
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _offset = (_camera.velocity.x * 0.1f) * -_fondoVelocity * Time.deltaTime;
        transform.position += _offset;
    }
}
