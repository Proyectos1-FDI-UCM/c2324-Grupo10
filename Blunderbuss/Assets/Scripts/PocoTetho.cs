using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PocoTehto : MonoBehaviour
{
    #region references
    private VidaManager _vidaManager;
    #endregion

    #region parameters
    [SerializeField] 
    private GameObject _canvas;
    [SerializeField]
    private GameObject _trigger;

    private InputManager _inputManager;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canvas.SetActive(true);
        _trigger.SetActive(true);
        _inputManager.enabled = false;

        if (gameObject.CompareTag("Rambutan"))
        {
            _vidaManager.RambutanTutorial();
        }
    }

   public void Desactiva()
    {
        _canvas.SetActive(false);
        _trigger.SetActive(false);
        _inputManager.enabled = true;
    }
    #endregion

    private void Start()
    {
       _inputManager = GameManager.Instance.InputManager;
       _vidaManager = GameManager.Instance.Player.GetComponent<VidaManager>();
       _canvas.SetActive(false );
    }
    void Update()
    {
        
    }
}
