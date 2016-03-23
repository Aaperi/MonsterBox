using UnityEngine;
using System.Collections;

public class LevelGui : MonoBehaviour
{
    //todo: calculate screen size and make gui element scale from it
    private Rect TimewindowRect = new Rect(20, 20, 120, 50);
    private Rect PickupwindowRect = new Rect(145, 20, 120, 50);
    private Rect StarswindowRect = new Rect(270, 20, 120, 50);
    private float leveltime;
    private int seconds, minutes;
    private string niceTime;
    private GameManager Gamemanager;
    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        TimewindowRect = GUI.Window(0, TimewindowRect, Timebox, "Time");
        PickupwindowRect = GUI.Window(1, PickupwindowRect, pickupbox, "Pickups");
        StarswindowRect = GUI.Window(2, StarswindowRect, starsbox, "Stars high score");
    }
    void pickupbox(int windowID)
    {
        GUI.Label(new Rect(45, 20, 100, 30), PlayerPrefs.GetInt("pickupCount") + "/" + Gamemanager.MaxPickupCount);
    }
    void Timebox(int windowID)
    {
        if(Gamemanager.LevelEnd == false)
            leveltime += Time.deltaTime;
        minutes = Mathf.FloorToInt(leveltime / 60F); // get minutes from level
        seconds = Mathf.FloorToInt(leveltime - minutes * 60); // get seconds from level

        niceTime = string.Format("{0:00}:{1:00}", minutes, seconds); //make string to show time in nice format

        GUI.Label(new Rect(42, 20, 100, 30), niceTime);
    }
    void starsbox(int windowID)
    {
        // show star form level in high score
        if (Gamemanager.LevelStars == 1)
            GUI.Label(new Rect(55,20,100,30), "X"); 
        if (Gamemanager.LevelStars == 2)
            GUI.Label(new Rect(50, 20, 100, 30), "XX");
        if (Gamemanager.LevelStars == 3)
            GUI.Label(new Rect(42, 20, 100, 30), "XXX");
    }
}
