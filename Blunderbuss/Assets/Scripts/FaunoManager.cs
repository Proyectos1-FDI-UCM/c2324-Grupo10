using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FaunoManager : MonoBehaviour
{
    #region references
    [SerializeField] private FaunoConfig _configuration; //configuración externa de las variables que va a usar el fauno

    [SerializeField] private GameObject _conjCuch;

    [SerializeField] private GameObject _escupeMina;

    [SerializeField] private CuchillaManager[] _cuchillaManagers = new CuchillaManager[14];

    [SerializeField] private MinaComponent _minaComponent = new MinaComponent();

    [SerializeField] private GameObject[] _minas = new GameObject[4];

    [SerializeField] private Rigidbody2D[] _minasRB = new Rigidbody2D[4];

    private ObstacleComponent _obstacleComponent;
    private SFXFaunoManager _sfxFauno;
    private BossHealth _bossHealth;
    private Rigidbody2D _faunoRB;
    private Transform _myTransform;
    private SpriteRenderer _spriteF;
    private BoxCollider2D _boxColl;
    private GameObject _player;

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

    private int _minaActivas = 0;

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
        yield return new WaitForSeconds(0.4f);
        _sfxFauno.Rugido1SFX();
        yield return new WaitForSeconds(3);
    }

    private IEnumerator Embestida()
    {
        //determina si el jugador está a su derecha o izq, coge la posición de la pared específica y embiste hacia ese lado
        //es un ataque de larga distancia 
        
        StartCoroutine(Rugido());
        _faunoAnimator.Correr();
        int dir = SetDirection();
        yield return new WaitForSeconds(6);

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

        yield return new WaitForSeconds(0.5f);

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

        yield return new WaitForSeconds(3);
        StartCoroutine(FaunoAI());
    }

    private IEnumerator Cuchillada()
    {
        //expandir el hitbox del fauno para simular el tajo
        int dir = SetDirection();

        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2(8f, 3.5f);

        Vector2 offBase = _boxColl.offset;
        Vector2 offDest = _boxColl.offset * new Vector2(dir * 2f, -0.5f);

        float scSpeed = 10f;
        float offSpeed = 5f;

        _obstacleComponent.pDamage = 15;

        _faunoAnimator.Cuchillada();

        while (_boxColl.offset != offDest)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scDest, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offDest, offSpeed * Time.deltaTime);

            Debug.Log("Polla");

            yield return null;
        }
        
        yield return new WaitForSeconds(0.4f);
        _sfxFauno.CuchilladaSFX();

        while (_boxColl.offset != offBase)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scBase, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offBase, offSpeed * Time.deltaTime);

            Debug.Log("Coño");

            yield return null;
        }

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

        if (!_spriteF.flipX)
        {   
            for (int i = _cuchillaManagers.Length - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(0.15f);
                _sfxFauno.CuchillaSFX();

                if (_cuchillaManagers[i].transform.position.x < _myTransform.position.x)
                    StartCoroutine(_cuchillaManagers[i].SacaCuchilla());
            }
        }
        else
        {
            for (int i = 0; i < _cuchillaManagers.Length; i++)
            {
                yield return new WaitForSeconds(0.15f);
                _sfxFauno.CuchillaSFX();

                if (_cuchillaManagers[i].transform.position.x > _myTransform.position.x)
                    StartCoroutine(_cuchillaManagers[i].SacaCuchilla());
            }
        }

            StartCoroutine(FaunoAI());
    }

    private IEnumerator Mina()
    {
        //Doble collider (trigger suelo/fisico player)
        //Se activa fisico al tocar el suelo
        //llevar la mina a la pos de escupeMina
        //addforce con un vector y luego que lo maneja 
        //se queda donde aterrize x segundos y se desactiva(setactive = false)
        int dir = SetDirection();

        int i = 0;
        while(i<_minas.Length-1 && _minas[i].activeSelf == true)
        {
            i++;
        }

        _faunoAnimator.Aliento();
        _sfxFauno.MinaSFX();
        yield return new WaitForSeconds(0.5f);

        _minaActivas++;
        _minas[i].SetActive(true);
        CircleCollider2D _coll = _minas[i].GetComponent<CircleCollider2D>();
        MinaComponent _mc = _minas[i].GetComponent<MinaComponent>();
        _coll.enabled = true;
        _minas[i].transform.position = _escupeMina.transform.position;
        _minasRB[i].AddForce(new Vector2(_configuration.VectMina.x*dir, _configuration.VectMina.y), ForceMode2D.Impulse);
        _mc.Activa();

        StartCoroutine(FaunoAI());

        yield return new WaitForSeconds(_configuration.MinaTime);

        if (_minas[i].active == true)
        {
            _minas[i].SetActive(false);
            _minaActivas--;
        }
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
                if (rnd == 0 || _minaActivas == 3)
                {
                    vectorPosCuchilla = new Vector3(_myTransform.position.x + (_configuration.DistCuchilla * SetDirection()), -7, 0);
                    StartCoroutine(CuchillaFloor(vectorPosCuchilla));
                }     
                else
                    StartCoroutine(Mina());
            }
            else
            {
                if (rnd == 0 && _minaActivas < 3)
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
                if (rnd == 0)
                    StartCoroutine(Walk());
                    //StartCoroutine(Embestida());
                else
                    //StartCoroutine(Embestida());
                    StartCoroutine(SaltoVert());
                    //StartCoroutine(Walk());

            }
            else
            {
                if (rnd == 0)
                    //StartCoroutine(Embestida());
                    StartCoroutine(SaltoVert());
                else
                    StartCoroutine(Embestida());
            }
        }
    }
    private IEnumerator StartC()
    {
        StartCoroutine(Rugido());
        yield return new WaitForSeconds(6);
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

        for(int i = 0; i<_minasRB.Length; i++)
        {
            _minasRB[i] = _minas[i].GetComponent<Rigidbody2D>(); 
        }
      
        _obstacleComponent = GetComponent<ObstacleComponent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        for (int i = 0; i < _minas.Length; i++)
        {
            _minas[i].SetActive(false);
        }
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
