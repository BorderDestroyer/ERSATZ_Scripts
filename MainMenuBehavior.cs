using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField]
    float _speed;

    float timer;
    [SerializeField]
    float timerREF;

    [SerializeField]
    GameObject RUS;
    [SerializeField]
    GameObject TitlePTOne;
    [SerializeField]
    GameObject TitlePTTwo;
    [SerializeField]
    GameObject TitleFull;
    [SerializeField]
    GameObject Menuing;
    [SerializeField]
    GameObject LevelSelect;

    AudioSource ass;
    [SerializeField]
    AudioClip swordEffect;
    [SerializeField]
    AudioClip titleMusic;

    [SerializeField]
    Button[] LevelSelectButtons;

    [SerializeField]
    Image whiteOut;

    bool runWhiteOut;
    void Start()
    {
        Time.timeScale = 1;

        Debug.Log(PlayerPrefs.GetInt("").ToString());
        Debug.Log(PlayerPrefs.GetInt("currentLevel").ToString());

        timer = 0;

        runWhiteOut = true;

        ass = GetComponent<AudioSource>();

        Debug.Log(PlayerPrefs.GetInt("LevelsComplete"));

        LevelSelect.SetActive(false);
        whiteOut.CrossFadeAlpha(.0001f, 0, true);
        TitleFull.SetActive(false);
        Menuing.SetActive(false);
        RUS.SetActive(false);

        if(PlayerPrefs.GetInt("LevelsComplete") <= 0)
        {
            LevelSelectButtons[0].interactable = true;
            LevelSelectButtons[1].interactable = false;
            LevelSelectButtons[2].interactable = false;
            LevelSelectButtons[3].interactable = false;
            LevelSelectButtons[4].interactable = false;
        }
        else if(PlayerPrefs.GetInt("LevelsComplete") <= 1)
        {
            LevelSelectButtons[0].interactable = true;
            LevelSelectButtons[1].interactable = true;
            LevelSelectButtons[2].interactable = false;
            LevelSelectButtons[3].interactable = false;
            LevelSelectButtons[4].interactable = false;
        }
        else if (PlayerPrefs.GetInt("LevelsComplete") <= 2)
        {
            LevelSelectButtons[0].interactable = true;
            LevelSelectButtons[1].interactable = true;
            LevelSelectButtons[2].interactable = true;
            LevelSelectButtons[3].interactable = false;
            LevelSelectButtons[4].interactable = false;
        }
        else if (PlayerPrefs.GetInt("LevelsComplete") <= 3)
        {
            LevelSelectButtons[0].interactable = true;
            LevelSelectButtons[1].interactable = true;
            LevelSelectButtons[2].interactable = true;
            LevelSelectButtons[3].interactable = true;
            LevelSelectButtons[4].interactable = false;
        }
        else if (PlayerPrefs.GetInt("LevelsComplete") <= 4)
        {
            LevelSelectButtons[0].interactable = true;
            LevelSelectButtons[1].interactable = true;
            LevelSelectButtons[2].interactable = true;
            LevelSelectButtons[3].interactable = true;
            LevelSelectButtons[4].interactable = true;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer < timerREF)
        {
            TitlePTOne.transform.Translate(Vector2.right * Time.deltaTime * _speed);
            TitlePTTwo.transform.Translate(Vector2.left * Time.deltaTime * _speed);
        }

        if(timer >= timerREF)
        {
            StartCoroutine(WhiteOut());
        }

        if(LevelSelect.active == true && Input.GetKeyDown(KeyCode.Escape))
        {
            switchLayers();
        }
    }

    IEnumerator WhiteOut()
    {
        if(runWhiteOut)
        {
            runWhiteOut = false;
            whiteOut.CrossFadeAlpha(2, .1f, true);
            yield return new WaitForSeconds(.08f);
            ass.clip = swordEffect;
            ass.loop = false;
            ass.Play();
            TitlePTOne.SetActive(false);
            TitlePTTwo.SetActive(false);
            TitleFull.SetActive(true);
            Menuing.SetActive(true);
            whiteOut.CrossFadeAlpha(0, .1f, true);
            yield return new WaitForSeconds(1f);
            ass.clip = titleMusic;
            ass.loop = true;
            ass.Play();
        }
        StopCoroutine(WhiteOut());
    }

    public void Usure()
    {
        RUS.SetActive(true);
    }

    public void no()
    {
        RUS.SetActive(false);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("LevelsComplete", 0);
        PlayerPrefs.SetInt("CanAttack", 0);
        StartCoroutine(LoadScene(1));
    }

    public void BeginSpecificLoad(int sceneInput)
    {
        StartCoroutine(LoadScene(sceneInput));
    }

    public void LoadURL(int ToLoad)
    {
        switch(ToLoad)
        {
            case 1:
                System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCKeSimtpEbtM1ZsA-Dqs6zg");
                break;
            case 2:
                System.Diagnostics.Process.Start("https://twitter.com/border_game");
                break;
            case 3:
                System.Diagnostics.Process.Start("https://www.instagram.com/borderdestroyer/");
                break;
        }

    }

    IEnumerator LoadScene(int SceneToLoad)
    {
        whiteOut.CrossFadeAlpha(2, 1, true);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void switchLayers()
    {
        if(Menuing.active == true && LevelSelect.active != true)
        {
            Menuing.SetActive(false);
            TitleFull.SetActive(false);
            LevelSelect.SetActive(true);
        }
        else if(Menuing.active != true && LevelSelect.active == true)
        {
            Menuing.SetActive(true);
            TitleFull.SetActive(true);
            LevelSelect.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }
}
