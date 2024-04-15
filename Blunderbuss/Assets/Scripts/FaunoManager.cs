using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FaunoManager : MonoBehaviour
{
    #region references
    private ObstacleComponent _obstacleComponent;
    private BossHealth _bossHealth;
    private Rigidbody2D _faunoRB;
    private Transform _myTransform;
    private SpriteRenderer _spriteF;
    private BoxCollider2D _boxColl;
    private GameObject _player;

    [SerializeField]
    float _paredIzq;
    [SerializeField]
    float _paredDer;

    public GameObject[] HipnoSpawn = new GameObject[8];
    public GameObject[] HipnoArea = new GameObject[8];
    public SpriteRenderer[] HipnoAS = new SpriteRenderer[8];
    public BoxCollider2D[] HipnoAB = new BoxCollider2D[8];

    private CameraController _camera;
    private FaunoAnimator _faunoAnimator;

    private int _direction = -1;
    #endregion

    #region parameters
    private bool _alive = true;
    #endregion

    #region methods
    private void Orient()
    {
        if (_alive)
        {
            Vector3 dist = _player.transform.position - _myTransform.position;

            if (dist.x < -2)
            {
                _direction = -1;

                if (_faunoAnimator.faunoAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !_spriteF.flipX)
                {
                    Mirror();
                }
            }

            else if (dist.x > 2)
            {
                _direction = 1;

                if (_faunoAnimator.faunoAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") && _spriteF.flipX)
                {
                    Mirror();
                }
            }
        }
    }

    private void Mirror()
    {
        _spriteF.flipX = !_spriteF.flipX;
        _boxColl.offset = new Vector2(-_boxColl.offset.x, _boxColl.offset.y);

        if (!_spriteF.flipX)
            _myTransform.position += Vector3.right * 4f;
        else
            _myTransform.position += Vector3.left * 4f;
    }

    #region attacks

    #endregion

    #region AI

    #endregion

    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _faunoRB = GetComponent<Rigidbody2D>();
        _bossHealth = GetComponent<BossHealth>();
        _boxColl = GetComponent<BoxCollider2D>();
        _spriteF = GetComponent<SpriteRenderer>();

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
