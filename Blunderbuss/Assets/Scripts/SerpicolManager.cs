using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpicolManager : MonoBehaviour
{
    #region references
    private ObstacleComponent _obstacleComponent;
    private BossHealth _bossHealth;
    private Rigidbody2D _serpiRB;
    private Transform _myTransform;
    private SpriteRenderer _spriteS;
    private BoxCollider2D _boxColl;
    private CircleCollider2D _cirColl;
    [SerializeField]
    float _paredIzq;
    [SerializeField]
    float _paredDer;

    public GameObject[] HipnoSpawn = new GameObject[8];
    public GameObject[] HipnoArea = new GameObject[8];
    public SpriteRenderer[] HipnoAS = new SpriteRenderer[8];
    public BoxCollider2D[] HipnoAB = new BoxCollider2D[8];

    public GameObject[] Gapo = new GameObject[3];
    private GapoManager[] _gapoManager = new GapoManager[3];

    private CameraController _camera;
    private SerpicolAnimator _serpicolAnimator;
    private SFXSerpicolManager _serpicolSFX;

    private GameObject[] _babas = new GameObject[25];
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
    private bool _alive = true;
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

                if (_serpicolAnimator.serpicolAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !_spriteS.flipX)
                {
                    Mirror();
                }
            }

            else if (dist.x > 2)
            {
                _direction = 1;

                if (_serpicolAnimator.serpicolAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && _spriteS.flipX)
                {
                    Mirror();
                }
            }
        }
    }

    private void Mirror()
    {
        _spriteS.flipX = !_spriteS.flipX;
        _boxColl.offset = new Vector2(-_boxColl.offset.x, _boxColl.offset.y);

        if (!_spriteS.flipX)
            _myTransform.position += Vector3.right * 4f;
        else
            _myTransform.position += Vector3.left * 4f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.layer != _currLay)
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
        }*/

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

    /*private IEnumerator CollDes(Collider2D collision)
    {
        _currLay = collision.gameObject.layer;
        yield return new WaitForSeconds(0.4f);
        if (collision.gameObject.layer == _currLay)
            _currLay = -1;
    }*/

    public void Muerte()
    {
        _alive = false;
        while (_serpicolAnimator.serpicolAnimator.GetCurrentAnimatorStateInfo(0).IsName("rueda"))
        {
            return;
        }

        StopAllCoroutines();

        StartCoroutine(MuerteAnim());
    }

    private IEnumerator MuerteAnim()
    {
        for (int i = 0; i < 8; i++)
        {
            HipnoSpawn[i].SetActive(false);
            HipnoAB[i].enabled = false;
            HipnoAS[i].enabled = false;
        }

        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < 8; i++)
        {
            HipnoArea[i].SetActive(false);
            HipnoAB[i].enabled = true;
            HipnoAS[i].enabled = true;
        }

        _boxColl.enabled = false;

        _serpicolAnimator.IdleAnimation();
        _serpicolAnimator.serpicolAnimator.speed = 3;

        yield return new WaitForSeconds(1);
        Mirror();
        yield return new WaitForSeconds(1);
        Mirror();
        yield return new WaitForSeconds(1);

        _serpicolAnimator.serpicolAnimator.speed = 0;
        yield return new WaitForSeconds(1);
        _serpicolAnimator.serpicolAnimator.speed = 0.25f;
        _serpicolAnimator.CaracolaAnimation();

        yield return new WaitForSeconds(4);

        Vector3 carPos;

        if (!_spriteS.flipX)
            carPos = new Vector3(-3f, -1, 0);
        else
            carPos = new Vector3(3f, -1, 0);

        _myTransform.position += carPos;

        yield return new WaitForSeconds(1.5f);

        Vector3 tumba = new Vector3(_myTransform.position.x, _myTransform.position.y - 5, 0);
        float tumbaSpeed = 1.2f;

        while (_myTransform.position.y != tumba.y)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, tumba, tumbaSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        SendMessage("CargarEscena");
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
        _bossHealth.invulnerable = true;

        float esconderS = 1f;
        Vector3 carPos = new Vector3(directionAux * -3f, -1, 0);

        float rotSpeed = 1600f;
        float transSpeed = 22f;

        _obstacleComponent.pDamage = 20;
        float rotDest = 120;
        float transDist = 25f;
        
        int vueltas = 0;

        yield return new WaitForSeconds(esconderS);
        _myTransform.position += carPos;
        _boxColl.enabled = false;
        _cirColl.enabled = true;
        /*_boxColl.size = Vector2.one * 4;
        _boxColl.offset = Vector2.zero;*/

        Vector3 transVec = _myTransform.position + new Vector3(directionAux * transDist, 0, 0);

        yield return new WaitForSeconds(0.3f);
        _obstacleComponent.multiplier = 1.5f;
        _serpicolSFX.CaracolaSFX();
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
        _cirColl.enabled = false;
        _boxColl.enabled = true;
        _boxColl.size = _boxCollAuxS;
        _boxColl.offset = _secureOff;
        //Mirror(); // invertir carPos.x y _boxCollAuxO.x si se quiere mirror pero queda peor.
        _bossHealth.invulnerable = false;
        _serpicolAnimator.IdleAnimation();
        _myTransform.position += -carPos;
        yield return new WaitForSeconds(0.7f);
        _boxColl.offset = _boxCollAuxO;

        StartCoroutine(SerpicolAI());
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

        _serpicolSFX.BocaoSFX();
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

        StartCoroutine(SerpicolAI());
    }

    public IEnumerator Hipnosis()
    {
        int directionAux = _direction;

        float initPos = 5f;
        float initPos2 = 10f;
        float spawnDist = 6f;
        float umbral = 6f;
        int spawnQ = 4;
        float wait = 0.3f;
        float limitLeftH = -15;
        float limitRightH = 15;

        _serpicolSFX.PrerrayoSFX();
        _serpicolAnimator.HipnosisAnimation();

        float currentPos = _myTransform.position.x + (directionAux * initPos);
        float currentPos2 = _myTransform.position.x + (-directionAux * initPos2);

        yield return new WaitForSeconds(wait);

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH && currentPos < limitRightH)
            {
                HipnoSpawn[i].transform.position = new Vector3(currentPos, HipnoSpawn[i].transform.position.y, 0);
                HipnoSpawn[i].SetActive(true);
            }

            if (currentPos2 > limitLeftH && currentPos2 < limitRightH)
            {
                HipnoSpawn[HipnoSpawn.Length - i - 1].transform.position = new Vector3(currentPos2, HipnoSpawn[HipnoSpawn.Length - i - 1].transform.position.y, 0);
                HipnoSpawn[HipnoSpawn.Length - i - 1].SetActive(true);
            }

            currentPos += directionAux * spawnDist;
            currentPos2 += -directionAux * spawnDist;
            yield return new WaitForSeconds(wait);
        }

        currentPos = _myTransform.position.x + (directionAux * initPos);
        currentPos2 = _myTransform.position.x + (-directionAux * initPos2);
        yield return new WaitForSeconds(wait * 2.5f);
        _serpicolSFX.RayoSFX();

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH && currentPos < limitRightH)
            {
                HipnoArea[i].transform.position = new Vector3(currentPos, HipnoArea[i].transform.position.y, 0);
                HipnoArea[i].SetActive(true);
                HipnoSpawn[i].SetActive(false);
            }
            if (currentPos2 > limitLeftH && currentPos2 < limitRightH)
            {
                HipnoArea[HipnoArea.Length - i - 1].transform.position = new Vector3(currentPos2, HipnoArea[HipnoArea.Length - i - 1].transform.position.y, 0);
                HipnoArea[HipnoArea.Length - i - 1].SetActive(true);
                HipnoSpawn[HipnoSpawn.Length - i - 1].SetActive(false);
            }
            currentPos += directionAux * spawnDist;
            currentPos2 += -directionAux * spawnDist;
        }

        yield return new WaitForSeconds(0.7f);
        _serpicolAnimator.IdleAnimation();

        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < spawnQ * 2; i++)
        {
            HipnoAB[i].enabled = false;
            HipnoAS[i].enabled = false;
        }

        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < spawnQ * 2; i++)
        {
            HipnoArea[i].SetActive(false);
            HipnoAB[i].enabled = true;
            HipnoAS[i].enabled = true;
        }

        StartCoroutine(SerpicolAI());
    }

    public IEnumerator Disparo()
    {
        int directionAux = _direction;

        _serpicolAnimator.GaposAnimation();
        bool turn;

        float force = 90;
        Vector3 relPos = new Vector3(directionAux * 0.1f, 0.6f, 0);
        Vector2 dir;
        if(_player.transform.position.y > 1)
        {
            dir = new Vector2(directionAux * 5, 10);
        }
        else
        {
            dir = new Vector2(directionAux * 5, 1);
        }
        

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

        StartCoroutine(SerpicolAI());
    }

    public IEnumerator ChoqueP()
    {
        float esconderS = 1f;
        _obstacleComponent.multiplier = 1;
        _obstacleComponent.pDamage = 5;

        _serpiRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _serpicolAnimator.Suelo(false);

        Vector3 impulse;

        _serpicolSFX.GolpeParedSFX();

        if (!_spriteS.flipX)
            impulse = new Vector3(-2100, 5300, 0);
        else
            impulse = new Vector3(2100, 5300, 0);

        _myTransform.rotation = Quaternion.identity;
        _serpiRB.velocity = Vector2.zero;
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
        _cirColl.enabled = false;
        _boxColl.enabled = true;
        _boxColl.size = _boxCollAuxS;
        _boxColl.offset = _secureOff;
        _bossHealth.invulnerable = false;
        _serpicolAnimator.IdleAnimation();
        _myTransform.position += -carPos;
        yield return new WaitForSeconds(0.7f);
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

        StartCoroutine(SerpicolAI());
    }

    #endregion

    #region AI

    public IEnumerator SerpicolAI()
    {
        float range1 = 3.5f;
        float range2 = 8;
        float distX;
        int rnd = Random.Range(0, 3);

        yield return new WaitForSeconds(1);

        distX = Mathf.Abs(_myTransform.position.x - _player.transform.position.x);

        if (distX <= range1)
        {
            StartCoroutine(Mordisco());
        }
        else if (distX <= range2)
        {
            if (_bossHealth.health > (_bossHealth.maxHealth/2))
            {
                if (rnd == 0)
                    StartCoroutine(Caracola());
                else
                    StartCoroutine(Disparo());
            }
            else
            {
                if (rnd == 0)
                    StartCoroutine(Caracola());
                else
                    StartCoroutine(Hipnosis());
            }
        }
        else
        {
            if (_bossHealth.health > (_bossHealth.maxHealth/2))
            {
                StartCoroutine(Caracola());
            }
            else
            {
                if (rnd == 0)
                    StartCoroutine(Hipnosis());
                else
                    StartCoroutine(Caracola());
            }
        }
    }

    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.GetComponent<CameraController>();
        _obstacleComponent = GetComponent<ObstacleComponent>();
        _bossHealth = GetComponent<BossHealth>();
        _serpiRB = GetComponent<Rigidbody2D>();
        _myTransform = transform;
        _serpicolAnimator = GetComponent<SerpicolAnimator>();
        _serpicolSFX = GetComponent<SFXSerpicolManager>();
        _spriteS = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();
        _cirColl = GetComponent<CircleCollider2D>();
        _player = GameManager.Instance.Player;
        
        for (int i = 0; i < HipnoArea.Length; i++)
        {
            HipnoAS[i] = HipnoArea[i].GetComponent<SpriteRenderer>();
            HipnoAB[i] = HipnoArea[i].GetComponent<BoxCollider2D>();
        }

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

        StartCoroutine(SerpicolAI());
    }

    // Update is called once per frame
    void Update()
    {
        Orient();
    }
}
