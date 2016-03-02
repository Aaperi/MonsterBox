using UnityEngine;
using System.Collections;

public class cameramovement : MonoBehaviour {
    public Vector3 offsets;
    public GameObject player;
    private Transform playertrans;
	// Use this for initialization
	void Start () {
        playertrans = player.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(playertrans.position.x + offsets.x, playertrans.position.y + offsets.y, -10);
	}
}
