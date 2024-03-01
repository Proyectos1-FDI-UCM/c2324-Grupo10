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
    public float tiempoInvulnerable = 0.5f;        // Para que podamos ajustar sl tiempo de invulnerabilidad
    #endregion

    #region methods
    public void Curarse()
    {
        if (HealQuantity > 0)
        {
            health += healingValue;
            HealQuantity--;
        }
        _UIManager.gestionRambutan();
        _UIManager.actualizaVida();
    }
    public IEnumerator takeDamage(float damage, float delaySeconds)
    {
        delaySeconds = tiempoInvulnerable;
        // Podemos tambien declarar delay seconds al principio del codigo para cambiar el tiempo de invulnerabilidad en funcion del boss
        if (_playerManager.state != 5)
        {
            health -= damage;
            _playerManager.state = 5;
            yield return new WaitForSeconds(delaySeconds);
            _playerManager.state = 0;
        }
        _UIManager.actualizaVida();
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
