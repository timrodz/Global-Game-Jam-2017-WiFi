using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	private bool _gotSignal = false;

	public void HandleGotSignal()
	{
		if(_gotSignal == false)
		{
			print("got signal");
			_gotSignal = true;
			GameManager.Instance.HandleNewHouseGotSignal();
		}
	}	
}
