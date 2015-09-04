using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public Canvas gameOverScreen;
	public GameObject whatToDestroy;
	
	void Start () {


		gameOverScreen.enabled = false;

	}

	void OnTriggerEnter2D(Collider2D trap){
		
		if (trap.gameObject.tag == "Player") {
			Destroy(whatToDestroy);
			gameOverScreen.enabled = true;

		}

	}


}
