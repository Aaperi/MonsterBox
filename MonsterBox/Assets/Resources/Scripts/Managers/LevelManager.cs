using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    private GameManager Gamemanager;
    public float LevelTime = 0,
                 Star1LevelTime = 30,  
                 Star2LevelTime = 20, 
                 Star3LevelTime = 10;
    public bool finnish = false;
    public int Stars,
                PickupCount,
                PickupCountFor1Star = 0,
                PickupCountFor2Star = 1, 
                PickupCountFor3Star = 2;
    // Use this for initialization
    void Start () {
        LevelTime = 0;
        PlayerPrefs.SetInt("pickupCount", 0);
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
        Gamemanager.LevelName = Application.loadedLevelName;
        Gamemanager.LevelEnd = false;
        if (Gamemanager.ResetData == false)
            Gamemanager.LoadLevelData();
    }
	
	// Update is called once per frame
	void Update () {
        TimeInLevel();
        
	}
    void TimeInLevel()
    {

        if (finnish)
        {
            calculateStars();
            Gamemanager.LevelEnd = true;
        }
        else
        {
            LevelTime += Time.deltaTime;
            PickupCount = PlayerPrefs.GetInt("pickupCount");
        }
    }
    void calculateStars()
    {
        //PickupCount = PlayerPrefs.GetInt("pickupCount");
        if (LevelTime < Star1LevelTime && PickupCount >= PickupCountFor1Star)
        {
            if (LevelTime < Star2LevelTime && PickupCount >= PickupCountFor2Star)
            {
                if (LevelTime < Star3LevelTime && PickupCount >= PickupCountFor3Star)
                    Stars = 3;
                else
                    Stars = 2;
            }
            else
                Stars = 1;
        }
        else
            Stars = 0;

        levelEnd();
        
    }
    void levelEnd()
    {
        //Debug.LogError("stars = " + Stars);
        if (Stars >= 1)
        {
            //unlockNextLevel = true;
            Gamemanager.UnlockNextLevel = true;
            int levelpickupsHighScore = Gamemanager.PickupHighscore;
            if (PickupCount > levelpickupsHighScore)
            {
                Gamemanager.PickupHighscore = PickupCount;
            }
            int starsOnLevel = Gamemanager.LevelStars;
            if (Stars > starsOnLevel)
            {
                Gamemanager.LevelStars = Stars;
            }
            float leveltimeHighscore = Gamemanager.LevelTimeHighScore;
            if (LevelTime < leveltimeHighscore || leveltimeHighscore == 0)
            {
                Gamemanager.LevelTimeHighScore = LevelTime;
            }
            Gamemanager.SaveLevelData();
        }
        
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.LogError("enter");
        finnish = true;
    }
}
