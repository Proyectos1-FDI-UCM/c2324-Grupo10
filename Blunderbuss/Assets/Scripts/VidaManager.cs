using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaManager : MonoBehaviour
{
    #region references
    [SerializeField]
    UIManager _UIManager;
    private PlayerVFX _vfx;
    private PlayerManager _playerManager;
    #endregion

    #region parameters
    public bool debug=false;
    public float maxHealth;                        // Por si queremos mejorar la vida del personaje
    public float health;
    public float healingValue = 50f;               // Por si queremos meter una mejora de que las pociones curan mas
    public int maxHeal = 1;                        // Por si queremos meter una mejora de mas curas
    public int HealQuantity;
    #endregion

    #region methods
    public void Curarse()
    {
        health += healingValue;
        health = Mathf.Clamp(health, 0, maxHealth);
        HealQuantity--;
        _UIManager.gestionRambutan(true);
        StartCoroutine(_vfx.CuraVFX());
        _UIManager.actualizaVida();
    }

    public void ResetVida()
    {
        health = maxHealth;
        _UIManager.actualizaVida();
        _UIManager.gestionRambutan(false);
    }
    public IEnumerator takeDamage(float damage)
    {
        float tiempoInvulnerable = 1.6f;
        float tiempoAturdido = 0.4f;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        _playerManager.Invulnerable(true);
        _playerManager.Aturdimiento(false);
        
        StartCoroutine(_playerManager.Parpadeo());
        _UIManager.actualizaVida();
        yield return new WaitForSeconds(tiempoAturdido);
        _playerManager.Aturdimiento(true);
        if (health <= 0)
        {
            morir();
        }
        if (_playerManager.playerRB.velocity.y == 0)
            _playerManager.state = 0;
        else
            _playerManager.state = 1;
        yield return new WaitForSeconds(tiempoInvulnerable);
        if (health != 0)
        {
            _playerManager.Invulnerable(false);
        }
    }
    public void morir()
    {      
         _UIManager.hasMuerto();
    }

    public void RambutanTutorial()
    {
        if(health == maxHealth)
        {
            health = maxHealth / 2;
            _UIManager.actualizaVida();
        }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GameManager.Instance.Player.GetComponent<PlayerManager>();
        _vfx = GetComponent<PlayerVFX>();
        HealQuantity = maxHeal;
        maxHealth = 100f;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(debug)
        {
            print("Vida actual " + health);
        }
    }
}
