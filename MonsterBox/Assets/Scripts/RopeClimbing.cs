using UnityEngine;
using System.Collections;

public class RopeClimbing : MonoBehaviour 
{
	private RobotController thePlayer;

	void Start () 
	{
		thePlayer = FindObjectOfType<RobotController>();
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.name == "pelaaja")
		{
			thePlayer.onRope = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if(other.name == "pelaaja")
		{
			thePlayer.onRope = false;
		}
	}


}
