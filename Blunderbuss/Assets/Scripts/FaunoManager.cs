using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FaunoManager : MonoBehaviour
{
    #region references
    [SerializeField] private FaunoConfig _configuration; //configuración externa de las variables que va a usar el fauno

    [SerializeField] private GameObject _conjCuch;

    [SerializeField] private CuchillaManager[] _cuchillaManagers = new CuchillaManager[4];

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
                            //=0 ; blablabla
                            //=1 ; blablabla
                            //=2 ; blablabla
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    private Transform WalkTowards(float deltaTime) // provisionalmente que camine hacia el jugador.
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = (SetDirection() * _configuration.WalkSpeed * deltaTime);

        if (_alive)
        {
            _myTransform.position += newPos;
        }
        return _myTransform;
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
    private IEnumerator Embestida()
    {
        //determina si el jugador está a su derecha o izq, coge la posición de la pared específica y embiste hacia ese lado
        //es un ataque de larga distancia 

       while(!_hitWall)
        {
            Vector3 newPos = Vector3.zero;
            newPos = new Vector3((SetDirection() * _configuration.RunSpeed * Time.deltaTime), 0, 0);
            _myTransform.position += newPos;
            yield return null;
        }

        yield return new WaitUntil(() => _hitWall == true);
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
    }

    private IEnumerator Cuchillada()
    {
        //a ver no se la verdad
        //expandir el hitbox del fauno para simular el tajo
        int dir = SetDirection();

        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2(2f, 0.7f);

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

    }

    private void Aliento()
    {
        //hacer que aparezca una hitbox horizontal desde el fauno a la pared a la que este mirando
    }


    #endregion

    #region AI

    #endregion
    /*private IEnumerator FaunoAI()
    {

    }*/
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _faunoRB = GetComponent<Rigidbody2D>();
        _bossHealth = GetComponent<BossHealth>();
        _boxColl = GetComponent<BoxCollider2D>();
        _spriteF = GetComponent<SpriteRenderer>();
        _faunoAnimator = GetComponent<FaunoAnimator>();
      
        _obstacleComponent = GetComponent<ObstacleComponent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        StartCoroutine(CuchillaFloor());
    }

    // Update is called once per frame
    void Update()
    {
        //WalkTowards(Time.deltaTime);
        
    }
}
