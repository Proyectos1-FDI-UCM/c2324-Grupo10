using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullseyeManager : MonoBehaviour
{
    #region references
    private TutorialManager _tutorialManager;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _tutorialManager = TutorialManager.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fuego")
        {
            _tutorialManager.Dianas--;
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;

            StartCoroutine(_tutorialManager.PrimeraPuerta());
        }
    }
}
