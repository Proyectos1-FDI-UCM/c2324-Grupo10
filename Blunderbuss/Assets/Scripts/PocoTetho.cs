using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourCript : MonoBehaviour
{
    #region parameters
    [SerializeField] 
    private GameObject _Canvas;
    private float tiempoActivo = 5f;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _Canvas.SetActive(true);
        StartCoroutine(mantente());
       
    }
    public IEnumerator mantente()
    {
        yield return new WaitForSeconds(tiempoActivo);
        _Canvas.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    private void Start()
    {
       _Canvas.SetActive(false );
    }
    void Update()
    {
        
    }
}
