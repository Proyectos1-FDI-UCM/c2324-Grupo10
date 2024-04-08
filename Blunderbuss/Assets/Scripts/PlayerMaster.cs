using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMaster : MonoBehaviour
{
    #region references
    static private GameObject _playerMaster;
    /*static public GameObject playerMaster;
    {
        get { return _playerMaster; }
    }*/
    private PlayerManager _playerManager;
    private VidaManager _vidaManager;
    private BalasManager _balasManager;
    #endregion

    private void Awake()
    {
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

    // Start is called before the first frame update
    void Start()
    {
        /*_playerManager = GameManager.Instance.playerManager;
        _vidaManager = GameManager.Instance.vidaManager;
        _balasManager = GameManager.Instance.balasManager;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
