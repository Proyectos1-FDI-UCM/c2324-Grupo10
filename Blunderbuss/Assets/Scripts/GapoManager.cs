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
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        /*spriteR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();*/
    }

    void Update()
    {
        Rotacion();
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
