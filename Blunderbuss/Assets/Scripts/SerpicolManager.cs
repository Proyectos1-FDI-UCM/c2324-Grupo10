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

        _obstacleComponent.pDamage = 5;

        StartCoroutine(Mordisco(-1));
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
}
