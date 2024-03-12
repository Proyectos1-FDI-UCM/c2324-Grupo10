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

        _obstacleComponent.pDamage = 5;

        StartCoroutine(Caracola(-1));
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

        _obstacleComponent.pDamage = 15;
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

            print(_myTransform.rotation.eulerAngles.z);
            print(rotDest);
            print(vueltas);
            print((_myTransform.rotation.eulerAngles.z + 1) % 120);

            if (_myTransform.rotation.eulerAngles.z % 120 < 3 || _myTransform.rotation.eulerAngles.z % 120 > 117)
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

        yield return new WaitForSeconds(0.3f);

        _myTransform.rotation = Quaternion.identity;
        _spriteS.flipX = !_spriteS.flipX;
        yield return new WaitForSeconds(esconderS);
    }
}
