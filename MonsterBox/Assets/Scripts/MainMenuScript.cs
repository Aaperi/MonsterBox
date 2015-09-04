using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public Canvas MainMenu;
	public Canvas OptionsMenu;
	public Canvas StoreMenu;
	public Canvas CreditsMenu;
	public Button newGameButton;
	public Button OptionsButton;
	public Button exitGameButton;

	
	// Use this for initialization
	void Start () {
		
		MainMenu = MainMenu.GetComponent<Canvas> ();
		newGameButton = newGameButton.GetComponent<Button> ();
		OptionsButton = OptionsButton.GetComponent<Button> ();
		exitGameButton = exitGameButton.GetComponent<Button> ();
		OptionsMenu.enabled = false;
		StoreMenu.enabled = false;
		CreditsMenu.enabled = false;
		
	}

	public void OptionsPressed(){
		OptionsMenu.enabled = true;
	}

	public void StorePressed(){
		StoreMenu.enabled = true;
	}

	public void CreditsPressed(){
		CreditsMenu.enabled = true;
	}

	public void BackPressed(){
		if (OptionsMenu) {
			OptionsMenu.enabled = false;
		}
		if (StoreMenu) {
			StoreMenu.enabled = false;
		}
		if (CreditsMenu) {
			CreditsMenu.enabled = false;
		}
	}

	public void NewGamePressed(){
		Application.LoadLevel (0);
	}
	
	public void ExitGamePressed(){
		Application.Quit ();
	}
}
