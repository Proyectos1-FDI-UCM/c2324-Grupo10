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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                Destroy(gameObject);
                break;
            default:
                break;
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
