using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float _speed;
    float _speedREF;
    float _gravityREF;
    float animSpeedREF;
    float IFrameCountdown;
    [SerializeField]
    float IFrameREF;

    public int CheckPointInteger;
    public int _health;
    int maxHealth;
    public int CurrentRespawn;

    [SerializeField]
    GameObject[] ap;
    [SerializeField]
    GameObject melee;
    [SerializeField]
    GameObject[] RespawnPositions;

    [SerializeField]
    Image[] heartIcons;

    [SerializeField]
    Sprite GoodHeart;
    [SerializeField]
    Sprite BadHeart;

    CameraBehavior cb;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Color basicBitch;
    [SerializeField]
    Color slightlyDarker;

    Vector2 movement;
    Vector2 position;

    [SerializeField]
    bool grounded;
    [SerializeField]
    bool jumped;
    [SerializeField]
    bool canJump;
    bool meleeAttacking;
    bool slowedDown;
    [SerializeField]
    bool invincible;
    bool attackedInAir;

    public bool canAttack;

    AudioSource ass;
    [SerializeField]
    AudioClip[] jumpSFX;
    [SerializeField]
    AudioClip[] hurt;

    SpriteRenderer sr;

    Animator anim;

    GameManager gm;
    void Start()
    {
        if(PlayerPrefs.GetInt("CanAttack") == 1)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        attackedInAir = false;
        invincible = false;
        slowedDown = false;
        jumped = false;
        CurrentRespawn = 0;

        ass = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        cb = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        _speedREF = _speed;
        animSpeedREF = anim.speed;
        _gravityREF = rb.gravityScale;
    }

    void Update()
    {
        #region movement
        movement.x = Input.GetAxis("Horizontal");

        if(movement.x < 0 && !slowedDown && !meleeAttacking)
        {
            sr.flipX = true;
        }
        else if(movement.x > 0 && !slowedDown && !meleeAttacking)
        {
            sr.flipX = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("jumped set to true");
            jumped = true;
        }
        #endregion

        #region meleeAttacking
        if (Input.GetKeyDown(KeyCode.UpArrow) && !meleeAttacking && !slowedDown && canAttack)
        {
            meleeAttacking = true;
            anim.Play("UpAttack");
            melee.transform.position = ap[0].transform.position;
            melee.transform.rotation = Quaternion.Euler(melee.transform.rotation.x, melee.transform.rotation.y, 90);
            melee.SetActive(true);
            Invoke("ResetMelee", .4f);
            Invoke("ResetMovement", .4f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !meleeAttacking && !slowedDown && canAttack)
        {
            sr.flipX = true;
            anim.Play("SideAttack");
            meleeAttacking = true;
            melee.transform.position = ap[1].transform.position;
            melee.transform.rotation = Quaternion.Euler(melee.transform.rotation.x, melee.transform.rotation.y, 0);
            melee.SetActive(true);
            Invoke("ResetMelee", .5f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !meleeAttacking && !slowedDown && canAttack)
        {
            sr.flipX = false;
            anim.Play("SideAttack");
            meleeAttacking = true;
            melee.transform.position = ap[2].transform.position;
            melee.transform.rotation = Quaternion.Euler(melee.transform.rotation.x, melee.transform.rotation.y, 0);
            melee.SetActive(true);
            Invoke("ResetMelee", .5f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !meleeAttacking && jumped && !slowedDown && !attackedInAir && canAttack)
        {
            attackedInAir = true;
            meleeAttacking = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            anim.Play("DownAttack");
            rb.gravityScale = _gravityREF / 8;
            melee.transform.position = ap[3].transform.position;
            melee.transform.rotation = Quaternion.Euler(melee.transform.rotation.x, melee.transform.rotation.y, 90);
            melee.SetActive(true);
            Invoke("ResetMelee", .8f);
            Invoke("ResetMovement", .8f);
        }
        #endregion

        #region animations
        if (movement.x == 0 && !jumped && !meleeAttacking)
        {
            anim.Play("PlayerIdle");
        }
        else if(movement.x != 0 && !jumped && !meleeAttacking)
        {
            anim.Play("PlayerRun");
        }

        if(slowedDown)
        {
            anim.speed = animSpeedREF / 10;
        }
        else if(!slowedDown)
        {
            anim.speed = animSpeedREF;
        }
        #endregion

        #region IFrames
        if (invincible)
        {
            IFrameCountdown += Time.deltaTime;
            if(IFrameCountdown >= IFrameREF)
            {
                IFrameCountdown = 0;
                invincible = false;
                sr.color = basicBitch;
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * movement * _speed * Time.deltaTime);

        if(jumped && canJump)
        {
            Debug.Log("jumped?");
            ass.clip = jumpSFX[Random.Range(0, jumpSFX.Length - 1)];
            ass.Play();
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            grounded = false;
            canJump = false;
        }
    }

    IEnumerator Flashing()
    {
        while(invincible)
        {
            Debug.Log("Flashing");
            sr.color = slightlyDarker;
            yield return new WaitForSeconds(.1f);
            sr.color = basicBitch;
            yield return new WaitForSeconds(.1f);
        }
    }

    void ResetMelee()
    {
        melee.SetActive(false);
        meleeAttacking = false;
    }

    void ResetMovement()
    {
        anim.Play("PlayerIdle");
        slowedDown = false;
        _speed = _speedREF;
        rb.gravityScale = _gravityREF;
    }

    void ResetJump()
    {
        canJump = true;
    }

    public void UpdateHealth(int healthInteger, bool adding)
    {
        if(adding)
        {
            _health += healthInteger;
        }
        else if(!adding && !invincible)
        {
            invincible = true;
            ass.clip = hurt[Random.Range(0, hurt.Length - 1)];
            ass.Play();
            Debug.Log("IFRAMES Active");
            StartCoroutine(Flashing());
            _health -= healthInteger;
        }

        switch (_health)
        {
            case 3:
                heartIcons[0].sprite = GoodHeart;
                heartIcons[1].sprite = GoodHeart;
                heartIcons[2].sprite = GoodHeart;
                break;
            case 2:
                heartIcons[0].sprite = GoodHeart;
                heartIcons[1].sprite = GoodHeart;
                heartIcons[2].sprite = BadHeart;
                break;
            case 1:
                heartIcons[0].sprite = GoodHeart;
                heartIcons[1].sprite = BadHeart;
                heartIcons[2].sprite = BadHeart;
                break;
            case 0:
                heartIcons[0].sprite = BadHeart;
                heartIcons[1].sprite = BadHeart;
                heartIcons[2].sprite = BadHeart;
                gm.playerDed = true;
                Destroy(this.gameObject);
                break;
        }
    }

    void DestroyGameObject(GameObject toDestroy)
    {
        Destroy(toDestroy.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("environment"))
        {
            rb.gravityScale = _gravityREF;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            sr.flipY = false;
            jumped = false;
            grounded = true;
            canJump = true;
            attackedInAir = false;
        }
        else if(collision.CompareTag("PassBarrier"))
        {
            canJump = false;
            slowedDown = true;
            cb.MoveToNextPoint = true;
            _speed = 1;
            CurrentRespawn += 1;
            rb.gravityScale = .1f;
            rb.velocity = new Vector2(0, 0);
            Invoke("ResetJump", 1);
            Invoke("ResetMovement", 1f);
        }
        else if(collision.CompareTag("Deathplane"))
        {
            UpdateHealth(1, false);
            transform.position = RespawnPositions[CurrentRespawn].transform.position;
        }
        else if(collision.CompareTag("Enemy"))
        {
            UpdateHealth(1, false);
        }
    }
}
