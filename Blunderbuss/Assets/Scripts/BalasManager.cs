using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalasManager : MonoBehaviour
{
    #region references
    [SerializeField]
    UIManager _UIManager;
    PlayerManager _playerManager;
    InputManager _inputManager;
    private PlayerVFX _vfx;
    #endregion

    #region parameters

    public int maxBalas = 6;                       // Por si queremos meter una mejora de mas balas
    public int BalaQuantity;
    public float reloadTime = 0.4f;                // Por si queremos cambiar la recarga

    #endregion

    #region methods
    // Esta es una primera version de los metodos que se aplican de forma instantanea, mas adelante tenemos que meter cooldowns y si queremos una forma para que la vida suba/baje progresivamente

    public void restaBala()
    {
        SpriteRenderer spriteR = _UIManager.spriteRendererSac[BalaQuantity-1];
        SpriteRenderer spriteRC = _UIManager.spriteRendererSacC[BalaQuantity-1];
        bool aux = false;
        if (spriteRC.enabled)
        {
            _UIManager.Rojo(false);
            aux = true;
        }
        _UIManager.Rojo(false);
        BalaQuantity--;
        if (BalaQuantity == 0)
            _vfx.DesactivaLLama();
        _UIManager.quitaBala();
        if (aux)
        {
            _UIManager.Rojo(true);
        }
    }
    public IEnumerator Recargar()
    {
        yield return new WaitForSeconds(reloadTime);  // Tiempo mientras se ejecuta la animacion de recarga
        if (!_playerManager.invulnerable)
            StartCoroutine(_vfx.RecargaVFX());
        _playerManager.SetBoolBB(false);
        _UIManager.Rojo(false);
        _inputManager.DesactivarBBT();
        BalaQuantity = maxBalas;                      // Se puede volver a disparar
        _UIManager.reiniciaBalas();                   // Puedes ver que puedes disparar otra vez
    }

    public void RecargarInsta()
    {
        _playerManager.SetBoolBB(false);
        _UIManager.Rojo(false);
        _inputManager.DesactivarBBT();
        BalaQuantity = maxBalas;                      // Se puede volver a disparar
        _UIManager.reiniciaBalas();                   // Puedes ver que puedes disparar otra vez
    }
    // El take damage tiene que ser testeado todavia, tenemos que hacer un enemigo que le quite vida al personaje
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BalaQuantity = maxBalas;
        _playerManager = GetComponent<PlayerManager>();
        _inputManager = GameManager.Instance.InputManager;
        _vfx = GetComponent<PlayerVFX>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
