using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLevelManager : MonoBehaviour
{
    int count;

    float timer;
    [SerializeField]
    float[] timerREFS;

    GameObject mainCamera;
    GlitchEffect ge;

    [SerializeField]
    GameObject[] text;

    AudioSource ass;
    [SerializeField]
    AudioClip glitchSFX;
    void Start()
    {
        count = 0;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ge = mainCamera.GetComponent<GlitchEffect>();

        ass = GetComponent<AudioSource>();

        StartCoroutine(SwitchGlitchSFX());


    }

    IEnumerator SwitchGlitchSFX()
    {
        yield return new WaitForSeconds(1);
        ass.volume = .2f;
        ge.intensity = .2f;
        ge.colorIntensity = .2f;
        ge.flipIntensity = .2f;
        ass.loop = true;
        ass.clip = glitchSFX;
        ass.Play();
    }

    IEnumerator SwitchText(GameObject curText, GameObject newText)
    {
        curText.SetActive(false);
        yield return new WaitForSeconds(1f);
        newText.SetActive(true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timerREFS[count])
        {
            if(count == timerREFS.Length - 1)
            {
                Debug.Log("Game Exited");
                Application.Quit();
            }
            StartCoroutine(SwitchText(text[count], text[count + 1]));
 
            count += 1;
        }
    }
}
