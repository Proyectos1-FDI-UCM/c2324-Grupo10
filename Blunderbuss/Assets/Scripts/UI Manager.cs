using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private ResourceManager _resourceManager;
    private SpriteRenderer _spriteRenderer;
    public GameObject  _rambutan;
    #endregion

    #region parameters
    private bool[] balas;
    private bool healAviable;
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
            _spriteRenderer.enabled = false;
        }
    }  
    public void gestionaBalas()
    {

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;   
        _resourceManager = _gameManager.ResourceManager;
        balas = new bool[_resourceManager.maxBalas];
        _rambutan = GameObject.FindGameObjectWithTag("Rambutan");
        _spriteRenderer = _rambutan.GetComponent<SpriteRenderer>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
