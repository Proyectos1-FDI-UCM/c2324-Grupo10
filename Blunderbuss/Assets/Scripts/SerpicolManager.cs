using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpicolManager : MonoBehaviour
{
    #region references
    private ObstacleComponent _obstacleComponent;
    private Rigidbody2D _serpiRB;
    private Transform _myTransform;
    private SpriteRenderer _spriteS;
    private BoxCollider2D _boxColl;
    [SerializeField]
    float _paredIzq;
    [SerializeField]
    float _paredDer;

    public GameObject[] HipnoSpawn = new GameObject[3];
    public GameObject[] HipnoArea = new GameObject[3];

    public GameObject[] Gapo = new GameObject[3];
    private GapoManager[] _gapoManager = new GapoManager[3];

    private CameraController _camera;
    private SerpicolAnimator _serpicolAnimator;

    private GameObject[] _babas = new GameObject[30];
    [SerializeField]
    GameObject _baba;

    private GameObject _player;
    private int _direction = -1;
    private int _currLay = -1;

    private Vector2 _boxCollAuxS;
    private Vector2 _boxCollAuxO;
    private Vector2 _secureOff;
    #endregion

    #region parameters
    #endregion

    #region methods

    private void Orient()
    {
        Vector3 dist = _player.transform.position - _myTransform.position;

        if (dist.x < -2)
        {
            _direction = -1;

            if (_serpicolAnimator.serpicolAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !_spriteS.flipX)
            {
                Mirror();
                _myTransform.position += Vector3.left * 3f;
            }
        }

        else if (dist.x > 2)
        {
            _direction = 1;

            if (_serpicolAnimator.serpicolAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && _spriteS.flipX)
            {
                Mirror();
                _myTransform.position += Vector3.right * 3f;
            }
        }
    }

    private void Mirror()
    {
        _spriteS.flipX = !_spriteS.flipX;
        _boxColl.offset = new Vector2(-_boxColl.offset.x, _boxColl.offset.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _currLay)
        {
            StartCoroutine(CollDes(collision));

            if (_serpicolAnimator.serpicolAnimator.GetInteger("AnimState") == 1 && collision.CompareTag("Pared"))
            {
                StopAllCoroutines();
                StartCoroutine("ChoqueP");
            }

            if (collision.CompareTag("Suelo"))
            {
                _serpiRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                _serpicolAnimator.Suelo(true);
            }
        }
    }

    private IEnumerator CollDes(Collider2D collision)
    {
        _currLay = collision.gameObject.layer;
        yield return new WaitForSeconds(0.4f);
        if (collision.gameObject.layer == _currLay)
            _currLay = -1;
    }

    #region attacks

    public IEnumerator Caracola()
    {
        int directionAux = _direction;

        _boxCollAuxS = _boxColl.size;
        _boxCollAuxO = _boxColl.offset;

        _secureOff = _boxCollAuxO + new Vector2(directionAux * -1, 0);

        _serpicolAnimator.CaracolaAnimation();
        _boxColl.offset = _secureOff;

        float esconderS = 1f;
        Vector3 carPos = new Vector3(directionAux * -3f, -1, 0);

        float rotSpeed = 1600f;
        float transSpeed = 18f;

        _obstacleComponent.pDamage = 20;
        float rotDest = 120;
        float transDist = 22f;
        
        int vueltas = 0;

        yield return new WaitForSeconds(esconderS);
        _myTransform.position += carPos;
        _boxColl.size = Vector2.one * 4;
        _boxColl.offset = Vector2.zero;

        Vector3 transVec = _myTransform.position + new Vector3(directionAux * transDist, 0, 0);

        yield return new WaitForSeconds(0.3f);
        _obstacleComponent.multiplier = 3;
        while (vueltas != 5)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, transVec, transSpeed * Time.deltaTime);
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, Quaternion.Euler(0, 0, -directionAux * rotDest), rotSpeed * Time.deltaTime);
            yield return null;

            if ((System.Convert.ToInt32(_myTransform.rotation.eulerAngles.z) == rotDest && directionAux < 0) || ((360 - System.Convert.ToInt32(_myTransform.rotation.eulerAngles.z)) == rotDest) && directionAux > 0)
            {
                if (rotDest == 358)
                {
                    vueltas++;
                    rotDest = 120;
                }
                else
                {
                    rotDest += 119;
                }
            }
        }

        _obstacleComponent.multiplier = 1;
        _obstacleComponent.pDamage = 5;

        yield return new WaitForSeconds(0.3f);
        _myTransform.rotation = Quaternion.identity;
        _boxColl.size = _boxCollAuxS;
        _boxColl.offset = _secureOff;
        Mirror();
        _serpicolAnimator.IdleAnimation();
        _myTransform.position += new Vector3(carPos.x, -carPos.y, 0);
        yield return new WaitForSeconds(esconderS);
        _boxColl.offset = new Vector2(-_boxCollAuxO.x, _boxCollAuxO.y);
    }

    public IEnumerator Mordisco()
    {
        int directionAux = _direction;

        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2(2f, 0.7f);

        Vector2 offBase = _boxColl.offset;
        Vector2 offDest = _boxColl.offset + new Vector2(directionAux * 2f, -0.5f);

        float scSpeed = 10f;
        float offSpeed = 5f;

        _serpicolAnimator.BocaoAnimation();

        _obstacleComponent.pDamage = 15;

        while (_boxColl.offset != offDest)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scDest, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offDest, offSpeed * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        _obstacleComponent.pDamage = 5;

        while (_boxColl.offset != offBase)
        {
            _boxColl.size = Vector3.MoveTowards(_boxColl.size, scBase, scSpeed * Time.deltaTime);
            _boxColl.offset = Vector3.MoveTowards(_boxColl.offset, offBase, offSpeed * Time.deltaTime);

            yield return null;
        }

        _serpicolAnimator.IdleAnimation();
    }

    public IEnumerator Hipnosis()
    {
        int directionAux = _direction;

        float initPos = 6.5f;
        float spawnDist = 6f;
        float umbral = 5f;
        int spawnQ = 3;
        float wait = 0.3f;
        float limitLeftH = _paredIzq + umbral;
        float limitRightH = _paredDer - umbral;

        _serpicolAnimator.HipnosisAnimation();

        float currentPos = _myTransform.position.x + (directionAux * initPos);
        yield return new WaitForSeconds(wait);

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH || currentPos < limitRightH)
            {
                HipnoSpawn[i].transform.position = new Vector3(currentPos, HipnoSpawn[i].transform.position.y, 0);
                HipnoSpawn[i].SetActive(true);
            }
            currentPos += directionAux * spawnDist;
            yield return new WaitForSeconds(wait);
        }

        currentPos = _myTransform.position.x + (directionAux * initPos);
        yield return new WaitForSeconds(wait * 3);

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH || currentPos < limitRightH)
            {
                HipnoArea[i].transform.position = new Vector3(currentPos, HipnoArea[i].transform.position.y, 0);
                HipnoArea[i].SetActive(true);
            }
            currentPos += directionAux * spawnDist;
        }

        yield return new WaitForSeconds(0.7f);
        _serpicolAnimator.IdleAnimation();

        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < spawnQ; i++)
        {
            HipnoSpawn[i].SetActive(false);
            HipnoArea[i].SetActive(false);
        }
    }

    public IEnumerator Disparo()
    {
        int directionAux = _direction;

        _serpicolAnimator.GaposAnimation();
        bool turn;

        Vector3 relPos = new Vector3(directionAux * 0.1f, 0.6f, 0);
        Vector2 dir = new Vector2(directionAux * 5, 1);
        float force = 130;

        if (directionAux == 1)
            turn = false;
        else
            turn = true;
        //aqui
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < Gapo.Length; i++)
        {
            _gapoManager[i].spriteR.flipX = turn;
            Gapo[i].transform.position = _myTransform.position + relPos;
            Gapo[i].SetActive(true);
            _gapoManager[i].rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

            force += 30;
        }
        yield return new WaitForSeconds(1.8f);
        _serpicolAnimator.IdleAnimation();
    }

    public IEnumerator ChoqueP()
    {
        float esconderS = 1;
        _obstacleComponent.multiplier = 1;
        _obstacleComponent.pDamage = 5;

        _serpiRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _serpicolAnimator.Suelo(false);

        Vector3 impulse;

        if (!_spriteS.flipX)
            impulse = new Vector3(-2100, 5300, 0);
        else
            impulse = new Vector3(2100, 5300, 0);

        _myTransform.rotation = Quaternion.identity;
        _serpiRB.AddForce(impulse, ForceMode2D.Impulse);
        StartCoroutine(Lluvia());

        int directionAux;

        if (_myTransform.position.x > 0)
            directionAux = 1;
        else
            directionAux = -1;

        Vector3 carPos = new Vector3(directionAux * -3f, -1, 0);

        float rotSpeed = 850f;
        float rotDest = 120;

        int vueltas = 0;

        while (vueltas != 4)
        {
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, Quaternion.Euler(0, 0, directionAux * rotDest), rotSpeed * Time.deltaTime);
            yield return null;
            if ((System.Convert.ToInt32(_myTransform.rotation.eulerAngles.z) == rotDest && directionAux > 0) || (360 - (System.Convert.ToInt32(_myTransform.rotation.eulerAngles.z)) == rotDest) && directionAux < 0)
            {
                if (rotDest == 358)
                {
                    vueltas++;
                    rotDest = 120;
                }
                else
                {
                    rotDest += 119;
                }
            }
        }

        _myTransform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.3f);
        _myTransform.rotation = Quaternion.identity;
        _boxColl.size = _boxCollAuxS;
        _boxColl.offset = _secureOff;
        _serpicolAnimator.IdleAnimation();
        _myTransform.position += -carPos;
        yield return new WaitForSeconds(esconderS);
        _boxColl.offset = _boxCollAuxO;
    }

    public IEnumerator Lluvia()
    {
        float fallWait = 1;
        StartCoroutine(_camera.ShakeBegin(3));

        yield return new WaitForSeconds(0.1f);
        _currLay = -1;

        yield return new WaitForSeconds(fallWait);

        for (int i = 0; i < _babas.Length; i++)
        {
            if (!_babas[i].activeInHierarchy)
            {
                _babas[i].transform.position = new Vector3(Random.Range(_paredIzq + 2, _paredDer - 2), Random.Range(7f, 10f), 0);
                _babas[i].SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    #endregion

    #region AI
    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.GetComponent<CameraController>();
        _obstacleComponent = GetComponent<ObstacleComponent>();
        _serpiRB = GetComponent<Rigidbody2D>();
        _myTransform = transform;
        _serpicolAnimator = GetComponent<SerpicolAnimator>();
        _spriteS = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();
        _player = GameManager.Instance.Player;

        _obstacleComponent.pDamage = 5;

        for (int i = 0; i < Gapo.Length; i++)
        {
            _gapoManager[i] = Gapo[i].GetComponent<GapoManager>();
        }

        for (int i = 0; i < _babas.Length; i++)
        {
            GameObject obj = Instantiate(_baba);
            obj.SetActive(false);
            _babas[i] = obj;
        }

        StartCoroutine(Caracola());
    }

    // Update is called once per frame
    void Update()
    {
        Orient();
    }
}
