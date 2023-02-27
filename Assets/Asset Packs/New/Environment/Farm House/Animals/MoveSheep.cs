using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSheep : MonoBehaviour {

	float time = 0;
	void Update ()
	{
		Animator animator = gameObject.GetComponent<Animator>();
		time = time + Time.deltaTime;
		if(time > 10 && time < 12.5f)
		{
			animator.SetBool("Idle", false);
			gameObject.GetComponent<Animator>().enabled = true;
			transform.position += transform.forward * Time.deltaTime * 2;
			transform.Rotate(Vector3.up * Time.deltaTime * 75);
		}
		if(time > 12.5f)
		{
			time = 0;
			animator.SetBool("Idle", true);
		}
		
	}
}
