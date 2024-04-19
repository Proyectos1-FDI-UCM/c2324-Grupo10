using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinaComponent : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    public Rigidbody2D rb;
    private CircleCollider2D _collTrigger;
    #endregion

    #region parameters
    int state = 0;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Pared"))
        {
            _collTrigger.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            state = 1;
        }
        else if(collision.gameObject.CompareTag("Player") && state == 1)
        {
            //daño o lo q sea
        }
        
    }
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        _collTrigger = GetComponent<CircleCollider2D>();
    }

    private void OnDisable()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = 0;
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
