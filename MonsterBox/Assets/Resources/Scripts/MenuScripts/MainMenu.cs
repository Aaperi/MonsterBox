using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public const int MaxStoreItemsVisible = 3;

    public string level1name = "World1-1";

    public Canvas storeCanvas;

    public Canvas confirmBoxCanvas;

    private Text confirmBoxText;
    private Button confirmBoxButtonConfirm;
    private Button confirmBoxButtonCancel;

    private GameObject confirmBoxTarget;
    private string confirmBoxMessage;

    private Transform storeContent;

    void Start () {
        storeContent = storeCanvas.transform.Find("Panel/Panel/Panel/Content");
        StoreSlider(0);

        confirmBoxText = confirmBoxCanvas.transform.Find("Panel/Text").GetComponent<Text>();
        confirmBoxButtonConfirm = confirmBoxCanvas.transform.Find("Panel/Buttons/Confirm Button").GetComponent<Button>();
        confirmBoxButtonCancel = confirmBoxCanvas.transform.Find("Panel/Buttons/Cancel Button").GetComponent<Button>();

        confirmBoxCanvas.enabled = false;
    }

    public void StoryButton () {
        SceneManager.LoadScene(level1name, LoadSceneMode.Single);
    }

    public void ButtonQuit (){
        ConfirmBox("Quit game?", "Quit", "Back", gameObject, "Quit");
    }

    public void Quit () {
        Debug.Log("Quit called");
        Application.Quit();
    }

    public void ButtonConfirm () {
        // If we have target
        if ( confirmBoxTarget ) {
            // If we have message, send message
            if ( confirmBoxMessage.Length > 0 )
                confirmBoxTarget.SendMessage(confirmBoxMessage);
        }

        // Clear ConfirmBox info
        confirmBoxCanvas.enabled = false;
        confirmBoxTarget = null;
        confirmBoxMessage = "";
    }

    public void ButtonCancel () {
        // Clear ConfirmBox info
        confirmBoxCanvas.enabled = false;
        confirmBoxTarget = null;
        confirmBoxMessage = "";
    }

    public void StoreSlider (float value) {
        // Hide all store content
        foreach ( Transform t in storeContent )
            t.gameObject.SetActive(false);

        int index = (int)value;

        // Show store content based on index
        if ( index + MaxStoreItemsVisible < storeContent.childCount ) {
            for ( int i = index; i < index+MaxStoreItemsVisible; i++ ) {
                storeContent.GetChild(i).gameObject.SetActive(true);
            }
        } else {
            for ( int i = storeContent.childCount-MaxStoreItemsVisible; i < storeContent.childCount; i++ ) {
                storeContent.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    void ConfirmBox (string text, string confirmText, string cancelText, GameObject target, string message ) {
        // Dont do anything if we dont have text to show
        if ( text.Length < 1 ) {
            // Hide ConfirmBox Canvas (Just in case it's visible for some reason)
            confirmBoxCanvas.enabled = false;
            return;
        }

        confirmBoxText.text = text;
        confirmBoxButtonConfirm.transform.Find("Text").GetComponent<Text>().text = confirmText;
        confirmBoxButtonCancel.transform.Find("Text").GetComponent<Text>().text = cancelText;
        confirmBoxCanvas.enabled = true;
        
        confirmBoxTarget = target;
        confirmBoxMessage = message;
    }
}
