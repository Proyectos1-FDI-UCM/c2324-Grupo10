using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFinal : MonoBehaviour
{
    #region references
    public Transform _myTransform;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_myTransform.position.y < 1070)
        {
            _myTransform.position = new Vector3(_myTransform.position.x, _myTransform.position.y + 0.1f, _myTransform.position.z);
        }
    }
}
