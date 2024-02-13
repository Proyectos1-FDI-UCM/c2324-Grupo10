using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerManager _playerManager;
    private InputManager _inputManager;
    private GameManager _gameManager;
    private SpriteRenderer _spriteR;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _spriteR = GetComponent<SpriteRenderer>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
    }

    // Update is called once per frame
    void Update()
    {
        Orient();
    }

    private void Orient()
    {
        if(_playerManager.state < 2)
        {
            if (_inputManager.axisX > 0)
            {
                _spriteR.flipX = false;
            }
            else if (_inputManager.axisX < 0)
            {
                _spriteR.flipX = true;
            }
        }
    }
}
