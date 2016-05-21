using UnityEngine;
using System.Collections;

public class WWWFormTest : MonoBehaviour {

	// this is basically a reference how I have created everything.

	//Things that will need changing in the future:
	// -URLs in this script (a specific folder in the "true" database, perhaps?)
	// -server/Database/username/password in all of the php scripts

	// in the future I'll probably make php scripts give unity script the values "tableVariables"
	// and "stagesPerWorlds". But for now this'll have to do.

	// The most interesting function here is most likely: Ienumerator StageData_read(int ID)


	// remember to use "Replace("<br />","\r\n")" to fix the texts.
	// unlocked is either 0 or 1, in both instances it's used. (Basically a bool, but stored as INT)
	// Create_mastertable.php should never be called from unity

	private string url 	 = "http://localhost/scripts/Create_table.php";

	private string url2  = "http://localhost/scripts/StageData_Update.php";
	private string url3  = "http://localhost/scripts/StageData_Read.php";
	private string url4  = "http://localhost/scripts/StageData_Update_UnlockStage.php";

	private string url11 = "http://localhost/scripts/UnlockData_Read.php";
	private string url12 = "http://localhost/scripts/UnlockData_Update.php";

	private string url21 = "http://localhost/scripts/UnlockData_AddItem.php";
	private string url22 = "http://localhost/scripts/StageData_AddStage.php";
	private string url23 = "http://localhost/scripts/StageData_AddWorld.php";

	// "read" -script values for correct execution
	private int tableVariables = 6; // this should be the number of variables stored for each stage (in the database)
	private int[] stagesPerWorlds = new int[]{5,5,6};
	private int currentWorld;
	private int world_storage;
	private int currentStage;
	private int stage_storage;

	void Start () {
		StartCoroutine(ReReWrite(5f, 1)); // just a periodical test to change numbers
	}

	void Update () { // just for testing purposes, simple on-command Ienumerator execution
		if (Input.GetKeyDown (KeyCode.T)) {
			Debug.Log ("dafug?");
			StartCoroutine(StageData_Read(1));
			//StartCoroutine (StageData_ReadLite ());
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			StartCoroutine(Start_UnlockStage(1,Random.Range(1,4), Random.Range(1,6)));
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			StartCoroutine (CreateTable());
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			StartCoroutine(UnlockData_Update(1, "unlockitem2"));
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			StartCoroutine (UnlockData_Read (1));
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			StartCoroutine (UnlockData_AddItem ("gagagogo")); // just a test name
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			StartCoroutine (StageData_AddStage (2));
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			StartCoroutine (StageData_AddWorld (4));
		}
	}


	// -------------- TESTING -------------------------------------------------------------------------

	IEnumerator ReReWrite (float time, int ID) // once started, periodically injects random numbers to given ID's StageData -table
	{
		StartCoroutine(StageData_Update (ID, Random.Range (1, 4),Random.Range(1,6), Random.Range (1, 4), Random.Range (0.999f, 9001.999f), Random.Range (0.123f, 360.123f), Random.Range(0,3), Random.Range(0,3)));
		yield return new WaitForSeconds (time);
		StartCoroutine (ReReWrite (time, ID));
	}

	// ------------------------ STAGE DATA ---------------------------------------------------------------

	IEnumerator StageData_Read(int ID) // needs to know which player's table it reads
	{
		WWWForm formRead = new WWWForm (); // initializing form
		formRead.AddField ("ID", ID); // adding a field (required by php side) to the form
		Debug.Log("yeah?");
		WWW postRead = new WWW (url3, formRead); // creating a WWW request
		yield return postRead; // waiting till it's done
		string[] echoText = new string[]{}; // initializing string array
		echoText = postRead.text.Replace("<br />","\r\n").Split (new string[] {":"}, System.StringSplitOptions.None ); // tidying the echo from php up and splitting it into an array
		if (echoText [0] != "ERROR") { // making sure that the PHP side worked as expected
			// use the following as a reference how to extract data from "echoText"
			// Debug.Log ("Split Length= "+ echoText.Length); // if you want to check that it's "21" instead of 20 because of a for loop used in the php side

			// the following is rather complicated way to account the differences between stage amounts between worlds (in addition to variable amount of worlds)
			for (int i = 0; i < (echoText.Length -1); i += tableVariables) {
				currentWorld = 1;
				world_storage = 0;
				currentStage = 0;
				stage_storage = 0;
				for (int i2 = 0; i2 < (stagesPerWorlds.Length-1); i2 ++) {
					Debug.Log ("tableVariables=" + tableVariables + " stagesper=" + stagesPerWorlds [i2] + " world_storage=" + world_storage);
					int addition = Mathf.FloorToInt (i / ((tableVariables * stagesPerWorlds [i2]) + world_storage));
					if (addition >= 1) {
						addition = 1;
						currentWorld += addition;
						stage_storage += tableVariables * stagesPerWorlds [i2];
					}
					world_storage += tableVariables * stagesPerWorlds [i2];
				}
				currentStage = (i - stage_storage) / tableVariables + 1;

				Debug.Log ("World " + currentWorld + "-" + currentStage + " Unlocked=" + int.Parse (echoText [i]));
				Debug.Log ("World " + currentWorld + "-" + currentStage + " Stars=" + int.Parse (echoText [i + 1]));
				Debug.Log ("World " + currentWorld + "-" + currentStage + " Highscore=" + float.Parse (echoText [i + 2]));
				Debug.Log ("World " + currentWorld + "-" + currentStage + " BestTime=" + float.Parse (echoText [i + 3]));
				Debug.Log ("World " + currentWorld + "-" + currentStage + " Chests=" + int.Parse (echoText [i + 4]));
				Debug.Log ("World " + currentWorld + "-" + currentStage + " Shards=" + int.Parse (echoText [i + 5]));
			}
		}
	}


