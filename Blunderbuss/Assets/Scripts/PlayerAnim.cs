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
    private BoxCollider2D _boxColl;
    private VidaManager _vidaManager;
    #endregion

    #region parameters

    #endregion

    private void Awake()
    {
        corsoAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _spriteR = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
        _vidaManager = GetComponent<VidaManager>();
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
                Flip(false);
            }
            else if (_inputManager.axisX < 0)
            {
                Flip(true);
            }
        }
    }

    public void Flip(bool right)
    {
        if(right)
        {
            _spriteR.flipX = true;
            _boxColl.offset = new Vector2(Mathf.Abs(_boxColl.offset.x), _boxColl.offset.y);
        }
        else
        {
            _spriteR.flipX = false;
            _boxColl.offset = new Vector2(-Mathf.Abs(_boxColl.offset.x), _boxColl.offset.y);
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
        StartCoroutine(SlideBox());
    }

    private IEnumerator SlideBox()
    {
        Vector2 preOffset = _boxColl.offset;
        Vector2 preSize = _boxColl.size;

        Vector2 postOffset = new Vector2(_boxColl.offset.x * 1.8f, -1.59f);
        Vector2 postSize = new Vector2(4.93f, 2.58f);

        _boxColl.offset = postOffset;
        _boxColl.size = postSize;

        yield return new WaitForSeconds(0.4f);

        _boxColl.offset = preOffset;
        _boxColl.size = preSize;
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
