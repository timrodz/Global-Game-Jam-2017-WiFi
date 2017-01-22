using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpanderWave : MonoBehaviour 
{
	void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("House"))
		{
			print("House by ExpanderWave");
			other.GetComponent<House>().HandleGotSignal();
		}		
	}
}
