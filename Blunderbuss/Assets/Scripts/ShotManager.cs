using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    #region references
    //private PlayerManager _playerManager;
    public Transform _myTransform;

    [SerializeField] GameObject _fuegoAire;
    [SerializeField] GameObject _fuegoSuelo;

    private SpriteRenderer _spriteRFA;
    private SpriteRenderer _spriteRFS;

    private BoxCollider2D _boxCollFA;
    private BoxCollider2D _boxCollFS;

    private float _faDist = 4f;
    private float _fsDist = 4f;
    #endregion

    #region parameters
    #endregion

    void Start()
    {
        _spriteRFA = _fuegoAire.GetComponent<SpriteRenderer>();
        _spriteRFS = _fuegoSuelo.GetComponent<SpriteRenderer>();

        _boxCollFA = _fuegoAire.GetComponent<BoxCollider2D>();
        _boxCollFS = _fuegoSuelo.GetComponent<BoxCollider2D>();
    }


    void Update()
    {

    }

    public IEnumerator FireSpawn(bool suelo, Vector2 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(0); //Lo pongo para que no de error. La corrutina siempre pide mínimo uno de estos.

        //El metodo consta de un if y un else. En ambos caso se hace exactamente lo mismo salvo que multiplicamos las componentes del vector2 por las diferentes dists.
        //Respectivamente activar boxColl y spriteR para desactivarlo despues del intervalo.
        //En position multiplicamos a x e y por _faDist/_fsDist y esperamos que cuando se invoque el metodo nos den un vector normalizado (up, down, left, right).
        //El quaternion se determina también al invocarlo (Quaternion.Euler(0, 0, 90), etc).

        //En un futuro implementaremos trigger animación.
    }
}
