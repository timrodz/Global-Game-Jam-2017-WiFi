using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastWaveCreator : MonoBehaviour 
{
	public float Delay  = 0.5f;
	public GameObject BroadcastWavePrefab;

	[HideInInspector]
	public bool CanGrow = true;

	void Start () 
	{
		StartCoroutine(SpawnWave());	
	}

	void OnTriggerEnter(Collider other) 
	{
			if (other.CompareTag("Signal Disruptor")) {
				CanGrow = false;
				Vector3 scale = transform.localScale;
				transform.localScale = new Vector3(scale.x + 0.1f, scale.y + 0.1f, scale.z + 0.1f);
				//GetComponent<CapsuleCollider>().radius -= 0.65f;
				print("broacaster signal disruptor");
			}
		else if(other.CompareTag("House"))
		{
			print("House by BroadcastWave");
			other.GetComponent<House>().HandleGotSignal();
		}
	}
	
	public IEnumerator SpawnWave()
	{
		while(true)
		{
			yield return new WaitForSeconds(Delay);
			GameObject go = (GameObject)GameObject.Instantiate(BroadcastWavePrefab, transform.position, Quaternion.identity);
			go.GetComponent<BroadCastWave>().Limit = transform;
		}
	}
}
