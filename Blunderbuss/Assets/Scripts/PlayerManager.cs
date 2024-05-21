using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerManager : MonoBehaviour
{
    #region references
    private GameManager _gameManager;
    private InputManager _inputManager;
    private BalasManager _balasManager;
    private VidaManager _vidaManager;
    [SerializeField]
    CameraController _cameraController;
    private ShotManager _shotManager;
    [SerializeField]
    UIManager _UIManager;
    private PlayerVFX _vfx;
    private SFXCorsoManager _sfxCorso;
    public PlayerAnim playerAnim;

    public Transform myTransform;
    public Rigidbody2D playerRB;
    public SpriteRenderer spriteR;
    public BoxCollider2D boxColl;
    public Transform targetEnemy;
    #endregion

    #region parameters
    public int state = 1; //Estado 0: Suelo; Estado 1: Aire; Estado 2: Pared; Estado 3: Mov Bloqueado/Evento; Estado 4: Deslizamiento; Estado 5: Invulnerable; Estado 6: Pelotazo; 
    public bool invulnerable = false;

    public bool suelo;
    private float _speedGround = 4f;
    private float _speedAir = 3f;
    private float _speedWall = 2f;
    private float _airForce = 500f;
    private bool _slideEnable = false;
    public bool shotEnable = true;
    public bool ballBlowEnable = true;
    public bool chargeFinish = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        myTransform = playerRB.transform;
        spriteR = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();

        _gameManager = GameManager.Instance;
        _inputManager = _gameManager.InputManager;
        _balasManager = GetComponent<BalasManager>();
        _vidaManager = GetComponent<VidaManager>();
        _shotManager = GetComponent<ShotManager>();
        playerAnim = GetComponent<PlayerAnim>();
        _vfx = GetComponent<PlayerVFX>();
        _sfxCorso = GetComponent<SFXCorsoManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(state < 3)
            Move(_inputManager.axisX);
        print(chargeFinish);
    }

    public void Move(float axis)
    {
        if (state == 0)
        {
            if (axis > 0.5f)
            {
                playerRB.velocity = new Vector2(_speedGround, playerRB.velocity.y);
            }
            else if (axis < -0.5f)
            {
                playerRB.velocity = new Vector2(-_speedGround, playerRB.velocity.y);
            }
            else
            {
                playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            }
        }
        else if (state == 1 || state == 2)
        {
            if (axis > 0.5f && playerRB.velocity.x < _speedAir)
            {
                playerRB.AddForce(new Vector2(_airForce, 0), ForceMode2D.Force);
            }
            else if (axis < -0.5f && playerRB.velocity.x > -_speedAir)
            {
                playerRB.AddForce(new Vector2(-_airForce, 0), ForceMode2D.Force);
            }
        }
    }

    public IEnumerator Slide()
    {
        float _slideDur = 0.3f;
        float _slideCD = 0.3f;
        float _impulse = 560f;

        if (state == 0 && _slideEnable)
        {
            state = 4;
            _slideEnable = false;
            playerAnim.Slide();
            _sfxCorso.DeslizSFX();


            if (!spriteR.flipX)
            {
                playerRB.AddForce(new Vector2(_impulse, 0), ForceMode2D.Impulse);
            }
            else
            {
                playerRB.AddForce(new Vector2(-_impulse, 0), ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(_slideDur);
            if (state == 4)
                state = 0;

            yield return new WaitForSeconds(_slideCD);
            _slideEnable = true;
        }
    }

    private IEnumerator ShotTemp(Vector2 stop, Vector2 impulse, int mode)
    {
        StartCoroutine(_cameraController.ShakeBegin(2));

        state = 3;
        shotEnable = false;
        _slideEnable = false;

        float _shotCD = 0.3f;

        playerAnim.Shot(mode);
        playerRB.velocity = stop;
        playerRB.AddForce(impulse, ForceMode2D.Impulse);
        _balasManager.restaBala();

        yield return new WaitForSeconds(_shotCD);
        shotEnable = true;
        _slideEnable = true;
        if (!suelo)
            state = 1;
        else
            state = 0;
    }

    public void Shot(int dir)
    {
        float impulse;
        bool dispSuelo;

        if (shotEnable && _balasManager.BalaQuantity > 0)
        {
            if (state != 2)
            {
                switch (dir)
                {
                    case 0:
                        if (state == 0)
                        {
                            dispSuelo = false;
                            impulse = 700;
                        }
                        else
                        {
                            dispSuelo = false;
                            impulse = 1000f;
                        }

                        if (!spriteR.flipX)
                        {
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, new Vector2 (1f, 0.1f), Quaternion.identity, false));
                            StartCoroutine(ShotTemp(Vector2.zero, new Vector2(-impulse, 0), dir));
                        }
                        else
                        {
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, new Vector2(-1f, 0.1f), Quaternion.identity, true));
                            StartCoroutine(ShotTemp(Vector2.zero, new Vector2(impulse, 0), dir));
                        }
                        break;
                    case 1:
                        if (state == 0)
                        {
                            suelo = false;
                            playerAnim.Grounded(false);
                            dispSuelo = true;
                            impulse = 1000;
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.down, Quaternion.identity, false));
                        }
                        else if (state == 4)
                        {
                            suelo = false;
                            playerAnim.Grounded(false);
                            dispSuelo = true;
                            impulse = 1300f;
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.down, Quaternion.identity, false));
                        }
                        else
                        {
                            dispSuelo = false;
                            impulse = 800f;
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.down, Quaternion.Euler(0, 0, -90), false));
                        }
 
                        StartCoroutine(ShotTemp(new Vector2(playerRB.velocity.x, 0), new Vector2(0, impulse), dir));

                        break;
                    case 2:
                        impulse = 1400f;
                        if (state == 1)
                        {
                            dispSuelo = false;
                            StartCoroutine (_shotManager.FireSpawn(dispSuelo, Vector2.up, Quaternion.Euler(0, 0, 90), false));
                            StartCoroutine (ShotTemp(Vector2.zero, new Vector2(0, -impulse), dir));
                        }
                        else if (state == 0)
                        {
                            dispSuelo = false;
                            StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.up, Quaternion.Euler(0, 0, 90), false));
                            StartCoroutine(ShotTemp(Vector2.zero, Vector2.zero, dir));
                        }
                        break;
                }
            }
            else
            {
                impulse = 1000f;
                dispSuelo = true;

                if (spriteR.flipX)
                {
                    StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.left, Quaternion.Euler(0, 0, 90), true));
                    StartCoroutine(ShotTemp(Vector2.zero, new Vector2(impulse, impulse / 2f), dir));
                }


                else
                {
                    StartCoroutine(_shotManager.FireSpawn(dispSuelo, Vector2.right, Quaternion.Euler(0, 0, 90), false));
                    StartCoroutine(ShotTemp(Vector2.zero, new Vector2(-impulse, impulse / 2f), dir));
                }
                    
            }
        }
    }

    public IEnumerator BallBlowTemp(bool chargeFinish)
    {
        if (ballBlowEnable && _balasManager.BalaQuantity > 0)
        {
            _vfx.ActivaLLama();
            _UIManager.Rojo(true);
            int count = 3;

            yield return new WaitForSeconds(count);

            _vfx.DetonaLLama();
            SetBoolBB(true);
        }
    }

    public void SetBoolBB(bool eq)
    {
        chargeFinish = eq;
    }

    public IEnumerator BallBlow(bool chargeFinish)
    {
        SetBoolBB(false);
        if (chargeFinish && ballBlowEnable && _balasManager.BalaQuantity > 0 && !invulnerable)
        {
            ballBlowEnable = false;
            state = 6;
            shotEnable = false;
            _slideEnable = false;

            float shotCD = 0.3f;
            float buildUp = 0.3f;
            float smallJump = 300f;
            float rotationSpeed = 300f;
            float rotationSpeed2 = 2000f;
            float jumpStop = 0.1f;
            float impulse = 1500f;
            Vector3 dir = (targetEnemy.position - transform.position).normalized;
            Vector3 impDir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation;
            Quaternion targetRotation2;
            Quaternion targetRotation3;

            if (dir.x < 0)
            {
                spriteR.flipX = true;
                targetRotation = Quaternion.Euler(0, 0, angle + 180);
                impDir = myTransform.right;
            }
            else
            {
                spriteR.flipX = false;
                targetRotation = Quaternion.Euler(0, 0, angle);
                impDir = -myTransform.right;
            }

            playerRB.velocity = Vector3.zero;
            playerRB.gravityScale = 0;

            suelo = false;
            playerAnim.Grounded(false);
            playerAnim.Pelotazo();
            _sfxCorso.PelotazoSFX();
            if (suelo)
            {
                playerRB.AddForce(new Vector2(0, smallJump), ForceMode2D.Impulse);
                yield return new WaitForSeconds(jumpStop);
                playerRB.velocity = Vector3.zero;
            }
            else if (state == 2)
            {
                if (!spriteR.flipX)
                {
                    playerRB.AddForce(new Vector2(smallJump, 0), ForceMode2D.Impulse);
                    yield return new WaitForSeconds(jumpStop);
                    playerRB.velocity = Vector3.zero;
                }
                else
                {
                    playerRB.AddForce(new Vector2(-smallJump, 0), ForceMode2D.Impulse);
                    yield return new WaitForSeconds(jumpStop);
                    playerRB.velocity = Vector3.zero;
                }
            }

            if (!spriteR.flipX)
            {
                targetRotation = Quaternion.Euler(0, 0, angle);
                targetRotation2 = Quaternion.Euler(0, 0, angle + 135f);
                targetRotation3 = Quaternion.Euler(0, 0, angle + 270f);
                while (myTransform.rotation != targetRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
                impDir = myTransform.right;
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 0, angle + 180f);
                targetRotation2 = Quaternion.Euler(0, 0, angle + 45f);
                targetRotation3 = Quaternion.Euler(0, 0, angle - 90f);
                while (myTransform.rotation != targetRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
                impDir = -myTransform.right;
            }
            yield return new WaitForSeconds(buildUp);
            playerRB.gravityScale = 1;

            StartCoroutine(_cameraController.ShakeBegin(3));
            _shotManager.BallBlowSpawn(impDir);
            _UIManager.Rojo(false);
            _balasManager.restaBala();

            playerRB.AddForce(-impDir * impulse, ForceMode2D.Impulse);
            while (myTransform.rotation != targetRotation2)
            {
                transform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation2, rotationSpeed2 * Time.deltaTime);
                yield return null;
            }
            while (myTransform.rotation != targetRotation3)
            {
                transform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation3, rotationSpeed2 * Time.deltaTime);
                yield return null;
            }
            while (myTransform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.RotateTowards(myTransform.rotation, Quaternion.identity, rotationSpeed2 * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(shotCD);
            shotEnable = true;
            _slideEnable = true;

            if (!suelo)
                state = 1;
            else
                state = 0;

            ballBlowEnable = true;
        }
        else
            _UIManager.Rojo(false);
    }

    public IEnumerator Recarga()
    {
        float reloadCD = 0.75f;
        if (state == 0 && _balasManager.BalaQuantity != _balasManager.maxBalas)
        {
            SetBoolBB(false);
            _UIManager.Rojo(false);
            state = 3;
            playerRB.velocity = Vector2.zero;
            StartCoroutine(_balasManager.Recargar());
            playerAnim.Reload();
            _sfxCorso.RecargaSFX();
            yield return new WaitForSeconds(reloadCD);
            state = 0;
        }
    }

    public IEnumerator Cura()
    {
        float healCD = 0.75f;
        if (state == 0 && _vidaManager.health < _vidaManager.maxHealth && _vidaManager.HealQuantity > 0)
        {
            state = 3;
            playerRB.velocity = Vector2.zero;
            playerAnim.Heal();
            _sfxCorso.HealSFX();
            yield return new WaitForSeconds(healCD);
            if(!invulnerable)
                _vidaManager.Curarse();
            state = 0;
        }
    }

    public void Aturdimiento(bool aturdimiento)
    {
        _inputManager.enabled = aturdimiento;
    }

    public void Invulnerable(bool inv)
    {
        invulnerable = inv;
    }

    public void HitB()
    {
        playerAnim.Hit();
    }

    public void ResetColisiones()
    {
        spriteR.enabled = true;
        boxColl.enabled = true;
        playerRB.velocity = Vector2.zero;
    }

    public IEnumerator Parpadeo()
    {
        if (_vidaManager.health != 0)
        {
            for (int i = 0; i < 10; i++)
            {
                spriteR.enabled = !spriteR.enabled;
                yield return new WaitForSeconds(0.2f);
            }

            spriteR.enabled = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            suelo = true;
            playerAnim.Grounded(true);
            if (state != 3 && state != 6)
            {
                state = 0;
                _slideEnable = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared" && !suelo && state != 6)
        {
            playerAnim.Wall(true);
            state = 2;

            if (playerRB.velocity.y < 0)
                playerRB.velocity = new Vector2(playerRB.velocity.x, -_speedWall);

            if (collision.contacts[0].normal.x < 0)
                playerAnim.Flip(false);
            else if (!suelo && state != 6)
                playerAnim.Flip(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared")
        {
            playerAnim.Wall(false);
            if (state != 0 && state != 4 && state != 6)
            {
                state = 1;
            }
        }
    }
}
