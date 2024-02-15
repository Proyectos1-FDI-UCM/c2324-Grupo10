using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    #region references
    private PlayerManager _playerManager;
    #endregion

    #region parameters
    private float maxHealth;                        // Por si queremos mejorar la vida del personaje
    private float health;                     
    public float healingValue = 50f;               // Por si queremos meter una mejora de que las pociones curan mas
    public int maxBalas = 4;                       // Por si queremos meter una mejora de mas balas
    private int BalaQuantity;
    public int maxHeal = 1;                        // Por si queremos meter una mejora de mas curas
    private int HealQuantity;
    public float tiempoInvulnerable = 0.5f;        // Para que podamos ajustar sl tiempo de invulnerabilidad
    #endregion

    #region methods
    //Esta es una primera version de los metodos que se aplican de forma instantanea, mas adelante tenemos que meter cooldowns y si queremos una forma para que la vida suba/baje progresivamente
    public void Curarse()
    {
        if (HealQuantity > 0)
        {
            health += healingValue;
            HealQuantity--;
        }
    }
    public void Recargar()
    {
        BalaQuantity = maxBalas; 
    }
    //El take damage tiene que ser testeado todavia, tenemos que hacer un enemigo que le quite vida al personaje
    public IEnumerator takeDamage(float damage, float delaySeconds)
    {
        delaySeconds = tiempoInvulnerable;
        //Podemos tambien declarar delay seconds al principio del codigo para cambiar el tiempo de invulnerabilidad en funcion del boss
        if (_playerManager.state != 5)
        {
            health -= damage;
            _playerManager.state = 5;
            yield return new WaitForSeconds(delaySeconds);
            _playerManager.state = 0;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BalaQuantity = maxBalas;
        HealQuantity = maxHeal;
        maxHealth = 100f;       
        health = maxHealth;       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void FixedUpdate()
    {
        //Temas de debug para ver si rulan las cosas
        print("Salud = " + health);      
        print("Curas disponibles = " + HealQuantity);      
        print("Disparos disponibles = " + BalaQuantity);      
    }
    */

}
