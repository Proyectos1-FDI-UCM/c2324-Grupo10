using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourCript : MonoBehaviour
{
    #region parameters
    [SerializeField] 
    private GameObject _Canvas;
    [SerializeField]
    private GameObject _Button;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _Canvas.SetActive(true);
        _Button.SetActive(true);
    }

   public void Desactiva()
    {
        _Canvas.SetActive(false); 
        _Button.SetActive(false);
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
