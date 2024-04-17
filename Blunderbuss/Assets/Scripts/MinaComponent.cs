using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaComponent : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _collFisico;
    [SerializeField] private CircleCollider2D _collTrigger;
    #endregion

    #region parameters
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            _collFisico.enabled = true;
            _collTrigger.enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (collision.gameObject.CompareTag("Pared"))
        {
            _collFisico.enabled = true;
            _collTrigger.enabled = true;
        }
    }
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
