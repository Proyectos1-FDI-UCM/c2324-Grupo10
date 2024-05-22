using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaunoManager : MonoBehaviour
{
    #region references
    [SerializeField] private FaunoConfig _configuration; //configuración externa de las variables que va a usar el fauno

    [SerializeField] private GameObject _conjCuch;

    [SerializeField] private GameObject _escupeMina;

    [SerializeField] private CuchillaManager[] _cuchillaManagers = new CuchillaManager[14];

    [SerializeField] private Vector2[] _vectsMina = new Vector2[4];

    [SerializeField] private GameObject _mina;

    private ObstacleComponent _obstacleComponent;
    private SFXFaunoManager _sfxFauno;
    private BossHealth _bossHealth;
    private Rigidbody2D _faunoRB;
    private Transform _myTransform;
    private SpriteRenderer _spriteF;
    private BoxCollider2D _boxColl;
    private GameObject _player;
    private SceneManagerS _sceneManagerS;

    [SerializeField]
    float _paredIzq;
    [SerializeField]
    float _paredDer;

    private CameraController _camera;
    private FaunoAnimator _faunoAnimator;

    private int _direction = -1;
    #endregion

    #region parameters
    private bool _alive = true; //está vivo?
    private bool _hitWall = false; //choca con pared?
    private bool _hitGround = true;//choca con suelo?
    private bool _first = true;
    Vector3 vectorPosCuchilla;
    private bool _cuchillasSuelo = false;

    private int _numVectMinas = 0;

    private int _state = 0; //variable de control de estados del fauno
                            //=0 ; en ataque
                            //=1 ; caminando
    #endregion

    #region methods
    private void Orient()
    {
        if (_alive)
        {
            Vector3 dist = _player.transform.position - _myTransform.position;

            if (dist.x < -2)
            {
                _direction = -1;

                if ((_faunoAnimator.faunoAnim.GetCurrentAnimatorStateInfo(0).IsName("Fauno_Idle") || _faunoAnimator.faunoAnim.GetCurrentAnimatorStateInfo(0).IsName("Fauno_Walk")) && _spriteF.flipX)
                {
                    Mirror();
                }
            }

            else if (dist.x > 2)
            {
                _direction = 1;

                if ((_faunoAnimator.faunoAnim.GetCurrentAnimatorStateInfo(0).IsName("Fauno_Idle") || _faunoAnimator.faunoAnim.GetCurrentAnimatorStateInfo(0).IsName("Fauno_Walk")) && !_spriteF.flipX)
                {
                    Mirror();
                }
            }
        }
    }

    private void Mirror()
    {
        _spriteF.flipX = !_spriteF.flipX;
    }

    private Vector3 SeparaPared(int dir)
    {
        Vector3 aux = new Vector3 (-dir * 2, 0, 0);

        return aux;
    }

    public void Muerte()
    {
        _alive = false;

        GameManager.Instance.faunoDead = true;

        while(_cuchillasSuelo && !_hitGround)
        {
            return;
        }

        StopAllCoroutines();

        StartCoroutine(MuerteAnim());
    }

    private IEnumerator MuerteAnim()
    {
        _boxColl.enabled = false;

        _faunoAnimator.Grito();

        yield return new WaitForSeconds(1);
        StartCoroutine(Rugido());

        yield return new WaitForSeconds(3f);

        Vector3 tumba = new Vector3(_myTransform.position.x, _myTransform.position.y - 7, 0);
        float tumbaSpeed = 1.2f;

        while (_myTransform.position.y != tumba.y)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, tumba, tumbaSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        if (GameManager.Instance.serpicolDead)
        {
            _sceneManagerS.Escena = "EscenaFin";
        }
        _sceneManagerS.CargarEscena();
    }

    private int SetDirection() // calcula si el jugador esta a la derecha o izq del fauno.
    {
        float setDir = (_player.transform.position.x - _myTransform.position.x);
        if(setDir > 0) { return 1; } // >0 es hacia izq
        else return -1;
    }

    private float Distancia()
    {
        return _player.transform.position.x - _myTransform.position.x;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pared"))
        {
            _hitWall = true;
        }
        if (other.CompareTag("Suelo"))
        {
            _faunoRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            _hitGround = true;
            if (!_first)
                _faunoAnimator.SaltarEnd();
            else
                _first = false;
        }
    }


    #region attacks
    private IEnumerator Walk()
    {
        _faunoAnimator.Camina();
        _state = 1;
        while (Mathf.Abs(Distancia()) > _configuration.CloseRange)
        {
            Vector3 newPos = new Vector3((SetDirection() * _configuration.WalkSpeed * Time.deltaTime), 0, 0);
            _myTransform.position += newPos;
            yield return null;
        }
        _state = 0;
        StartCoroutine(Cuchillada());
    }

    private IEnumerator Rugido()
    {
        yield return new WaitForSeconds(0.2f);
        _sfxFauno.Rugido1SFX();
        yield return new WaitForSeconds(3);
    }

    private IEnumerator Embestida()
    {
        //determina si el jugador está a su derecha o izq, coge la posición de la pared específica y embiste hacia ese lado
        //es un ataque de larga distancia 
        
        _faunoAnimator.Correr();
        int dir = SetDirection();
        //yield return new WaitForSeconds(6);

        _obstacleComponent.pDamage = 20;
        _obstacleComponent.multiplier = 1.5f;
        while (!_hitWall)
        {
            Vector3 newPos = Vector3.zero;
            newPos = new Vector3((dir * _configuration.RunSpeed * Time.deltaTime), 0, 0);
            _myTransform.position += newPos;
            yield return null;
        }
        //_camera.camState = 2;
        _faunoAnimator.CorrerEnd();
        _sfxFauno.ParedSFX();
        _obstacleComponent.pDamage = 5;
        _obstacleComponent.multiplier = 1;
        _hitWall = false;
        yield return new WaitForSeconds(0.5f);
        _myTransform.position += SeparaPared(dir);

        StartCoroutine(FaunoAI());
    }

    private IEnumerator SaltoVert()
    {
        //salta hacia arriba y se mantiene fuera de pantalla con una altura constante durante x segundos
        //va cambiando su posición en x siguiendo al jugador hasta que cae sobre la ultima pos guardada
        //onda expansiva al caer opcional
        //sombra movetowards
        _faunoAnimator.Saltar();

        _obstacleComponent.pDamage = 15;

        yield return new WaitForSeconds(0.5f);

        _hitGround = false;

        _faunoRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _faunoRB.AddForce(transform.up*_configuration.JumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);
        _faunoRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        /*float finalTime = Time.time + _configuration.AirTime;
        print(finalTime);
        while (Time.time < finalTime)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, new Vector3(_player.transform.position.x, _myTransform.position.y, 0), _configuration.WalkSpeed);
            yield return null;
            print(Time.time);
        }
        _faunoRB.constraints = RigidbodyConstraints2D.FreezeRotation;*/

        yield return new WaitForSeconds(5);
        float Xpos = _player.transform.position.x;
        Vector3 NewPos = new Vector3(Xpos, _myTransform.position.y, 0f);
        _myTransform.position = NewPos;
        _faunoRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _faunoRB.AddForce(transform.up * _configuration.DropForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        _myTransform.position = new Vector3 (_myTransform.position.x, -1.94f, 0f);

        yield return new WaitForSeconds(2);
        _obstacleComponent.pDamage = 5;

        StartCoroutine(FaunoAI());
    }

    private IEnumerator Cuchillada()
    {
        //expandir el hitbox del fauno para simular el tajo
        int dir = SetDirection();

        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2(1.5f, 1.2f);

        Vector2 offBase = _boxColl.offset;
        Vector2 offDest = new Vector2(dir * 1f, 0.5f);

        _obstacleComponent.pDamage = 15;

        _faunoAnimator.Cuchillada();

        yield return new WaitForSeconds(1.2f);

        _boxColl.size = scDest;
        _boxColl.offset = offDest;
        
        yield return new WaitForSeconds(0.4f);
        _sfxFauno.CuchilladaSFX();

        _boxColl.size = scBase;
        _boxColl.offset = offBase;

        _obstacleComponent.pDamage = 5;

        StartCoroutine(FaunoAI());
    }

    private IEnumerator CuchillaFloor(Vector3 vectorPosCuchilla)
    {
        //hacer que surjan a lo largo del mapa varias hitboxes verticales con un poco de retraso
        int dir = SetDirection();


        _sfxFauno.Rugido2SFX();
        _faunoAnimator.CuchilladaSuelo();

        yield return new WaitForSeconds(1.2f);

        _cuchillasSuelo = true;

        if (!_spriteF.flipX)
        {   
            for (int i = _cuchillaManagers.Length - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(0.15f);
                
                if (_cuchillaManagers[i].transform.position.x < _myTransform.position.x)
                {
                    _sfxFauno.CuchillaSFX();
                    StartCoroutine(_cuchillaManagers[i].SacaCuchilla());
                }
            }
        }
        else
        {
            for (int i = 0; i < _cuchillaManagers.Length; i++)
            {
                yield return new WaitForSeconds(0.15f);

                if (_cuchillaManagers[i].transform.position.x > _myTransform.position.x)
                {
                    _sfxFauno.CuchillaSFX();
                    StartCoroutine(_cuchillaManagers[i].SacaCuchilla());
                }
            }
        }

        _cuchillasSuelo = false;
        StartCoroutine(FaunoAI());
    }

    private IEnumerator Mina()
    {
        //proyectil

        int dir = SetDirection();

        _faunoAnimator.Aliento();
        _sfxFauno.MinaSFX();
        yield return new WaitForSeconds(0.5f);

        GameObject minaPrefab = Instantiate(_mina, _escupeMina.transform.position, Quaternion.identity);

        minaPrefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(_vectsMina[_numVectMinas].x * dir, _vectsMina[_numVectMinas].y), ForceMode2D.Impulse);
        Debug.Log(_numVectMinas.ToString());
        _numVectMinas++;

        if (_numVectMinas > 3)
        {
            _numVectMinas = 0;
        }

        StartCoroutine(FaunoAI());

    }


    #endregion

    #region AI
    private IEnumerator FaunoAI()
    {
        int rnd = Random.Range(0, 3);
        yield return new WaitForSeconds(2f);

        if (Mathf.Abs(Distancia()) <= _configuration.CloseRange)
        {
            StartCoroutine(Cuchillada());
        }
        else if (Mathf.Abs(Distancia()) <= _configuration.LongRange)
        {
            if (_bossHealth.health > (_bossHealth.maxHealth / 2))
            {
                if(rnd == 2)
                {
                    StartCoroutine(Mina());
                }
                else if (rnd == 0)
                {
                    vectorPosCuchilla = new Vector3(_myTransform.position.x + (_configuration.DistCuchilla * SetDirection()), -7, 0);
                    StartCoroutine(CuchillaFloor(vectorPosCuchilla));
                }
                else
                {
                    StartCoroutine(Walk());
                }
            }
            else
            {
                if (rnd == 0)
                    StartCoroutine(Mina());
                else
                {
                    vectorPosCuchilla = new Vector3(_myTransform.position.x + (_configuration.DistCuchilla * SetDirection()), -7, 0);
                    StartCoroutine(CuchillaFloor(vectorPosCuchilla));
                }
            }
        }
        else
        {
            if (_bossHealth.health > (_bossHealth.maxHealth / 2))
            {
                StartCoroutine(SaltoVert());
            }
            else
            {
                if (rnd == 0)
                    StartCoroutine(SaltoVert());
                else
                    StartCoroutine(Embestida());
            }
        }
    }
    private IEnumerator StartC()
    {
        StartCoroutine(Rugido());
        yield return new WaitForSeconds(4);
        StartCoroutine(FaunoAI());
    }
    #endregion

    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _faunoRB = GetComponent<Rigidbody2D>();
        _bossHealth = GetComponent<BossHealth>();
        _boxColl = GetComponent<BoxCollider2D>();
        _spriteF = GetComponent<SpriteRenderer>();
        _faunoAnimator = GetComponent<FaunoAnimator>();
        _sfxFauno = GetComponent<SFXFaunoManager>();
        _sceneManagerS = GetComponent<SceneManagerS>();
      
        _obstacleComponent = GetComponent<ObstacleComponent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        
        _obstacleComponent.pDamage = 5;
        StartCoroutine(StartC());
    }

    // Update is called once per frame
    void Update()
    {
        Distancia();
        Orient();
    }
}
