using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public float LevelTime = 0,
                 Star1LevelTime = 30,  
                 Star2LevelTime = 20, 
                 Star3LevelTime = 10;
    public bool finnish = false, unlockNextLevel = false;
    public int Stars,
                PickupCount,
                PickupCountFor1Star = 0,
                PickupCountFor2Star = 1, 
                PickupCountFor3Star = 2;
    // Use this for initialization
    void Start () {
        LevelTime = 0;
        PlayerPrefs.SetInt("pickupCount", 0);
    }
	
	// Update is called once per frame
	void Update () {
        TimeInLevel();
        
	}
    void TimeInLevel()
    {
        if (finnish)
        {
            //Debug.LogError("used time in level: " + LevelTime);
            calculateStars();
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
        if (LevelTime < Star1LevelTime && LevelTime > Star2LevelTime && PickupCount == PickupCountFor1Star)
        {
            Stars = 1;
           
        }
        else if (LevelTime < Star2LevelTime && PickupCount == PickupCountFor2Star)
        {
            Stars = 2;
        }
        else if (LevelTime < Star3LevelTime && PickupCount == PickupCountFor3Star)
        {
            Stars = 3;
        }
        else
            Stars = 0;
        levelEnd();
        
    }
    void levelEnd()
    {
        Debug.LogError("stars = " + Stars);
        if (Stars >= 1)
            unlockNextLevel = true;
        
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.LogError("enter");
        finnish = true;
    }
}
