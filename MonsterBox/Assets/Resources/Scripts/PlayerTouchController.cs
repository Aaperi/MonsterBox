using UnityEngine;
using System.Collections;

public class PlayerTouchController : MonoBehaviour {
    private GameManager Gamemanager;
    private Vector2 m_touchStartPosition, m_touchEndPosition, m_touchMovedPosition;
    public bool m_InEditor = false;
    private bool m_Jump, m_Dash;
    private int screenWidth, screenHeigth;
    private float screenPercentWidth, screenPercentHeight, minSwipeDist = 50.0f;
    [SerializeField]private float m_MaxSpeed = 10f;// The fastest the player can travel in the x axis.
    [SerializeField]private float m_JumpForce = 400f;// Amount of force added when the player jumps.
    [SerializeField]private float m_DashForce = 20f;//Amount of dash force added when player dash
    [SerializeField]private bool m_AirControl = false;// Whether or not a player can steer while jumping;
    [SerializeField]private LayerMask m_WhatIsGround; // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public enum MoveState
    {
        Idle,
        RunRight,
        DashRight,
        JumpRight,
        RunLeft,
        DashLeft,
        JumpLeft,
    }
    MoveState m_moveState;
    public enum Direction
    {
        Left,
        right,
        idle,
    }
    Direction m_direction;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
        if (Application.isEditor)
            m_InEditor = true;
        m_moveState = MoveState.Idle;

        screenWidth = Screen.width;
        screenPercentWidth = screenWidth / 100;
        screenHeigth = Screen.height;
        screenPercentHeight = screenHeigth / 100;
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Gamemanager.LevelEnd)
            return;
        #region inputs
        if (!m_InEditor)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                switch (Input.GetTouch(i).phase)
                {
                    case TouchPhase.Began:
                        m_touchStartPosition = Input.GetTouch(i).position;
                        break;
                    case TouchPhase.Ended:
                        
                        m_touchMovedPosition = Input.GetTouch(i).position;
                        float gestureDist = (m_touchMovedPosition - m_touchStartPosition).magnitude;
                        Vector2 touchMovement = m_touchMovedPosition - m_touchStartPosition;
                        Vector2 swipeType = Vector2.zero;
                        if (gestureDist > minSwipeDist)
                        {
                            if (Mathf.Abs(touchMovement.x) > Mathf.Abs(touchMovement.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(touchMovement.x);

                            }
                            else {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(touchMovement.y);

                            }

                            if (swipeType.x != 0.0f)//swipe to dash
                            {
                                if (swipeType.x > 0.0f)
                                {

                                    // MOVE RIGHT
                                    if (m_touchStartPosition.x >= (screenPercentWidth * 75))
                                    {
                                       // m_Rigidbody2D.AddForce(new Vector2(m_DashForce, 0f));
                                    }
                                }
                                else if (swipeType.x < 0.0f)
                                {
                                    // MOVE LEFT
                                    if (m_touchStartPosition.x <= (screenPercentWidth * 25))
                                    {
                                        //m_Rigidbody2D.AddForce(new Vector2(-m_DashForce, 0f));
                                        
                                    }
                                }
                            }

                            if (swipeType.y != 0.0f)//swipe to jump
                            {
                                if (swipeType.y > 0.0f)
                                {

                                    if (m_touchStartPosition.x <= (screenPercentWidth * 25))
                                    {
                                        m_Rigidbody2D.AddForce(new Vector2(50f, m_JumpForce));
                                        
                                    }
                                    else if (m_touchStartPosition.x >= (screenPercentWidth * 75))
                                    {
                                        m_Rigidbody2D.AddForce(new Vector2(50f, m_JumpForce));
                                        
                                    }
                                }
                            }
                        }
                        break;
                    case TouchPhase.Canceled:
                        m_moveState = MoveState.Idle;
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                            //select move left and right from touch position
                            if (m_touchStartPosition.x <= (screenPercentWidth * 25))
                                m_moveState = MoveState.RunLeft;

                            if (m_touchStartPosition.x >= (screenPercentWidth * 75))
                                m_moveState = MoveState.RunRight;
                        break;
                }
                if (Input.touchCount == 0)
                    m_moveState = MoveState.Idle;  
            }
        }
        #region mouse
        else {
            if (Input.GetButtonDown("Fire1"))
            {
                m_touchStartPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
               

            }
            else if (Input.GetButton("Fire1"))
            {
                if (m_touchStartPosition.x <= (screenPercentWidth * 25))
                    m_moveState = MoveState.RunLeft;
                if (m_touchStartPosition.x >= (screenPercentWidth * 75))
                    m_moveState = MoveState.RunRight;

                m_touchMovedPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 mouseMovement = m_touchMovedPosition - m_touchStartPosition;

                if (mouseMovement.x >= 10 || mouseMovement.x <= -10)
                {
                    if (m_touchStartPosition.x > m_touchMovedPosition.x &&
                        m_touchStartPosition.x <= (screenPercentWidth * 25))
                    {
                        m_moveState = MoveState.DashLeft;
                    }
                    if (m_touchStartPosition.x < m_touchMovedPosition.x &&
                        m_touchStartPosition.x >= (screenPercentWidth * 75))
                    {
                        m_moveState = MoveState.DashRight;
                    }
                }
                else if (mouseMovement.y >= 10 && !m_Jump)
                {
                    if (m_touchStartPosition.y < m_touchMovedPosition.y &&
                        m_touchStartPosition.x <= (screenPercentWidth * 25))
                    {
                        m_moveState = MoveState.JumpLeft;
                        m_Jump = true;
                    }
                    if (m_touchStartPosition.y < m_touchMovedPosition.y &&
                        m_touchStartPosition.x >= (screenPercentWidth * 75))
                    {
                        m_moveState = MoveState.JumpRight;
                        m_Jump = true;
                    }
                }
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                m_touchEndPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else
            {
                m_moveState = MoveState.Idle;
            }   
        }
        #endregion
        #endregion

        #region Movestates
        switch (m_moveState)
        {
            case MoveState.Idle:
                m_direction = Direction.idle;
                m_Dash = false;
                m_Jump = false;
                break;
            case MoveState.RunRight:
                m_direction = Direction.right;
                break;
            case MoveState.DashRight:
                m_direction = Direction.right;
                m_Dash = true;
                break;
            case MoveState.JumpRight:
                m_direction = Direction.right;
                //m_Jump = true;
                break;
            case MoveState.RunLeft:
                m_direction = Direction.Left;
                break;
            case MoveState.DashLeft:
                m_direction = Direction.Left;
                m_Dash = true;
                break;
            case MoveState.JumpLeft:
                m_direction = Direction.Left;
                //m_Jump = true;
                break;
        }
        #endregion
        if(m_moveState != MoveState.Idle)
            Move(m_direction, m_Dash, m_Jump);
       
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }
    #region movements

    public void Move(Direction direction, bool dash, bool jump)
    {
        if (m_moveState == MoveState.Idle)
            return;
        float move = 0;
        if (direction == Direction.right)
            move = 1; 
        else if (direction == Direction.Left) 
            move = -1;

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
       
        m_moveState = MoveState.Idle;
        
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion

}
