using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    private GameManager Gamemanager;
    private float leveltime;
    private int seconds, minutes;
    public Text timerText;
    public Text pickupText, pickupTextLevelEnd;
    public GameObject levelGui, levelClearedGui, Star1,Star2,Star3;
    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();

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
            starAnimation();
        }
    }
    private void starAnimation()
    {
        int levelstars = Gamemanager.LevelStars;
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
            Star2.active = true;
            Star3.active = false;

            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
        if (levelstars == 3)
        {
            Star1.active = true;
            Star2.active = true;
            Star3.active = true;

            pickupTextLevelEnd.text = PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount;
        }
    }
}
