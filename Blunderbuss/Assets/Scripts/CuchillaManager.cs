using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuchillaManager : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    [SerializeField] private FaunoConfig _config;
    #endregion

    #region parameters
    #endregion

    #region methods
    /// <summary>
    /// saca cuchilla, espera x segundos, guarda cuchilla
    /// </summary>
    /// <returns></returns>
    public IEnumerator SacaCuchilla() 
    {
        Vector3 iniPos = _myTransform.position;
        Vector3 finalPos = _myTransform.position + new Vector3(0, 5, 0);
        
        while(_myTransform.position != finalPos)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, finalPos, _config.SubeSpeed*Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while(_myTransform.position != iniPos)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, iniPos, _config.BajaSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion

    private void Awake()
    {
        _myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
