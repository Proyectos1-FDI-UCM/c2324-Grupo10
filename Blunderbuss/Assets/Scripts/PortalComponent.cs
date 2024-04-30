using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalComponent : MonoBehaviour
{
    #region references
    [SerializeField] Sprite _portalD;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Animator _animatorPortal;

    private GameManager _gameManager;
    #endregion

    void Start()
    {
        _animatorPortal = GetComponent<Animator>();
        _gameManager = GameManager.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_gameManager.faunoDead && CompareTag("Fauno"))
        {
            _boxCollider.enabled = false;
            _animatorPortal.enabled = false;
            _spriteRenderer.sprite = _portalD;
        }

        if (_gameManager.serpicolDead && CompareTag("Serpicol"))
        {
            _boxCollider.enabled = false;
            _animatorPortal.enabled = false;
            _spriteRenderer.sprite = _portalD;
        }
    }
}
