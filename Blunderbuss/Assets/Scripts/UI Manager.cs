using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;

    [SerializeField]
    BalasManager _balasManager;

    [SerializeField]
    VidaManager _vidaManager;

    [SerializeField]
    GameObject DeathCanvas;

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
    private float _vidaLength = 17.37f;
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
        _transformVida.localScale = new Vector3 (_vidaManager.health / _vidaManager.maxHealth * _vidaLength, _transformVida.localScale.y, _transformVida.localScale.z);
    }

    public void Rojo(bool eq)
    {
        if (_balasManager.BalaQuantity > 0)
        {
            SpriteRenderer spriteR = _sacos[_balasManager.BalaQuantity - 1].GetComponent<SpriteRenderer>();

            string colorT;

            if (eq)
                spriteR.color = new Color(1, 0.1f, 0, 1);
            else
                spriteR.color = new Color(0.8f, 0.33f, 0, 1);
        }
    }

    public void hasMuerto()
    {
        DeathCanvas.SetActive(true);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;   
        DeathCanvas.SetActive(false);

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
