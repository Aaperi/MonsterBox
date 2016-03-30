using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    private GameManager Gamemanager;
    private float leveltime;
    private int seconds, minutes;
    private bool GamePaused = false;
    public Text timerText;
    public float staranimationtimer;
    public Text pickupText, pickupTextLevelEnd, levelEndTime;
    public GameObject levelGui, levelClearedGui, Star1,Star2,Star3, NextLevelButton, pauseMenu;
    //public Button RetryButton, ;
    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
        Time.timeScale = 1;
        levelClearedGui.active = false;
        
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
            levelClearedGui.active = true;
            levelGui.active = false;
            //here level cleared screen code
            levelEndTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            starAnimation();
        }
    }
    private void starAnimation()
    {
        int levelstars = Gamemanager.LevelStars;
        if (levelstars > 0)
            NextLevelButton.active = true;
        else
            NextLevelButton.active = false;

        if (levelstars == 1)
        {
            Star1.active = true;
            
            Star2.active = false;
            Star3.active = false;

            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
        if (levelstars == 2)
        {
            Star1.active = true;
            staranimationtimer += Time.deltaTime;
            if(staranimationtimer >= 0.5f)
                Star2.active = true;

            Star3.active = false;

            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
        if (levelstars == 3)
        {
            Star1.active = true;
            staranimationtimer += Time.deltaTime;
            if (staranimationtimer >= 0.5f)
                Star2.active = true;
            if (staranimationtimer >= 1)
                Star3.active = true;

            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
    }

    public void NextPress()
    {
        //Application.LoadLevel(Application.loadedLevel);
    }
    public void retryPress()
    {
        Gamemanager.m_maxPickupCount = 0;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void pause()
    {
        GamePaused = !GamePaused;
        pauseMenu.active = GamePaused;
        if(GamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
