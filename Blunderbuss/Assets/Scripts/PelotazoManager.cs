using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelotazoManager : MonoBehaviour
{
    #region references
    private PlayerManager _playerManager;
    private Transform _targetEnemy;
    private Transform _myTransform;

    private SpriteRenderer _spriteP;
    private CircleCollider2D _cirCollP;
    #endregion

    #region parameters
    private float _angulo;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _playerManager = GameManager.Instance.Player.GetComponent<PlayerManager>();

        _spriteP = GetComponent<SpriteRenderer>();
        _cirCollP = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Movement()
    {
        float speedP = 15;

        while (_myTransform.position != _playerManager.targetEnemy.position)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, _playerManager.targetEnemy.position, speedP * Time.deltaTime);
            Vector3 dir = _playerManager.targetEnemy.position - _myTransform.position;
            _angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _myTransform.rotation = Quaternion.AngleAxis(_angulo, Vector3.forward);
            yield return null;
        }

        _spriteP.enabled = false;
        _cirCollP.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            _spriteP.enabled = false;
            _cirCollP.enabled = false;
        }
    }
}
