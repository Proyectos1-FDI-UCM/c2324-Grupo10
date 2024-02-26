using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region references
    static private GameManager _instance;
    private InputManager _inputManager;
    public GameObject Player;
    private VidaManager _vidaManager;
    private BalasManager _balasManager;
    private PlayerManager _playerManager;
    static public GameManager Instance
    {
        get { return _instance; }
    }

    public InputManager InputManager
    {
        get { return _inputManager; }
    }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            _inputManager = GetComponent<InputManager>();
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = Player.GetComponentInParent<PlayerManager>();
        _vidaManager = Player.GetComponentInParent<VidaManager>();
        _balasManager = Player.GetComponentInParent<BalasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
