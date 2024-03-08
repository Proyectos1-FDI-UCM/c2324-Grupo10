using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovement : MonoBehaviour
{
    #region references
    private Rigidbody2D _playerRB;
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
        _playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _offset = (_playerRB.velocity.x * 0.1f) * -_fondoVelocity * Time.deltaTime;
        transform.position += _offset;
    }
}
