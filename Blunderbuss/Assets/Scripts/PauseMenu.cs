using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region references
    static private GameObject _playerMaster;
    #endregion

    #region parameters
    [SerializeField]
    GameObject CanvasPausa;
    [SerializeField]
    GameObject CanvasOpcionesPausa;
    #endregion

    #region methods
    public void pause()
    {
        CanvasPausa.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Continue ()
    {
        CanvasPausa.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Opciones()
    {
        CanvasPausa.SetActive(false);
        CanvasOpcionesPausa.SetActive(true);
    }
    public void Volver()
    {
        CanvasOpcionesPausa.SetActive(false) ;
        CanvasPausa.SetActive(true);
    }
    public void Salir()
    {
        Application.Quit();
    }
    #endregion 
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        CanvasPausa.SetActive(false);
        CanvasOpcionesPausa.SetActive(false) ;
        if (_playerMaster != null && _playerMaster != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _playerMaster = gameObject;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.activeSceneChanged += DestruirMenu;
    }

    void DestruirMenu(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
}
