using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public Vector2 touchLocation;
	Transform grounded;
	SpriteRenderer sr;
	public float speed;
	public LayerMask pMask;
	Rigidbody2D rb;
	public bool crounch;
	public float hinput = 0;
	public bool isGrounded = false;
	public bool canClide;
	public bool isClide;
	public bool inWater = false;
	public bool canDash;
	public bool canJump;
	public bool isJump = false;

	public bool onVine;
	public bool inVine;
	float normalG = 2;
	public bool resetHinput = false; /*tilapäinen ratkaisu pelaajan liikkumiseen vaikuttavan 
										hinput (Horizontal Input) käyttäytymiseen*/

						/*Scripti on tehty kosketusnäytön ohjausta ajatellen*/

	void Start () {
		canDash = true;
		isClide = false;
		crounch = false;
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		grounded = GameObject.Find(this.name +"/IsGrounded").transform;
	}


	void FixedUpdate () {

		touchLocation = Camera.main.ScreenToViewportPoint(Input.mousePosition);/*Antaa näytön x ja y arvoille
																				 max arvon 1.*/
		
		isGrounded = Physics2D.Linecast(transform.position, grounded.position, pMask);/*Tätä olisi hyvä olla useampi tai
																					    muokata, jotta peli osaisi lukea
																					    tarkasti millon pelaaja on maassa*/
																		 
		Move(hinput);

//--------------------------------------LEFT SIDE INPUTS---------------------------------------
		if(touchLocation.x < 0.5f){ 
			if(Input.GetMouseButton(0)){ 
				if(canDash && !inWater){//-------------------------------------RUN
					Move(-speed);
					hinput = 0;
					resetHinput = false;
					
				}
				
				if(inWater){//-------------------------------------------------SWIM
					Action(0,-2);
				}
				if(Input.GetAxis("Mouse Y") > 1.2f){
					if(isGrounded && !crounch && !inWater)//-------------------JUMP
						Action(0,-3);
//					else if(canClide && !isGrounded)//---CLIDE
//						isClide = true;
					else if(isGrounded && crounch) //--------------------------STAND UP
						Action(2,2);
				}
				if (Input.GetAxis("Mouse X") < -1.2f){
					if(canDash)//----------------------------------------------DASH LEFT
						Action(1,-1);
					
				}
				if(Input.GetAxis("Mouse Y") < -1.2f){
						if(isGrounded && !crounch)//---------------------------CROUNCH
							Action(2,1);
					
				}
			}
		}
//---------------------------------------RIGHT SIDE INPUTS--------------------------------------
		if(touchLocation.x > 0.5f){ 
			if(Input.GetMouseButton(0)){ 
				if(canDash && !inWater){//-------------------------------------RUN
					Move(speed);
					hinput = 0;
					resetHinput = false;
				}
				
				if(inWater)//--------------------------------------------------SWIM
					Action(0,2);
				
				if(Input.GetAxis("Mouse Y") > 1.2f){
					if(isGrounded && !crounch && !inWater)//-------------------JUMP
						Action(0,3);
//					else if(canClide && !isGrounded)//---CLIDE
//						isClide = true;
					else if(isGrounded && crounch)//---------------------------STAND UP
						Action(2,2);
				}
				if (Input.GetAxis("Mouse X") > 1.2f && canDash)//--------------DASH RIGHT
					Action(1,1);
				
				if(Input.GetAxis("Mouse Y") < -1.2f){
					if(isGrounded && !crounch)//-------------------------------CROUNCH
						Action(2,1);
					
				}
			}
		}

			
	//---GLIDE--
//		if(Input.GetAxis("Mouse Y") < -1.2f && !isGrounded && isClide && Input.GetMouseButton(0))
//			isClide = false;
//		if (isClide)//----------
//			rb.gravityScale = 0.2f;




//--------------------------------------PLAYER STATES-----------------------------------

		if(crounch)//----------------------------------------------------------CROUNCHING-----
			speed = 1;
		
		else if(!crounch)//----------------------------------------------------STANDING----
			speed = 2;
		
		if(inWater){//---------------------------------------------------------IN WATER----
			rb.mass = 1;
			rb.drag = 1;
			rb.gravityScale = normalG;
			crounch = false;
		}
		if(inVine){//----------------------------------------------------------IN VINE----
			rb.gravityScale = 0;
			rb.drag = 10;
			rb.mass = 10;
		}
		else if(!inWater || !isClide || !inVine || !crounch){//----------------OUT WATER---
			rb.mass = 1;
			rb.gravityScale = normalG;
			rb.drag = 0;
		}
		if(isGrounded && !inWater){//------------------------------------------ON GROUND----
			canClide = false;
			isClide = false;
			canJump = true;
			if(resetHinput){
				hinput = 0;
				resetHinput = false;
			}
		}

//-----------------------------------ALTERNATIVE CONTROLS----------------------------------------
		if(Input.GetKey(KeyCode.LeftArrow) && canDash)
			Move(-speed);
		else if(Input.GetKey(KeyCode.RightArrow) && canDash)
			Move(speed);
		if(Input.GetKey(KeyCode.Keypad0))
			Action(2,0);
		if(Input.GetButton("Jump"))//-------------------------------STRAIGHT JUMP---------
			Action(0,0);
		if(Input.GetKey(KeyCode.UpArrow) && onVine){//--------------VINE UP----
			inVine = true;
			rb.velocity = Vector2.up * 2;
		}
		else if(Input.GetKey(KeyCode.DownArrow) && onVine){//-------VINE DOWN----
			inVine = true;
			rb.velocity = Vector2.down * 2;
		}
	}