	IEnumerator Start_UnlockStage (int ID, int world, int stage)
	{
		WWWForm formUnlock = new WWWForm ();
		formUnlock.AddField ("ID", ID);
		formUnlock.AddField ("world", world);
		formUnlock.AddField ("stage", stage);
		WWW postUnlock = new WWW (url4, formUnlock);
		yield return postUnlock;
		Debug.Log (postUnlock.text.Replace("<br />","\r\n"));
	}


	IEnumerator StageData_Update (int ID, int world, int stage, int stars, float score, float time, int chests, int shards)
	{
		WWWForm form2 = new WWWForm ();
		//Debug.Log (score + "+" + time);
		string scoreAsString = score.ToString("#.000");
		string timeAsStrinng = time.ToString("#.000");
		//Debug.Log (scoreAsString + "+" + timeAsStrinng);
		form2.AddField ("ID", ID);
		form2.AddField ("world", world);
		form2.AddField ("stage", stage);
		form2.AddField ("stars", stars);
		form2.AddField ("score", scoreAsString);
		form2.AddField ("time", timeAsStrinng);
		form2.AddField ("chests", chests);
		form2.AddField ("shards", shards);
		WWW post2 = new WWW (url2, form2);
		yield return post2;
		if(!string.IsNullOrEmpty(post2.error)) {
			print( "Error downloading: " + post2.error );
		} else {
			Debug.Log(post2.text.Replace("<br />","\r\n"));
		}
	}

	IEnumerator StageData_AddStage (int world) // needs to know which world gets new stage (creates a single stage)
	{
		WWWForm form22 = new WWWForm ();
		form22.AddField ("world", world);
		WWW post22 = new WWW (url22, form22);
		yield return post22;
		if(!string.IsNullOrEmpty(post22.error)) {
			print( "Error downloading: " + post22.error );
		} else {
			Debug.Log(post22.text.Replace("<br />","\r\n"));
		}
	}

	IEnumerator StageData_AddWorld (int stages) // needs to know how many stages are added to the newly created world
	{
		WWWForm form23 = new WWWForm ();
		form23.AddField ("stageCount", stages);
		WWW post23 = new WWW (url23, form23);
		yield return post23;
		if(!string.IsNullOrEmpty(post23.error)) {
			print( "Error downloading: " + post23.error );
		} else {
			Debug.Log(post23.text.Replace("<br />","\r\n"));
		}
	}



	// -------------------------------- TABLE CREATION ----------------------------------------------------------

	IEnumerator CreateTable () // creates next ID in the list, refer to the php scripts on how this works
	{
		WWW post = new WWW (url); // make a www request
		yield return post; // wait till it's finished
		if(!string.IsNullOrEmpty(post.error)) {
			print( "Error downloading: " + post.error );
		} else {
			Debug.Log(post.text.Replace("<br />","\r\n"));
		}
	}


	// ----------------------------------------- UNLOCK DATA -------------------------------------------------------------------------

	IEnumerator UnlockData_Update (int ID, string itemName) // to unlock a specified item
	{
		WWWForm form12 = new WWWForm ();
		form12.AddField ("ID", ID);
		form12.AddField ("item", itemName);
		WWW post12 = new WWW (url12, form12);
		yield return post12;
		if(!string.IsNullOrEmpty(post12.error)) {
			print( "Error downloading: " + post12.error );
		} else {
			Debug.Log(post12.text.Replace("<br />","\r\n"));
		}
	}

	IEnumerator UnlockData_Read (int ID)
	{
		WWWForm form11 = new WWWForm ();
		form11.AddField ("ID", ID);
		WWW post11 = new WWW (url11, form11);
		yield return post11;
		string[] echoTextUdataRead = new string[]{};
		echoTextUdataRead = post11.text.Replace("<br />","\r\n").Split (new string[] {":"}, System.StringSplitOptions.None );
		if (echoTextUdataRead [0] != "ERROR") {
			// use the following as a reference how to extract data from "echoTextUdataRead"
			for (int i = 0; i < (echoTextUdataRead.Length -1); i += 1) { // This is because the split creates too long array, and that array's last member is empty and that causes problems

				// splitting the text, refer to the php if you want to know what kind of output it gives.
				// the "itemStringArray" is meant to hold all the keys you need to use this data effectively. [0] = name, [1]= unlocked status (0 or 1).
				string[] itemStringArray = echoTextUdataRead [i].Split(new string[] {"="}, System.StringSplitOptions.None);
				if (int.Parse (itemStringArray[1]) == 1) {
					Debug.Log (itemStringArray[0] + " is unlocked!");
				}
				else
				{
					Debug.Log (itemStringArray[0] + " not unlocked");
				}
			}
		}
	}

	IEnumerator UnlockData_AddItem (string itemName)
	{
		WWWForm form21 = new WWWForm ();
		form21.AddField ("item", itemName);
		WWW post21 = new WWW (url21, form21);
		yield return post21;
		if(!string.IsNullOrEmpty(post21.error)) {
			print( "Error downloading: " + post21.error );
		} else {
			Debug.Log(post21.text.Replace("<br />","\r\n"));
		}
	}

}
