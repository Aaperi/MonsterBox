using UnityEngine;
using System.Collections;

public class BrokeningPlatform : MonoBehaviour
{
    public float BreakTime = 10f;
    public bool startBreak = false, pickup = false;
    private int pickupCount;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BreakTime > 0 && startBreak)
        {
            BreakTime -= Time.deltaTime;
        }
        else if(BreakTime <= 0 && startBreak)
        {
            if (pickup)
            {
                pickupCount = PlayerPrefs.GetInt("pickupCount");
                PlayerPrefs.SetInt("pickupCount", pickupCount + 1);
                Destroy(this.gameObject);
            }
           else
                Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    { 
        //Debug.LogError("enter");
        startBreak = true;
    }
}
