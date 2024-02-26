using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private BalasManager _balasManager;
    private VidaManager _vidaManager;
    private SpriteRenderer _spriteRendererRam;
    private SpriteRenderer [] _spriteRendererSac;
    private Transform _transformVida;

    public GameObject _healthBar;
    public GameObject  _rambutan;
    public GameObject [] _sacos;
    #endregion

    #region parameters
    public bool[] balas;
    private bool healAviable;
    private int sacosAviable;
    #endregion

    #region methods
    public void gestionRambutan()                       //Gestionar si hay curacion disponible o no
    {
        if (_vidaManager.HealQuantity == 1)
        {
            _spriteRendererRam.enabled = true;
        }
        else if (_vidaManager.HealQuantity == 0)
        {
            healAviable = false;
        }
        if (!healAviable)
        {
            _spriteRendererRam.enabled = false;
        }
    }  
    public void quitaBala()
    {     
          _spriteRendererSac[sacosAviable].enabled = false;  // test orden sacos
          sacosAviable--;         
    }
    public void reiniciaBalas()
    {
        for (int i = 0; i < balas.Length; i++)
        {
            _spriteRendererSac[i].enabled = true;
        }
        sacosAviable = _balasManager.maxBalas-1;
    }
    public void actualizaVida()
    {
        _transformVida.localScale = new Vector3 (_vidaManager.health / _vidaManager.maxHealth,0,0);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;   
        //_resourceManager = _gameManager.ResourceManager;

        _rambutan = GameObject.FindGameObjectWithTag("Rambutan");
        _spriteRendererRam = _rambutan.GetComponent<SpriteRenderer>();

        _healthBar = GameObject.FindGameObjectWithTag("Vida");
        _transformVida = _healthBar.transform;

        sacosAviable = _balasManager.maxBalas - 1;

        _spriteRendererSac = new SpriteRenderer[_sacos.Length];
        for (int i = 0;i < _sacos.Length; i++)
        {
            _spriteRendererSac[i] = _sacos[i].GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
