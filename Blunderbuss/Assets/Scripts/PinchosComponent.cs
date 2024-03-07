using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PinchosComponent : MonoBehaviour
{
    #region references
    private VidaManager _vidaManager;
    private GameObject _player;
    private Rigidbody2D _rb;
    #endregion

    #region parameters
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        float damage = 20;
        float delaySeconds = 0.5f;
        PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
        if (playerManager)
        {
            StartCoroutine(_vidaManager.takeDamage(damage, delaySeconds));
            Rebound(playerManager.playerRB);
            print("pene");
        }
    }

    private void Rebound(Rigidbody2D rb) //Empuja al Colega hacia arriba.
    {
        Vector2 rebound = new(0,750);
        if(rb.velocity.x>=0)
        {
            rebound.x = -200;
        }
        else
        {
            rebound.x = +200;
        }
        rb.velocity = Vector2.zero;
        rb.AddForce(rebound, ForceMode2D.Impulse);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _vidaManager = _player.GetComponent<VidaManager>();
        _rb = _player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
