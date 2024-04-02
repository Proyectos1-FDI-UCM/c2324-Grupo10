using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullseyeManager : MonoBehaviour
{
    #region references
    [SerializeField]
    TutorialManager _tutorialManager;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fuego" && !gameObject.CompareTag("Boss"))
        {
            _tutorialManager.Dianas--;
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;

            StartCoroutine(_tutorialManager.PrimeraPuerta());
        }

        if (collision.gameObject.tag == "Fuego2" && gameObject.CompareTag("Boss"))
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;

            StartCoroutine(_tutorialManager.SegundaPuerta());
        }
    }
}
