using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnim : MonoBehaviour
{
    #region references
    public Animator corsoAnim;
    private PlayerManager _playerManager;
    private InputManager _inputManager;
    private GameManager _gameManager;
    private SpriteRenderer _spriteR;
    private VidaManager _vidaManager;
    #endregion

    #region parameters

    #endregion

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _spriteR = GetComponent<SpriteRenderer>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
        _vidaManager = GetComponent<VidaManager>();

        corsoAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Orient();
        CheckMov();
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
        Grounded(false);
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

    private void CheckMov()
    {
        if (Mathf.Abs(_playerManager.playerRB.velocity.x) > 0)
            corsoAnim.SetBool("Mov", true);
        else
            corsoAnim.SetBool("Mov", false);
    }

    public void Grounded(bool ground)
    {
        corsoAnim.SetBool("Suelo", ground);
    }

    public void Wall(bool mode)
    {
        corsoAnim.SetBool("Pared", mode);
    }

    public void Shot(int mode) //0: Horizontal     1: Abajo     2: Arriba
    {
        switch (mode)
        {
            case 0:
                corsoAnim.SetTrigger("DisparoH");
                break;
            case 1:
                corsoAnim.SetTrigger("Disparo-V");
                break;
            case 2:
                corsoAnim.SetTrigger("Disparo+V");
                break;
        }
    }

    public void Slide()
    {
        corsoAnim.SetTrigger("Slide");
    }

    public void Reload()
    {
        corsoAnim.SetTrigger("Recarga");
    }

    public void Heal()
    {
        corsoAnim.SetTrigger("Cura");
    }

    public void Hit()
    {
        if (_vidaManager.health != 0)
            corsoAnim.SetTrigger("Golpe");
        else
            corsoAnim.SetTrigger("Muerte");
    }

    public void Pelotazo()
    {
        corsoAnim.SetTrigger("Pelotazo");
    }

    public void Respawn()
    {
        corsoAnim.SetTrigger("Respawn");
    }
}
