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
    private float healingValue = 50f;               // Por si queremos meter una mejora de que las pociones curan mas
    private int maxBalas = 4;                       // Por si queremos meter una mejora de mas balas
    private int BalaQuantity;
    private int maxHeal = 1;                        // Por si queremos meter una mejora de mas curas
    private int HealQuantity;    
    #endregion

    #region methods
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
