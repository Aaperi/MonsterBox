using UnityEngine;
using System.Collections;

public class Pistelasku : MonoBehaviour 
{
	public float dist;
	public GUIText scoreText;
	public Transform Alku;
	public float newDist=0;


	// Use this for initialization
	void Start () 
	{
		dist = Vector3.Distance (Alku.position, transform.position);
		scoreText.text = ("Score aka kuljettu matka alusta: " + (int)dist);
	
	}


	// Update is called once per frame
	void Update ()
	{
			if (dist > newDist) 
			{
				scoreText.text = ("Score aka kuljettu matka alusta: " + (int)dist);
				newDist = dist;
			}
		dist = Vector3.Distance (Alku.position, transform.position);
	}
}
