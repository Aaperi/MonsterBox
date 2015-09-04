using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	public Canvas gameOverMenu;
	public Button newGameText;
	public Button exitGameText;

	// Use this for initialization
	void Start () {
	
		gameOverMenu = gameOverMenu.GetComponent<Canvas> ();
		newGameText = newGameText.GetComponent<Button> ();
		exitGameText = exitGameText.GetComponent<Button> ();


	}

	public void NewGamePressed(){
		Application.LoadLevel (0);
	}

	public void ExitGamePressed(){
		Application.Quit ();
	}
	

}
