using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerTouchMovement))]
public class PlayerTouchController : MonoBehaviour
{
    private GameManager Gamemanager;
    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;
    private float screenWidth,screenHeight;
    private PlayerTouchMovement m_movement;

    private void Awake()
    {
        m_movement = GetComponent<PlayerTouchMovement>();
    }

    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
        screenWidth = Screen.width / 100;
        screenHeight = Screen.height / 100;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.touchCount > 0)
            for (int i = 0; i < Input.touchCount; ++i)
            {

            Touch touch = Input.GetTouch(i);

            switch (touch.phase)
            {

                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Stationary:
                    if (startPos.x > screenWidth * 75)
                    {
                        m_movement.Move(1, false, false);
                    }
                    if (startPos.x < screenWidth * 35)
                    {
                        m_movement.Move(-1, false, false);
                    }
                    break;
                case TouchPhase.Ended:

                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;

                    if (swipeDistVertical > (screenHeight * minSwipeDistY))
                    {

                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                        if (swipeValue > 0 && m_movement.m_Grounded)
                        {
                            if (startPos.x > screenWidth * 70)
                            {
                                m_movement.Move(1, false, true);
                            }
                            if (startPos.x < screenWidth * 30)
                            {
                                m_movement.Move(-1, false, true);
                            }
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > (screenWidth * minSwipeDistX))

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        if (swipeValue > 0 && startPos.x > screenWidth * 70)//right swipe
                        {
                            m_movement.Move(1, true, false);
                        }

                        else if (swipeValue < 0 && startPos.x < screenWidth * 30)//left swipe
                        {
                            m_movement.Move(-1, true, false);
                        }
                    }
                    break;
            }
        }
    }
}
