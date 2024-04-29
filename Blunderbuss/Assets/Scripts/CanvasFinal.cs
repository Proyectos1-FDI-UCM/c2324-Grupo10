using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFinal : MonoBehaviour
{
    #region references
    public Transform _myTransform;
    #endregion

    #region parameters
    public float speed;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Salida());
    }

    // Update is called once per frame
    void Update()
    {
        if (_myTransform.position.y < 1070)
        {
            _myTransform.position = new Vector3(_myTransform.position.x, _myTransform.position.y + speed * Time.deltaTime, _myTransform.position.z);
        }
    }

    private IEnumerator Salida()
    {
        yield return new WaitForSeconds(20);
        SendMessage("CargarEscena");
    }
}
