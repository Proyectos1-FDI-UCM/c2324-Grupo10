using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    private GameManager _gameManager;
    private PlayerManager _playerManager;
    private VidaManager _vidaManager;
    private PauseMenu _pauseMenu;

    private Coroutine BBT;

    public float axisX = 0;

    public void Awake()
    {
        _inputActions = new InputActions();

        _inputActions.Player.Move.performed += ctx => axisX = ctx.ReadValue<float>();
        _inputActions.Player.Move.canceled += _ => axisX = 0;

        _inputActions.Player.Slide.started += ctx => StartCoroutine(_playerManager.Slide());

        //Dano
        _inputActions.Player.Shot.started += ctx => _playerManager.Shot(0);
        _inputActions.Player.Down_Shot.started += ctx => _playerManager.Shot(1);
        _inputActions.Player.Up_Shot.started += ctx => _playerManager.Shot(2);
        _inputActions.Player.Ball_Blow.started += ctx => BBT = StartCoroutine(_playerManager.BallBlowTemp(_playerManager.chargeFinish));
        _inputActions.Player.Ball_Blow.canceled += ctx =>
        {
            StopCoroutine(BBT);
            StartCoroutine(_playerManager.BallBlow(_playerManager.chargeFinish));
        };

        //Dani
        _inputActions.Player.Recharge.started += ctx =>  StartCoroutine(_playerManager.Recarga());
        _inputActions.Player.Potion.started += ctx => _vidaManager.Curarse();
        _inputActions.Player.Pause.started += ctx => _pauseMenu.pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _playerManager = _gameManager.Player.GetComponent<PlayerManager>();
        _vidaManager = _gameManager.Player.GetComponent<VidaManager>();
        _pauseMenu = EventSystem.current.GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
