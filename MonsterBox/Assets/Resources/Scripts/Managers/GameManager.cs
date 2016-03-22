using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public string currentLevelName;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region level data gets and sets
    //level data
    public string LevelName
    {
        get { return currentLevelName; }
        set { currentLevelName = value; }
    }
    public bool m_unlockNext;
    public bool UnlockNextLevel
    {
        get { return m_unlockNext; }
        set { m_unlockNext = value; }
    }
    public int m_stars;
    public int LevelStars
    {
        get { return m_stars; }
        set { m_stars = value; }
    }
    public float m_levelTime;
    public float LevelTimeHighScore
    {
        get { return m_levelTime; }
        set { m_levelTime = value; }
    }
    public int m_pickupsCount;
    public int PickupHighscore
    {
        get { return m_pickupsCount; }
        set { m_pickupsCount = value; }
    }
    #endregion

    #region save and load level data
    public void LoadLevelData()
    {
        m_stars = PlayerPrefs.GetInt(currentLevelName + " stars");
        m_pickupsCount = PlayerPrefs.GetInt(currentLevelName + " pickups");
        m_levelTime = PlayerPrefs.GetFloat(currentLevelName + " time");
        if (PlayerPrefs.GetInt(currentLevelName + " unlocknext") == 1)
            m_unlockNext = true;
        else
            m_unlockNext = false;
    }
    public void SaveLevelData()
    {
        PlayerPrefs.SetInt(currentLevelName + " stars", m_stars);
        PlayerPrefs.SetInt(currentLevelName + " pickups", m_pickupsCount);
        PlayerPrefs.SetFloat(currentLevelName + " time", m_levelTime);
        if (m_unlockNext)
            PlayerPrefs.SetInt(currentLevelName + " unlocknext", 1);
        else
            PlayerPrefs.SetInt(currentLevelName + " unlocknext", 1);
    }
    #endregion
}
