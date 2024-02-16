using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    [SerializeField] PlayerManager _playerManager;
    private ResourceManager _resourceManager;
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

        //Dani
        _inputActions.Player.Recharge.started += ctx =>  StartCoroutine(_resourceManager.Recargar());
        _inputActions.Player.Potion.started += ctx => _resourceManager.Curarse();
    }

    // Start is called before the first frame update
    void Start()
    {
        _resourceManager = GetComponent<ResourceManager>();
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
