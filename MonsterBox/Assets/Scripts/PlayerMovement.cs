using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	private GameObject star1;
	private GameObject star2;
	private GameObject star3;

	private GameObject buttonNext;
	
	protected string currentLevel;
	protected int worldIndex;
	protected int levelIndex;
	bool isLevelComplete ;
	public Text timerText;
	protected float totalTime = 0f;

	void Start () {
		isLevelComplete = false;
		star1 = GameObject.Find("star1");
		star2 = GameObject.Find("star2");
		star3 = GameObject.Find("star3");
		buttonNext = GameObject.Find("Next");
		star1.GetComponent<Image>().enabled = false;
		star2.GetComponent<Image>().enabled = false;
		star3.GetComponent<Image>().enabled = false;
		buttonNext.SetActive(false);
		currentLevel = Application.loadedLevelName;
	}

	void Update () {
		if(!isLevelComplete){
			totalTime += Time.deltaTime;
			timerText.text = "TIME: "+totalTime.ToString();
			Debug.Log(totalTime);
			transform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime*10f, 0, 0); //get input
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name=="Goal"){
			isLevelComplete = true;
			if(totalTime<5){
				star3.GetComponent<Image>().enabled = true;
				UnlockLevels(3);   //unlock next level funxtion 
			}
			else if(totalTime<10){
				star2.GetComponent<Image>().enabled = true;
				UnlockLevels(2);   //unlock next level funxtion 
			}
			else if(totalTime<15){
				star1.GetComponent<Image>().enabled = true;
				UnlockLevels(1);   //unlock next level funxtion 
			}
			buttonNext.SetActive(true);
			
		}
	}
	
	public void OnClickButton(){
		Application.LoadLevel("World1");
		
	}
	
	protected void  UnlockLevels (int stars){

		for(int i = 0; i < LockLevel.worlds; i++){
			for(int j = 1; j < LockLevel.levels; j++){               
				if(currentLevel == "Level"+(i+1).ToString() +"." +j.ToString()){
					worldIndex  = (i+1);
					levelIndex  = (j+1);
					PlayerPrefs.SetInt("level"+worldIndex.ToString() +":" +levelIndex.ToString(),1);
					if(PlayerPrefs.GetInt("level"+worldIndex.ToString() +":" +j.ToString()+"stars")< stars)
						PlayerPrefs.SetInt("level"+worldIndex.ToString() +":" +j.ToString()+"stars",stars);
				}
			}
		}
		
	}
	
	
}