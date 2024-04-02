using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region references
    [SerializeField] GameObject _puerta1;
    [SerializeField] GameObject _puerta2;
    #endregion

    #region parameters
    public int Dianas;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PrimeraPuerta()
    {
        if (Dianas <= 0)
        {
            float _speedD = 1f;
            float _distD = 3f;
            Vector3 _targetPos = new Vector3(_puerta1.transform.position.x, _puerta1.transform.position.y + _distD, _puerta1.transform.position.z);
            yield return new WaitForSeconds(1);
            while(_puerta1.transform.position != _targetPos)
            {
                _puerta1.transform.position = Vector3.MoveTowards(_puerta1.transform.position, _targetPos, _speedD * Time.deltaTime);
                yield return null;
            }
        }
    }

    public IEnumerator SegundaPuerta()
    {
        float _speedD = 1f;
        float _distD = 3f;
        Vector3 _targetPos = new Vector3(_puerta2.transform.position.x, _puerta2.transform.position.y + _distD, _puerta2.transform.position.z);
        yield return new WaitForSeconds(1);
        while (_puerta2.transform.position != _targetPos)
        {
            _puerta2.transform.position = Vector3.MoveTowards(_puerta2.transform.position, _targetPos, _speedD * Time.deltaTime);
            yield return null;
        }
    }
}
