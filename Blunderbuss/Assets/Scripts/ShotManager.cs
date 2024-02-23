using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    #region references
    public Transform _myTransform;

    [SerializeField] GameObject _fuegoAire;
    [SerializeField] GameObject _fuegoSuelo;

    private SpriteRenderer _spriteRFA;
    private SpriteRenderer _spriteRFS;

    private BoxCollider2D _boxCollFA;
    private BoxCollider2D _boxCollFS;

    #endregion

    #region parameters
    #endregion

    void Start()
    {
        _myTransform = transform;

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
        float _faDist = 0.3f;
        float _fsDist = 0.3f;
        float intervalo = 0.3f;

        if (!suelo)
        {
            _fuegoAire.transform.position = _myTransform.position + new Vector3(position.x * _faDist, position.y * _faDist, 0f);
            _fuegoAire.transform.rotation = rotation;
            _boxCollFA.enabled = true;
            _spriteRFA.enabled = true;
            yield return new WaitForSeconds(intervalo);
            _boxCollFA.enabled = false;
            _spriteRFA.enabled = false;
        }
        else
        {
            _fuegoSuelo.transform.position = _myTransform.position + new Vector3(position.x * _fsDist, position.y * _fsDist, 0f);
            _fuegoSuelo.transform.rotation = rotation;
            _boxCollFS.enabled = true;
            _spriteRFS.enabled = true;
            yield return new WaitForSeconds(intervalo);
            _boxCollFS.enabled = false;
            _spriteRFS.enabled = false;
        }
    }
}