//---------------------------MOVEMENT-------------------------------------------------
	void Move(float horizontalinput){

		Vector2 movement = rb.velocity;
		movement.x = horizontalinput * speed;
		rb.velocity = movement;
	}
	public void Startmove(float horizontalinput){

		hinput = horizontalinput;

	}

//----------------------------ACTIONS------------------------------------------------
	public void Action(int N, int act)
	{
		switch (N)
		{
		case 0: 
			StartCoroutine(Jump(act));
			break;
		case 1:
			StartCoroutine(Dash(act));
			break;
		case 2:
			StartCoroutine(Crounch(act));
			break;
		case 3:

			break;


		}
	}
//----------------------------COLLIDERS-------------------------------------------------
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Water")
		{
			inWater = true;
		}
		if(col.gameObject.tag == "Vine"){
			onVine = true;
		}


	}
	void OnTriggerExit2D(Collider2D exit){
		if(exit.gameObject.tag == "Water"){
			inWater = false;
		
		}
		if(exit.gameObject.tag == "Vine"){
			onVine = false;
			inVine = false;
		}
	}
	public void OnCollisionEnter2D(Collision2D obstacle){
		if(resetHinput){
			hinput = 0;
			resetHinput = false;
		}
	}
//----------------------IENUMERATORS FOR ACTIONS--------------------------------------
	IEnumerator Jump(int act)
	{		
		if(act == 3 && canJump || act == -3 && canJump){//-------------------------------NORMAL JUMP----
			canJump = false;
			inVine = false;
			rb.AddForce(new Vector2(act / 3, 1)*10, ForceMode2D.Impulse);
			hinput = act;
			yield return new WaitForSeconds(0.01f);
			resetHinput = true;
			yield return new WaitForSeconds(0.49f);
			canClide = true;
		}
		else if(act == 2 && canJump && inWater || act == -2 && canJump && inWater){//----SWIMMING----
			canJump = false;
			rb.AddForce(new Vector2(0, 1) *10, ForceMode2D.Impulse);
			hinput = act / 2;
			yield return new WaitForSeconds(0.2f);
			canJump = true;
			yield return new WaitForSeconds(0.2f);
			if(!canJump)
				StopCoroutine(Jump(act));
			else if(canJump)
				hinput = 0;
		}
		else if(act == 0 && canJump){//--------------------------------------------------STRAIGHT JUMP----
			canJump = false;
			inVine = false;
			rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
			yield return new WaitForSeconds(0.5f);
			canClide = true;
		}

	}
	IEnumerator Dash(int act)
	{
		hinput = act * 6;
		canDash = false;
		yield return new WaitForSeconds(0.5f);
		hinput = 0;
		yield return new WaitForSeconds(0.2f);
		canDash = true;


	}
	IEnumerator Crounch(int act)
	{
		if(act == 1){//----CROUNCH DOWN----
			crounch = true;
			yield return new WaitForSeconds(0.2f);
		}
		if(act == 2){//----STAND UP----
			yield return new WaitForSeconds(0.2f);
			crounch = false;
		}
	}

}