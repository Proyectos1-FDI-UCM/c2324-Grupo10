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

    public Transform InitPosFaun;
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

    public bool serpicolDead = false;
    public bool faunoDead = false;
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
        GameObject initPos = GameObject.FindGameObjectWithTag("InitPos");
        if (!first)
        {
            vidaManager.ResetVida();
            balasManager.RecargarInsta();
            playerManager.ResetColisiones();
        }
        else
        {
            playerManager = Player.GetComponent<PlayerManager>();
            vidaManager = Player.GetComponent<VidaManager>();
            balasManager = Player.GetComponent<BalasManager>();
            cameraController = Camera.main.GetComponent<CameraController>();

            first = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Player.transform.position = initPos.transform.position;
            cameraController.limitLeft = -9.7f;
            cameraController.limitRight = 45.89f;
            playerManager.targetEnemy = GameObject.FindGameObjectWithTag("Boss").transform;
            playerManager.invulnerable = false;
            _inputManager.enabled = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Player.transform.position = initPos.transform.position;
            cameraController.limitLeft = 0;
            cameraController.limitRight = 0;

            if(serpicolDead)
            {
                GameObject serpicolP = GameObject.FindGameObjectWithTag("Serpicol");

                BoxCollider2D serpicolPBC = serpicolP.GetComponent<BoxCollider2D>();
                SpriteRenderer serpicolPS = serpicolP.GetComponent<SpriteRenderer>();

                serpicolPBC.enabled = false;
                serpicolPS.color = new Color(0, 0, 0);
            }

            if (faunoDead)
            {
                GameObject faunoP = GameObject.FindGameObjectWithTag("Fauno");

                BoxCollider2D faunoPBC = faunoP.GetComponent<BoxCollider2D>();
                SpriteRenderer faunoPS = faunoP.GetComponent<SpriteRenderer>();

                faunoPBC.enabled = false;
                faunoPS.color = new Color(0, 0, 0);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Player.transform.position = initPos.transform.position;
            playerManager.spriteR.flipX = false;
            cameraController.limitLeft = -9;
            cameraController.limitRight = 9;
            playerManager.targetEnemy = GameObject.FindGameObjectWithTag("Boss").transform;
            playerManager.invulnerable = false;
            _inputManager.enabled = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Player.transform.position = initPos.transform.position;
            playerManager.spriteR.flipX = false;
            cameraController.limitLeft = -9;
            cameraController.limitRight = 9;
            playerManager.targetEnemy = GameObject.FindGameObjectWithTag("Boss").transform;
            playerManager.invulnerable = false;
            _inputManager.enabled = true;
        }
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
