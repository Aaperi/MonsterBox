using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiManager : MonoBehaviour
{
    private GameManager Gamemanager;
    private float leveltime;
    private int seconds, minutes;
    private bool GamePaused = false;
    public Text timerText;
    public float staranimationtimer;
    public Text pickupText, pickupTextLevelEnd, levelEndTime;
    public GameObject levelGui, levelClearedGui, Star1,Star2,Star3, NextLevelButton, pauseMenu, GameOver;
    //public Button RetryButton, ;
    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
        Time.timeScale = 1;
        levelClearedGui.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamemanager.LevelEnd == false)
        {
            leveltime += Time.deltaTime;
            minutes = Mathf.FloorToInt(leveltime / 60F); // get minutes from level
            seconds = Mathf.FloorToInt(leveltime - minutes * 60); // get seconds from level
                                                                  //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //make string to show time in nice format

            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            pickupText.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
        
        else
        {
            levelClearedGui.SetActive(true);
            levelGui.SetActive(false);
            //here level cleared screen code
            levelEndTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
            starAnimation();
        }
        if (Gamemanager.PlayerIsAlife == false)
        {
            GameOver.SetActive(true);
            levelGui.SetActive(false);
            Time.timeScale = 0;
        }
    }
    private void starAnimation()
    {
        int levelStars = Gamemanager.LevelStars;
        if (levelStars > 0)
            NextLevelButton.SetActive(true);
        else
            NextLevelButton.SetActive(false);

        ShowStars(levelStars);
    }

    private void ShowStars (int starCount) {
        switch ( starCount ) {
            case 1:
                Star1.SetActive(true);

                Star2.SetActive(false);
                Star3.SetActive(false);
            break;

            case 2:
                Star1.SetActive(true);
                staranimationtimer += Time.deltaTime;
                if ( staranimationtimer >= 0.5f )
                    Star2.SetActive(true);

                Star3.SetActive(false);
            break;

            case 3:
                Star1.SetActive(true);
                staranimationtimer += Time.deltaTime;
                if ( staranimationtimer >= 0.5f )
                    Star2.SetActive(true);
                if ( staranimationtimer >= 1 )
                    Star3.SetActive(true);
            break;

            default:
            Debug.LogError("Something went wrong.");
            break;
        }
    }

    public void NextPress()
    {
        //Application.LoadLevel(Application.loadedLevel);
    }
    public void retryPress()
    {
        Gamemanager.m_maxPickupCount = 0;
        Gamemanager.PlayerIsAlife = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.LoadLevel(Application.loadedLevel);
    }
    public void pause()
    {
        GamePaused = !GamePaused;
        pauseMenu.SetActive(GamePaused);
        if (GamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
