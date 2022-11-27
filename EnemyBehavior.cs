using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    float _speed;
    [SerializeField]
    float _speedREF;
    float timer;
    float timerREF = 1;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    AudioSource ass;
    [SerializeField]
    AudioClip[] hurt;

    CameraBehavior cb;

    SpriteRenderer sr;

    [SerializeField]
    Vector2 movement;

    [SerializeField]
    int enemyTyping;
    [SerializeField]
    int _health;

    [SerializeField]
    bool dontMove;
    void Start()
    {
        _speed = 0;

        _speedREF = Random.Range(10, 20);

        cb = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>();
        ass = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(timer < timerREF)
        {
            timer += Time.deltaTime;
        }
        else if(timer > timerREF && _speed == 0)
        {
            _speed = _speedREF;
        }

        if(!dontMove)
        {
            rb.MovePosition(rb.position + movement * _speed * Time.deltaTime);
        }
    }

    void UpdateHealth(int healthInteger, bool adding)
    {
        if (adding)
        {
            _health += healthInteger;
        }
        else if (!adding)
        {
            _health -= healthInteger;
            ass.clip = hurt[Random.Range(0, hurt.Length - 1)];
            ass.Play();
        }

        if(_health <= 0)
        {
            Invoke("DestroySelf", .08f);
        }

    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyCollision") && enemyTyping == 1)
        {
            movement = movement * -1;

            if (movement.x == 1)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
        else if(collision.CompareTag("PlayerAttack"))
        {
            UpdateHealth(1, false);
        }
    }
}
