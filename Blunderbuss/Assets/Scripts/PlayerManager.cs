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
    private float _speedGround = 2.5f;
    private float _speedAir = 5f;
    private float _speedWall = 2f;
    private float _airForce = 500f;
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
        //print(state);
        if(state < 2)
            Move(_inputManager.axisX);

        print(state);
    }

    public void Move(float axis)
    {
        if (state == 0)
        {
            if (axis > 0.5f)
            {
                _rb.velocity = new Vector2(_speedGround, _rb.velocity.y);
            }
            else if (axis < -0.5f)
            {
                _rb.velocity = new Vector2(-_speedGround, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
        else if (state == 1 || state == 4)
        {
            if (axis > 0.5f && _rb.velocity.x < _speedAir)
            {
                _rb.AddForce(new Vector2(_airForce, 0), ForceMode2D.Force);
            }
            else if (axis < -0.5f && _rb.velocity.x > -_speedAir)
            {
                _rb.AddForce(new Vector2(-_airForce, 0), ForceMode2D.Force);
            }
        }
    }

    public IEnumerator Slide()
    {
        float _slideDur = 0.3f;
        float _slideCD = 0.3f;
        float _impulse = 560f;

        if (state == 0 && _slideEnable)
        {
            state = 3;
            _slideEnable = false;
            
            if(!_spriteR.flipX)
            {
                _rb.AddForce(new Vector2(_impulse, 0), ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(new Vector2(-_impulse, 0), ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(_slideDur);
            if (state == 3)
                state = 0;

            yield return new WaitForSeconds(_slideCD);
            _slideEnable = true;
        }
    }

    private IEnumerator ShotTemp(Vector2 _stop, Vector2 _impulse)
    {
        state = 2;
        _shotEnable = false;
        _slideEnable = false;

        float _shotCD = 0.3f;

        _rb.velocity = _stop;
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
        float _impulse;

        if (_shotEnable)
        {
            if (state != 4)
            {
                switch (dir)
                {
                    case 0:
                        if (state == 0)
                            _impulse = 600;
                        else
                            _impulse = 800f;

                        if (!_spriteR.flipX)
                        {
                            StartCoroutine(ShotTemp(new Vector2(_rb.velocity.x, 0), new Vector2(-_impulse, 0)));
                        }
                        else
                        {
                            StartCoroutine(ShotTemp(new Vector2(_rb.velocity.x, 0), new Vector2(_impulse, 0)));
                        }
                        break;
                    case 1:
                        if (state == 0)
                            _impulse = 900;
                        else if (state == 3)
                            _impulse = 1200f;
                        else
                            _impulse = 800f;

                        StartCoroutine(ShotTemp(new Vector2(_rb.velocity.x, 0), new Vector2(0, _impulse)));

                        break;
                    case 2:
                        _impulse = 1400f;
                        if (state == 1)
                        {
                            StartCoroutine(ShotTemp(Vector2.zero, new Vector2(0, -_impulse)));
                        }
                        break;
                }
            }
            else
            {
                //StartCoroutine(ShotTemp(Vector2.zero, new Vector2(0, -_impulse)));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
            state = 0;

        if (collision.gameObject.tag == "Pared" && state != 0)
        {
            state = 4;
            Physics2D.gravity = new Vector2(0, -_speedWall);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared")
        {
            if (_rb.velocity.y != 0)
                state = 1;
            Physics2D.gravity = new Vector2(0, -19.62f);
        }
    }
}
