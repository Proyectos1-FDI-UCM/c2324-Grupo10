using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private InputManager _inputManager;
    private BalasManager _balasManager;
    [SerializeField]
    private CameraController _cameraController;
    private ShotManager _shotManager;

    public Transform _myTransform;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteR;
    #endregion

    #region parameters
    public int state; //Estado 0: Suelo; Estado 1: Aire; Estado 2: Pared; Estado 3: Mov Bloqueado/Evento; Estado 4: Deslizamiento;

    private float _speedGround = 3f;
    private float _speedAir = 3f;
    private float _speedWall = 2f;
    private float _airForce = 500f;
    private bool _slideEnable = true;
    public bool shotEnable = true;
    public float _groundHeight;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _myTransform = _rb.transform;
        _spriteR = GetComponent<SpriteRenderer>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
        _balasManager = GetComponent<BalasManager>();
        _shotManager = GetComponent<ShotManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(state < 3)
            Move(_inputManager.axisX);

        print(state + " " + _rb.velocity + " " + _myTransform.position.y);
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
        else if (state == 1 || state == 2)
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
            state = 4;
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
            if (state == 4)
                state = 0;

            yield return new WaitForSeconds(_slideCD);
            _slideEnable = true;
        }
    }

    private IEnumerator ShotTemp(Vector2 _stop, Vector2 _impulse)
    {
        StartCoroutine(_cameraController.ShakeBegin());

        state = 3;
        shotEnable = false;
        _slideEnable = false;

        float _shotCD = 0.3f;

        _rb.velocity = _stop;
        _rb.AddForce(_impulse, ForceMode2D.Impulse);
        _balasManager.restaBala();

        yield return new WaitForSeconds(_shotCD);
        shotEnable = true;
        _slideEnable = true;
        if (_myTransform.position.y >= _groundHeight)
            state = 1;
        else
            state = 0;
    }

    public void Shot(int dir)
    {
        float _impulse;
        bool _suelo;

        if (shotEnable && _balasManager.BalaQuantity > 0)
        {
            if (state != 2)
            {
                switch (dir)
                {
                    case 0:
                        if (state == 0)
                        {
                            _suelo = false;
                            _impulse = 700;
                        }
                        else
                        {
                            _suelo = false;
                            _impulse = 1000f;
                        }

                        if (!_spriteR.flipX)
                        {
                            StartCoroutine(_shotManager.FireSpawn(_suelo, Vector2.right, Quaternion.Euler(0, 0, 90)));
                            StartCoroutine(ShotTemp(Vector2.zero, new Vector2(-_impulse, 0)));
                        }
                        else
                        {
                            StartCoroutine(_shotManager.FireSpawn(_suelo, Vector2.left, Quaternion.Euler(0 ,0, -90)));
                            StartCoroutine(ShotTemp(Vector2.zero, new Vector2(_impulse, 0)));
                        }
                        break;
                    case 1:
                        if (state == 0)
                        {
                            _suelo = true;
                            _impulse = 900;
                        }
                        else if (state == 4)
                        {
                            _suelo = true;
                            _impulse = 1200f;
                        }
                        else
                        {
                            _suelo = false;
                            _impulse = 800f;
                        }

                        StartCoroutine(_shotManager.FireSpawn(_suelo, Vector2.down, Quaternion.identity));
                        StartCoroutine(ShotTemp(new Vector2(_rb.velocity.x, 0), new Vector2(0, _impulse)));

                        break;
                    case 2:
                        _impulse = 1400f;
                        if (state == 1)
                        {
                            _suelo = false;
                            StartCoroutine (_shotManager.FireSpawn(_suelo, Vector2.up, Quaternion.Euler(0, 0, 180)));
                            StartCoroutine (ShotTemp(Vector2.zero, new Vector2(0, -_impulse)));
                        }
                        break;
                }
            }
            else
            {
                _impulse = 1000f;
                _suelo = true;

                if (_spriteR.flipX)
                {
                    StartCoroutine(_shotManager.FireSpawn(_suelo, Vector2.left, Quaternion.Euler(0, 0, -90)));
                    StartCoroutine(ShotTemp(Vector2.zero, new Vector2(_impulse, _impulse / 2f)));
                }


                else
                {
                    StartCoroutine(_shotManager.FireSpawn(_suelo, Vector2.right, Quaternion.Euler(0, 0, 90)));
                    StartCoroutine(ShotTemp(Vector2.zero, new Vector2(-_impulse, _impulse / 2f)));
                }
                    
            }
        }
    }

    public IEnumerator Recarga()
    {
        float _reloadCD = 0.75f;
        if (state == 0 && _balasManager.BalaQuantity != _balasManager.maxBalas)
        {
            state = 3;
            _rb.velocity = Vector2.zero;
            StartCoroutine(_balasManager.Recargar());
            yield return new WaitForSeconds(_reloadCD);
            state = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
            state = 0;

        if (collision.gameObject.tag == "Pared")
        {
            if (collision.contacts[0].normal.x < 0 && _myTransform.position.y >= _groundHeight)
                _spriteR.flipX = false;
            else if (_myTransform.position.y >= _groundHeight)
                _spriteR.flipX = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared" && state != 0 && state != 4)
        {
            state = 2;

            if (_rb.velocity.y < 0)
                _rb.velocity = new Vector2(_rb.velocity.x, -_speedWall);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared" && state != 0 && state != 4)
        {
            state = 1;
        }
    }
}
