using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager _inputManager;
    private Transform _myTransform;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteR;
    public int state;
    private float _speed = 2.5f;
    private GameManager _gameManager;
    private bool _slideEnable = true;
    private bool _shotEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _myTransform = _rb.transform;
        _spriteR = GetComponent<SpriteRenderer>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(state);
        if(state < 2)
            Move(_inputManager.axisX);
    }

    public void Move(float axis)
    {
        if(axis > 0.5f)
        {
            _rb.velocity = new Vector2(1 * _speed, _rb.velocity.y);
        }
        else if(axis < -0.5f)
        {
            _rb.velocity = new Vector2(-1 * _speed, _rb.velocity.y);
        }
        else
        {
            if(state == 0)
                _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    public IEnumerator Slide()
    {
        float _slideDur = 0.3f;
        float _slideCD = 0.3f;
        float _impulse = 7f;

        if (state == 0 && _slideEnable)
        {
            state = 2;
            _slideEnable = false;
            
            if(_rb.velocity.x > 0)
            {
                _rb.AddForce(new Vector2(_impulse, 0), ForceMode2D.Impulse);
            }
            else if(_rb.velocity.x < 0)
            {
                _rb.AddForce(new Vector2(-_impulse, 0), ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(_slideDur);
            state = 0;

            yield return new WaitForSeconds(_slideCD);
            _slideEnable = true;
        }
    }

    private IEnumerator ShotTemp(Vector2 _impulse)
    {
        state = 2;
        _shotEnable = false;
        _slideEnable = false;

        float _shotCD = 0.3f;

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(_impulse, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_shotCD);
        _shotEnable = true;
        _slideEnable = true;
        if (_rb.velocity.y == 0)
            state = 0;
        else
            state = 1;
    }

    public void Shot(int dir)
    {
        float _impulse = 8f;

        if (_shotEnable)
        {
            switch (dir)
            {
                case 0:
                    if (!_spriteR.flipX)
                    {
                        StartCoroutine(ShotTemp(new Vector2(-_impulse, 0)));
                    }
                    else
                    {
                        StartCoroutine(ShotTemp(new Vector2(_impulse, 0)));
                    }
                    break;
                case 1:
                    StartCoroutine(ShotTemp(new Vector2(0, _impulse)));
                    break;
                case 2:
                    StartCoroutine(ShotTemp(new Vector2(_impulse, 0)));
                    break;
                case 3:
                    StartCoroutine(ShotTemp(new Vector2(-_impulse, 0)));
                    break;
                case 4:
                    if (state == 1)
                    {
                        StartCoroutine(ShotTemp(new Vector2(0, -_impulse)));
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
            state = 0;
    }
}
