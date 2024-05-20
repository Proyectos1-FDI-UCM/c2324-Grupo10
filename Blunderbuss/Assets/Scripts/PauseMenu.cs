using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region references
    static private GameObject _playerMaster;
    private string Inicio;
    private InputManager _inputManager;
    private GameObject firstButton;
    #endregion

    #region parameters
    [SerializeField]
    GameObject CanvasPausa;
    /*[SerializeField]
    GameObject CanvasOpcionesPausa;*/
    #endregion

    #region methods
    public void pause()
    {
        Time.timeScale = 0f;
        _inputManager.enabled = false;
        CanvasPausa.SetActive(true);
        firstButton = GameObject.FindGameObjectWithTag("First");
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void Continue ()
    {
        Time.timeScale = 1f;
        _inputManager.enabled = true;
        CanvasPausa.SetActive(false);
    }

    /*public void Opciones()
    {
        CanvasPausa.SetActive(false);
        CanvasOpcionesPausa.SetActive(true);
    }
    public void Volver()
    {
        CanvasOpcionesPausa.SetActive(false) ;
        CanvasPausa.SetActive(true);
    }*/
    public void Salir()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    #endregion 
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = GameManager.Instance.InputManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
