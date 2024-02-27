using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    static TutorialManager _instance;
    [SerializeField] GameObject _puerta1;
    static public TutorialManager Instance
    {
        get { return _instance; }
    }
    public int Dianas;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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
}
