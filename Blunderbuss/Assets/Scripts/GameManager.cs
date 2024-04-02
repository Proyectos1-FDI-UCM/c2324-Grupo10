using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region references
    static private GameManager _instance;
    private InputManager _inputManager;
    public GameObject Player;
    public VidaManager vidaManager;
    public BalasManager balasManager;
    public PlayerManager playerManager;
    static public GameManager Instance
    {
        get { return _instance; }
    }

    public InputManager InputManager
    {
        get { return _inputManager; }
    }

    #endregion

    #region parameters
    bool first = true;
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

        Player = GameObject.FindGameObjectWithTag("Player");

        playerManager = Player.GetComponent<PlayerManager>();
        vidaManager = Player.GetComponent<VidaManager>();
        balasManager = Player.GetComponent<BalasManager>();
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
        if (!first)
        {
            vidaManager.ResetVida();
            balasManager.RecargarInsta();
        }
        first = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
