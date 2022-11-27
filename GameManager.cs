using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int curScene;

    PlayerBehavior pb;

    public AudioSource ass;
    [SerializeField]
    AudioClip[] music;

    [SerializeField]
    GameObject levelEndScrene;
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject DedMenu;
    GameObject player;

    public bool playerDed;
    public bool levelEnd;
    void Start()
    {
        levelEnd = false;
        curScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("currentLevel", curScene);

        player = GameObject.FindGameObjectWithTag("Player");
        pb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
        ass = GetComponent<AudioSource>();

        StartCoroutine(NewMusic());

        DedMenu.SetActive(false);
        PauseMenu.SetActive(false);
        levelEndScrene.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else if((Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0))
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }

        if(playerDed)
        {
            DedMenu.SetActive(true);
        }

        if(levelEnd)
        {
            PlayerPrefs.SetInt("LevelsComplete", curScene);
            player.SetActive(false);
            levelEndScrene.SetActive(true);
        }
    }

    IEnumerator NewMusic()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        ass.clip = music[Random.Range(0, music.Length - 1)];
        ass.Play();
        StopCoroutine(NewMusic());
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(curScene + 1);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

    public void SomethingBroke()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        pb._health = 0;
        pb.UpdateHealth(0, false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DetermineRespawn()
    {
        curScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curScene);
    }
}
