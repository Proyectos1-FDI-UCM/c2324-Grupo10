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
    #endregion

    #region parameters
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _obstacleComponent = GetComponent<ObstacleComponent>();
        _serpiRB = GetComponent<Rigidbody2D>();
        _myTransform = transform;
        _spriteS = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();

        for (int i = 0; i < Gapo.Length; i++)
        {
            _gapoManager[i] = Gapo[i].GetComponent<GapoManager>();
        }

        _obstacleComponent.pDamage = 5;

        StartCoroutine(Hipnosis(-1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Caracola(int direction)
    {
        float esconderS = 0.4f;
        yield return new WaitForSeconds(esconderS);

        float rotSpeed = 2000f;
        float transSpeed = 15f;

        _obstacleComponent.pDamage = 20;
        float rotDest = 120;
        float transDist = 8f;
        Vector3 transVec = _myTransform.position + new Vector3(direction * transDist, 0, 0);
        int vueltas = 0;

        while(vueltas != 3)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, transVec, transSpeed * Time.deltaTime);
            if (vueltas != 3)
                _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, Quaternion.Euler(0, 0, -direction * rotDest), rotSpeed * Time.deltaTime);
            yield return null;

            print("rot " + _myTransform.rotation.eulerAngles.z);
            print("rotD " + rotDest);
            print("resto " + (360 - _myTransform.rotation.eulerAngles.z) % 120);
            //if (_myTransform.rotation.eulerAngles.z % 120 < 3 || _myTransform.rotation.eulerAngles.z % 120 > 117)
            if ((_myTransform.rotation.eulerAngles.z % 120 < 3 && direction >= 0) || ((360 - _myTransform.rotation.eulerAngles.z + 1) % 120 < 4) && direction <= 0)
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

        _obstacleComponent.pDamage = 5;
        yield return new WaitForSeconds(0.3f);

        _myTransform.rotation = Quaternion.identity;
        _spriteS.flipX = !_spriteS.flipX;
        yield return new WaitForSeconds(esconderS);
    }

    public IEnumerator Mordisco(int direction)
    {
        Vector2 scBase = _boxColl.size;
        Vector2 scDest = _boxColl.size * new Vector2 (2f, 0.7f);

        Vector2 offBase = _boxColl.offset;
        Vector2 offDest = _boxColl.offset + new Vector2(direction * 2f, -0.5f);

        float scSpeed = 60f;
        float offSpeed = 30f;

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
    }

    public IEnumerator Hipnosis(int direction)
    {
        float initPos = 6.5f;
        float spawnDist = 6f;
        float umbral = 5f;
        int spawnQ = 3;
        float wait = 0.3f;
        float limitLeftH = _paredIzq + umbral;
        float limitRightH = _paredDer - umbral;

        float currentPos = _myTransform.position.x + (direction * initPos);
        yield return new WaitForSeconds(wait);

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH || currentPos < limitRightH)
            {
                HipnoSpawn[i].transform.position = new Vector3(currentPos, HipnoSpawn[i].transform.position.y, 0);
                HipnoSpawn[i].SetActive(true);
            }
            currentPos += direction * spawnDist;
            yield return new WaitForSeconds(wait);
        }

        currentPos = _myTransform.position.x + (direction * initPos);
        yield return new WaitForSeconds(wait*3);

        for (int i = 0; i < spawnQ; i++)
        {
            if (currentPos > limitLeftH || currentPos < limitRightH)
            {
                HipnoArea[i].transform.position = new Vector3(currentPos, HipnoArea[i].transform.position.y, 0);
                HipnoArea[i].SetActive(true);
            }
            currentPos += direction * spawnDist;
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < spawnQ; i++)
        {
            HipnoSpawn[i].SetActive(false);
            HipnoArea[i].SetActive(false);
        }

    }

    public IEnumerator Disparo(int direction)
    {
        bool turn;

        Vector3 relPos = new Vector3(direction * 1.4f, 1f, 0);
        Vector2 dir = new Vector2(direction * 5, 1);
        float force = 130;

        if (direction == 1)
            turn = false;
        else
            turn = true;

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Gapo.Length; i++)
        {
            _gapoManager[i].spriteR.flipX = turn;
            Gapo[i].transform.position = _myTransform.position + relPos;
            Gapo[i].SetActive(true);
            _gapoManager[i].rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

            force += 30;
        }
        yield return null;
    }
}
