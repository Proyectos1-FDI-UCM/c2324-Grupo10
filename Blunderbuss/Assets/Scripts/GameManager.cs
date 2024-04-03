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
    public CameraController cameraController;
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
        cameraController = Camera.main.GetComponent<CameraController>();
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

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                Player.transform.position = new Vector3(-16, 6.3f, 0);
                cameraController.limitLeft = -9.7f;
                cameraController.limitRight = 45.89f;
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Player.transform.position = new Vector3(0, -2.76f, 0);
                cameraController.limitLeft = 0;
                cameraController.limitRight = 0;
                print("3");
            }

            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                Player.transform.position = new Vector3(-15, 9, 0);
                playerManager.spriteR.flipX = false;
                cameraController.limitLeft = -9;
                cameraController.limitRight = 9;
            }
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
