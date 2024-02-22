using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private ResourceManager _resourceManager;
    private SpriteRenderer _spriteRendererRam;
    private SpriteRenderer [] _spriteRendererSac;

    public GameObject  _rambutan;
    public GameObject [] _sacos;
    #endregion

    #region parameters
    public bool[] balas;
    private bool healAviable;
    private int sacosAviable;
    #endregion

    #region methods
    public void gestionRambutan()
    {
        if (_resourceManager.HealQuantity == 0)
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
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;   
        _resourceManager = _gameManager.ResourceManager;
        balas = new bool[_resourceManager.maxBalas];

        _rambutan = GameObject.FindGameObjectWithTag("Rambutan");
        _spriteRendererRam = _rambutan.GetComponent<SpriteRenderer>();

        sacosAviable = _resourceManager.maxBalas - 1;

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
