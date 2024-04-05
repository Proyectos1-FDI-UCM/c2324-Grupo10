using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    #region references
    private SpriteRenderer _spriteBoss;
    #endregion

    #region parameters
    public float maxHealth;
    public float health;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fuego"))
        {
            StartCoroutine(Damage());
        }
    }

    private IEnumerator Damage()
    {
        _spriteBoss.color =  new Color(1, 0.1f, 0, 1);

        yield return new WaitForSeconds(0.3f);

        _spriteBoss.color = new Color(1, 1, 1, 1);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _spriteBoss = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
