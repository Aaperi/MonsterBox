using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	bool jump = false;
	bool right = false;
	bool left = false;
	bool crouch = false;
	bool grounded;

	public float speed = 1;
	public float maxSpeed = 10;
	public float jumpForce = 10;
	public LayerMask ground = 1;

	Rigidbody2D rb;
	CircleCollider2D maa;


	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		maa = GetComponentInChildren<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (rb.velocity);

		if(maa.IsTouchingLayers(ground))
		   grounded = true;
		else
		   grounded = false;

		if (grounded) {
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow))
				jump = true;
			else
				jump = false;
		

			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
				right = true;
			else
				right = false;

			if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
				left = true;
			else
				left = false;
		}

		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.DownArrow))
			crouch = true;
		else
			crouch = false;
	}

	void FixedUpdate(){

		if (jump)
			rb.AddForce (Vector2.up * jumpForce);

		if (right) 
			rb.velocity += Vector2.right * speed;

		if (left) 
			rb.velocity += Vector2.right * -speed;

	}
}
