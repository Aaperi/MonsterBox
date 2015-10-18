using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Button playButton;
    public Button optionsButton;
    public Button storeButton;
    public Button creditsButton;
    public Button quitButton;

    public Canvas mainMenu;
    public Canvas optionsMenu;
    public Canvas creditsMenu;

    void Start () {

        mainMenu = mainMenu.GetComponent<Canvas>();

        playButton = playButton.GetComponent<Button>();
        optionsButton = playButton.GetComponent<Button>();
        storeButton = playButton.GetComponent<Button>();
        creditsButton = playButton.GetComponent<Button>();
        quitButton = playButton.GetComponent<Button>();

        optionsMenu.enabled = false;
        creditsMenu.enabled = false;



    }

    public void BackPressed()
    {
        if (optionsMenu)
        {
            optionsMenu.enabled = false;
        }
        if (creditsMenu)
        {
            creditsMenu.enabled = false;
        }


    }

    public void playPress(){
        Application.LoadLevel("World1");
    }

    public void optionsPress(){
        optionsMenu.enabled = true;
    }

    public void storePress(){
        Application.LoadLevel("Store");
    }

    public void creditsPress(){
        creditsMenu.enabled = true;
    }

    public void quitPress(){
        Application.Quit();
    }

}
