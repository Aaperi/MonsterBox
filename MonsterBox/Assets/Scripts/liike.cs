using UnityEngine;
using System.Collections;

public class liike : MonoBehaviour 
{
	public float speed = 10f;
	public int aika = 0;

	void Update () 
	{

		if (aika>=0
		    && aika<100)
		{

			transform.Translate(Vector2.right * speed * Time.deltaTime);
			aika++;
		}

		if (aika>=100
		    && aika<200)
		{
			transform.Translate(Vector2.left * speed * Time.deltaTime);
			aika++;
		}

		if (aika>=200
		    || aika<0)
		{
			aika=0;
		}
	}
}