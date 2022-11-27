using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    PlayerBehavior pb;
    
    [SerializeField]
    bool swordPickup;

    AudioSource ass;
    [SerializeField]
    AudioClip pickupSFX;

    private void Start()
    {
        ass = GetComponent<AudioSource>();
        pb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && swordPickup)
        {
            ass.clip = pickupSFX;
            ass.Play();
            PlayerPrefs.SetInt("CanAttack", 1);
            pb.canAttack = true;
            Invoke("DestroySelf", .1f);
        }
    }
}
