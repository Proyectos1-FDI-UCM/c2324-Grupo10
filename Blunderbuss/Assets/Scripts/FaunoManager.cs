using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FaunoManager : MonoBehaviour
{
    #region references
    [SerializeField] private FaunoConfig _configuration; //configuración externa de las variables que va a usar el fauno

    [SerializeField] private GameObject _conjCuch;

    [SerializeField] private GameObject _escupeMina;

    [SerializeField] private CuchillaManager[] _cuchillaManagers = new CuchillaManager[4];

    [SerializeField] private GameObject[] _minas = new GameObject[4];

    [SerializeField] private Rigidbody2D[] _minasRB = new Rigidbody2D[4];

    private ObstacleComponent _obstacleComponent;
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

    private int _state = 0; //variable de control de estados del fauno
                            //=0 ; en ataque
                            //=1 ; caminando
    #endregion

    #region methods
    /*private void Orient()
    {
        if (_alive)
        {
            Vector3 dist = _player.transform.position - _myTransform.position;

            if (dist.x < -2)
            {
                _direction = -1;

                if (_faunoAnimator.faunoAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !_spriteF.flipX)
                {
                    Mirror();
                }
            }

            else if (dist.x > 2)
            {
                _direction = 1;

                if (_faunoAnimator.faunoAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && _spriteF.flipX)
                {
                    Mirror();
                }
            }
        }
    }*/

    private void Mirror()
    {
        _spriteF.flipX = !_spriteF.flipX;
        _boxColl.offset = new Vector2(-_boxColl.offset.x, _boxColl.offset.y);

        if (!_spriteF.flipX)
            _myTransform.position += Vector3.right * 4f;
        else
            _myTransform.position += Vector3.left * 4f;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
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
        }
    }


    #region attacks
    private IEnumerator Walk()
    {
        _state = 1;
        while (Distancia() > _configuration.CloseRange)
        {
            Vector3 newPos = new Vector3((SetDirection() * _configuration.WalkSpeed * Time.deltaTime), 0, 0);
            _myTransform.position += newPos;
            yield return null;
        }
        _state = 0;
        StartCoroutine(FaunoAI());
    }

    private IEnumerator Rugido()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator Embestida()
    {
        //determina si el jugador está a su derecha o izq, coge la posición de la pared específica y embiste hacia ese lado
        //es un ataque de larga distancia 
        StartCoroutine(Rugido());
        yield return new WaitForSeconds(3);

        while(!_hitWall)
        {
            Vector3 newPos = Vector3.zero;
            newPos = new Vector3((SetDirection() * _configuration.RunSpeed * Time.deltaTime), 0, 0);
            _myTransform.position += newPos;
            yield return null;
        }

        yield return new WaitUntil(() => _hitWall == true);

        StartCoroutine(FaunoAI());
    }

    private IEnumerator SaltoVert()
    {
        //salta hacia arriba y se mantiene fuera de pantalla con una altura constante durante x segundos
        //va cambiando su posición en x siguiendo al jugador hasta que cae sobre la ultima pos guardada
        //onda expansiva al caer opcional
        //sombra movetowards
        _faunoRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _faunoRB.AddForce(transform.up*_configuration.JumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        _faunoRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
 
        float finalTime = Time.time + _configuration.AirTime;
        while (Time.time < finalTime)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, new Vector3(_player.transform.position.x, _myTransform.position.y, 0), _configuration.WalkSpeed);
            yield return null;
        }
        _faunoRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(FaunoAI());
    }

    private IEnumerator Cuchillada()
    {
        //a ver no se la verdad
        //expandir el hitbox del fauno para simular el tajo
        int dir = SetDirection();

        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2(8f, 3.5f);

        Vector2 offBase = _boxColl.offset;
        Vector2 offDest = _boxColl.offset * new Vector2(dir * 2f, -0.5f);

        float scSpeed = 10f;
        float offSpeed = 5f;

        while (_boxColl.offset != offDest)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scDest, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offDest, offSpeed * Time.deltaTime);

            yield return null;
        }
        
        yield return new WaitForSeconds(0.2f);


        while (_boxColl.offset != offBase)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scBase, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offBase, offSpeed * Time.deltaTime);

            yield return null;
        }
        
        StartCoroutine(FaunoAI());
    }

    private IEnumerator CuchillaFloor()
    {
        //hacer que surjan a lo largo del mapa varias hitboxes verticales con un poco de retraso
        int dir = SetDirection();

        _conjCuch.transform.position = new Vector3(_myTransform.position.x + (_configuration.DistCuchilla*dir), -7, 0);

        for(int i = 0; i < _cuchillaManagers.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(_cuchillaManagers[i].SacaCuchilla());
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
        while(i<_minas.Length-1 && _minas[i].activeSelf == false)
        {
            i++;
        }

        _minas[i].SetActive(true);
        _minas[i].transform.position = _escupeMina.transform.position;
        _minasRB[i].AddForce(new Vector2(_configuration.VectMina.x*dir, _configuration.VectMina.y), ForceMode2D.Impulse);

        yield return new WaitForSeconds(_configuration.MinaTime);

        _minas[i].SetActive(false);

        StartCoroutine(FaunoAI());
    }


    #endregion

    #region AI
    private IEnumerator FaunoAI()
    {
        int rnd = Random.Range(0, 3);
        yield return new WaitForSeconds(0.5f);

        if (Distancia() <= _configuration.CloseRange)
        {
            StartCoroutine(Cuchillada());
        }
        else if (Distancia() <= _configuration.LongRange)
        {
            if (_bossHealth.health > (_bossHealth.maxHealth / 2))
            {
                if (rnd == 0)
                    StartCoroutine(CuchillaFloor());
                else
                    StartCoroutine(Mina());
            }
            else
            {
                if (rnd == 0)
                    StartCoroutine(Mina());
                else
                    StartCoroutine(CuchillaFloor());
            }
        }
        else
        {
            if (_bossHealth.health > (_bossHealth.maxHealth / 2))
            {
                if (rnd == 0)
                    StartCoroutine(Walk());
                else
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

        StartCoroutine(Rugido());
        StartCoroutine(FaunoAI());
    }

    // Update is called once per frame
    void Update()
    {
        Distancia();
    }
}
