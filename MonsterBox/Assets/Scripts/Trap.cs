using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public Canvas gameOverScreen;
	public GameObject Player;
	
	void Start () {


		gameOverScreen.enabled = false;

	}

	void OnTriggerEnter2D(Collider2D trap){
		
		if (trap.gameObject.tag == "Player") {
			Destroy(Player);
			gameOverScreen.enabled = true;

		}

	}


}
