using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaManager : MonoBehaviour
{
    #region references
    [SerializeField]
    UIManager _UIManager;
    private PlayerManager _playerManager;
    #endregion

    #region parameters
    public bool debug=false;
    public float maxHealth;                        // Por si queremos mejorar la vida del personaje
    public float health;
    public float healingValue = 50f;               // Por si queremos meter una mejora de que las pociones curan mas
    public int maxHeal = 1;                        // Por si queremos meter una mejora de mas curas
    public int HealQuantity;
    public float tiempoInvulnerable = 0.4f;        // Para que podamos ajustar sl tiempo de invulnerabilidad
    #endregion

    #region methods
    public void Curarse()
    {
        if (HealQuantity > 0)
        {
            health += healingValue;
            health = Mathf.Clamp(health, 0, maxHealth);
            HealQuantity--;
        }
        _UIManager.gestionRambutan(true);
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
        // Podemos tambien declarar delay seconds al principio del codigo para cambiar el tiempo de invulnerabilidad en funcion del boss
        if (_playerManager.state != 5)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            _playerManager.state = 5;
            _playerManager.Aturdimiento();
            _UIManager.actualizaVida();
            yield return new WaitForSeconds(tiempoInvulnerable);
            if (_playerManager.playerRB.velocity.y == 0)
                _playerManager.state = 0;
            else
                _playerManager.state = 1;
            _playerManager.Aturdimiento();
        }
        if (health <= 0)
        {
            _UIManager.actualizaVida();
            morir();
        }
    }
    public void morir()
    {      
         _UIManager.hasMuerto();       
        _playerManager.spriteR.enabled = false;
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
