using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinaComponent : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _mina;

    private Transform _myTransform;
    public Rigidbody2D rb;
    private CircleCollider2D _collTrigger;
    private ObstacleComponent _obstacle;
    private SpriteRenderer _spriteRenderer;
    #endregion

    #region parameters
    int state = 0;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Pared"))
        {
            _obstacle.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            state = 1;
            StartCoroutine(EsperaYPeta());
        }
        else if(collision.gameObject.CompareTag("Player") && state == 1)
        {
            StartCoroutine(ExplotaMina());
        }
    }

    
    private IEnumerator ExplotaMina()
    {
        _spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.1f);

        _collTrigger.enabled = false;
 
        yield return new WaitForSeconds(3);

        Destroy(_mina);
    }

    private IEnumerator EsperaYPeta()
    {
        yield return new WaitForSeconds(5f);
        
        _spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.1f);

        _collTrigger.enabled = false;

        yield return new WaitForSeconds(3);

        Destroy(_mina);
    }
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        _collTrigger = GetComponent<CircleCollider2D>();
        _obstacle = GetComponent<ObstacleComponent>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _obstacle.enabled = false;
        state = 0;
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
