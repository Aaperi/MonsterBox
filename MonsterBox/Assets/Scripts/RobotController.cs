using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour 
{
	bool facingRight = true;
	public float maxSpeed = 10f;

	
	public bool onRope; //rope area or not
	public float climbSpeed;
	public float climbVelocity;
	public float gravityStore;
	
	private Rigidbody2D myrigidbody2D;


	void Start () 
	{
		myrigidbody2D = GetComponent<Rigidbody2D> ();
		
		gravityStore = myrigidbody2D.gravityScale;

	}

	void FixedUpdate () 
	{

		float move = Input.GetAxis ("Horizontal");
		
		GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		
		if (move > 0 &&!facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();

		if (move > 0 &&!facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();

		//rope climbing
		if (onRope) 
		{
			myrigidbody2D.gravityScale = 5f;
			climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
			myrigidbody2D.velocity = new Vector2(myrigidbody2D.velocity.x, climbVelocity);
		}
		
		if (!onRope) 
		{
			myrigidbody2D.gravityScale = gravityStore;
			
		}
	
	}

	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
