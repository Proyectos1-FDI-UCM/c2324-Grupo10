using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region references
    private InputManager _inputManager;

    [SerializeField]
    BalasManager _balasManager;

    [SerializeField]
    VidaManager _vidaManager;

    [SerializeField]
    GameObject DeathCanvas;

    private SpriteRenderer _spriteRendererRam;
    public SpriteRenderer[] spriteRendererSac;
    public SpriteRenderer[] spriteRendererSacC;
    private Transform _transformVida;

    public GameObject _healthBar;
    public GameObject  _rambutan;
    #endregion

    #region parameters
    public bool[] balas;
    private bool healAviable;
    private int sacosAviable;
    private float _vidaLength = 17.37f;
    #endregion

    #region methods
    public void gestionRambutan(bool inGame)                       //Gestionar si hay curacion disponible o no
    {
        if (inGame)
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

        else
        {
            _spriteRendererRam.enabled = true;
            _vidaManager.HealQuantity = 1;
        }
    }  
    public void quitaBala()
    {     
          spriteRendererSac[sacosAviable].enabled = false;  // test orden sacos
          spriteRendererSacC[sacosAviable].enabled = false;  // test orden sacos
          sacosAviable--;         
    }
    public void reiniciaBalas()
    {
        for (int i = 0; i < balas.Length; i++)
        {
            spriteRendererSac[i].enabled = true;
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
            SpriteRenderer spriteR = spriteRendererSac[_balasManager.BalaQuantity - 1];
            SpriteRenderer spriteRC = spriteRendererSacC[_balasManager.BalaQuantity - 1];

            if (eq)
            {
                spriteR.enabled = false;
                spriteRC.enabled = true;
            }
            else
            {
                spriteR.enabled = true;
                spriteRC.enabled = false;
            }
        }
    }

    public void hasMuerto()
    {
        StartCoroutine(PantallaMuerte());
    }

    private IEnumerator PantallaMuerte()
    {
        yield return new WaitForSeconds(2);

        DeathCanvas.SetActive(true);
        _inputManager.enabled = false;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    { 
        _inputManager = GameManager.Instance.GetComponent<InputManager>();

        DeathCanvas.SetActive(false);

        _rambutan = GameObject.FindGameObjectWithTag("Rambutan");
        _spriteRendererRam = _rambutan.GetComponent<SpriteRenderer>();

        _healthBar = GameObject.FindGameObjectWithTag("Vida");
        _transformVida = _healthBar.transform;

        sacosAviable = _balasManager.maxBalas - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
