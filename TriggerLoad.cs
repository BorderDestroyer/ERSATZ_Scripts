using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoad : MonoBehaviour
{
    [SerializeField]
    GameObject ld;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject respawnPOS;

    [SerializeField]
    AudioSource ass;
    [SerializeField]
    AudioClip glitchSFX;

    GameManager gm;

    [SerializeField]
    GlitchEffect ge;
    
    [SerializeField]
    bool glitchOnLoad;

    void Start()
    {
        ld.SetActive(false);

        ass = GetComponent<AudioSource>();
        ge = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GlitchEffect>();
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(ld.active == true && Input.GetKeyDown(KeyCode.Return))
        {
            gm.ass.Play();
            ge.intensity = 0;
            ge.colorIntensity = 0;
            ge.flipIntensity = 0;
            ld.SetActive(false);
        }
    }

    void ChangeIntensity(float newIntensity)
    {
        ge.intensity = newIntensity;
        ge.colorIntensity = newIntensity;
        ge.flipIntensity = newIntensity;
    }

    void LoadNextScene()
    {
        Debug.Log("Next Scene Loaded");
        ChangeIntensity(.25f);
        ld.SetActive(true);
        player.transform.position = respawnPOS.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(glitchOnLoad)
            {
                gm.ass.Stop();
                ass.clip = glitchSFX;
                ass.Play();
                ge.intensity = 1;
                ge.colorIntensity = 1;
                ge.flipIntensity = 1;
                Invoke("LoadNextScene", 1.5f);
            }
            else
            {
                gm.levelEnd = true;
            }
        }
    }
}
