using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapoManager : MonoBehaviour
{
    public SpriteRenderer spriteR;
    public Rigidbody2D rb;
    private Transform _myTransform;

    Vector2 velocityNorm;
    float angulo;
    bool baba = false;
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        if (_myTransform.position.y >= 7)
        {
            baba = true;
            _myTransform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    void Update()
    {
        if (!baba)
            Rotacion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            gameObject.SetActive(false);
    }

    private void Rotacion()
    {
        velocityNorm = rb.velocity.normalized;
        angulo = Mathf.Atan2(velocityNorm.y, velocityNorm.x) * Mathf.Rad2Deg;
        _myTransform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }
}
