using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleComponent : MonoBehaviour
{
    #region references
    private VidaManager _vidaManager;
    private GameObject _player;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxColl;
    #endregion

    #region parameters
    public float pDamage;
    public float multiplier = 1;
    public int currLay = -1;
    private Transform _myTransform;
    #endregion

    #region methods
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
        if (playerManager && !playerManager.invulnerable)
        {
            StartCoroutine(_vidaManager.takeDamage(pDamage));
            Rebound(playerManager);
        }
    }

    private void Rebound(PlayerManager playerManager) //Empuja al Colega hacia arriba.
    {
        Vector3 dir = _player.transform.position - transform.position;
        if (playerManager.suelo)
        {
            playerManager.suelo = false;
            playerManager.state = 1;
        }
        
        Vector2 rebound = new(0,750);
        if(dir.x < 0)
        {
            rebound.x = -500;
        }
        else
        {
            rebound.x = +500;
        }
        playerManager.playerRB.velocity = Vector2.zero;
        playerManager.playerRB.AddForce(rebound * multiplier, ForceMode2D.Impulse);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _myTransform = transform;
        _vidaManager = _player.GetComponent<VidaManager>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _boxColl = _player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
