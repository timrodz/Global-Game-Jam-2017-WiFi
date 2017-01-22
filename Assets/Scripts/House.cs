using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	private bool _gotSignal = false;
	
	[SerializeField] private GameObject _alien;

	public void HandleGotSignal()
	{
		if(_gotSignal == false)
		{
			print("got signal");
			_gotSignal = true;
			GameManager.Instance.HandleNewHouseGotSignal();

			_alien.GetComponent<Animator>().SetTrigger("GotWifi");
		}
	}	
}
