using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalasManager : MonoBehaviour
{
    #region references
    [SerializeField]
    UIManager _UIManager;
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
        BalaQuantity--;
        _UIManager.quitaBala();
    }
    public IEnumerator Recargar()
    {
        yield return new WaitForSeconds(reloadTime);  // Tiempo mientras se ejecuta la animacion de recarga
        BalaQuantity = maxBalas;                      // Se puede volver a disparar
        _UIManager.reiniciaBalas();                   // Puedes ver que puedes disparar otra vez
    }
    // El take damage tiene que ser testeado todavia, tenemos que hacer un enemigo que le quite vida al personaje

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BalaQuantity = maxBalas;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
