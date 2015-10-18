using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

	public float speed;
	public float maxSpeed;
	public float jumpForce;
	
	GameObject Player;
	Rigidbody2D rigid;
	BoxCollider2D box;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		rigid = Player.GetComponent<Rigidbody2D> ();
		box = GameObject.FindGameObjectWithTag("Ground").GetComponent<BoxCollider2D> ();
	}
	

	// Update is called once per frame
	void FixedUpdate () {
		if (rigid.velocity.x < maxSpeed && rigid.velocity.x > -maxSpeed) {
			if(Input.GetKey(KeyCode.A))
				rigid.velocity += Vector2.right * -speed;

			if(Input.GetKey(KeyCode.D))
				rigid.velocity += Vector2.right * speed;

			if (Input.GetKey (KeyCode.S))
				maxSpeed = 2.5f;
			else
				maxSpeed = 5;

			if(rigid.IsTouching(box)){
				if(Input.GetKeyDown(KeyCode.W))
					rigid.AddForce(Vector2.up * jumpForce);
			}
		}
	}
}
