using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleComponent : MonoBehaviour
{
    #region references
    private PlayerManager _playerManager;
    private VidaManager _vidaManager;
    private GameObject _player;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxColl;
    #endregion

    #region parameters
    public float pDamage;
    public float multiplier = 1;
    public int currLay = -1;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != currLay && gameObject.activeSelf)
        {
            StartCoroutine(CollDes(other));
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager)
            {
                StartCoroutine(_vidaManager.takeDamage(pDamage));
                Rebound(playerManager.playerRB);
            }
        }
    }

    private IEnumerator CollDes(Collider2D other)
    {
        currLay = other.gameObject.layer;
        yield return new WaitForSeconds(0.4f);
        if (other.gameObject.layer == currLay)
            currLay = -1;
    }

    private void Rebound(Rigidbody2D rb) //Empuja al Colega hacia arriba.
    {
        _playerManager.suelo = false;
        Vector2 rebound = new(0,750);
        if(rb.velocity.x>=0)
        {
            rebound.x = -500;
        }
        else
        {
            rebound.x = +500;
        }
        rb.velocity = Vector2.zero;
        rb.AddForce(rebound * multiplier, ForceMode2D.Impulse);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _vidaManager = _player.GetComponent<VidaManager>();
        _playerManager = _player.GetComponent<PlayerManager>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _boxColl = _player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
