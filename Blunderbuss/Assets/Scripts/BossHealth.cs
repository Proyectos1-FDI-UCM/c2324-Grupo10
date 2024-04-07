using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    #region references
    private SpriteRenderer _spriteBoss;
    [SerializeField]
    GameObject HealthBar;
    [SerializeField]
    GameObject Boss;
    private Transform _HealthBarTra;
    #endregion

    #region parameters
    public bool debug;
    public float maxHealth;
    public float health;
    public float DamageAmmount;
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
        DamageAmmount = 100f;
        ActualizaVida();

        yield return new WaitForSeconds(0.3f);

        _spriteBoss.color = new Color(1, 1, 1, 1);
    }

    private void ActualizaVida()
    {
        health = Mathf.Clamp(health - DamageAmmount, 0, maxHealth); 
        _HealthBarTra.localScale = new Vector3(6.5122f*(health / maxHealth) , 0.27501f, 1);
        if (health <= 0)
        {
            Muere();
        }
    }

    private void Muere()
    {

    }
    #endregion

    private void Awake()
    {
        
    }
    void Start()
    {
        _spriteBoss = GetComponent<SpriteRenderer>();
        health = maxHealth;
        _HealthBarTra = HealthBar.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (debug)
        {
            print("Vida actual " + health);
        }
    }

}
