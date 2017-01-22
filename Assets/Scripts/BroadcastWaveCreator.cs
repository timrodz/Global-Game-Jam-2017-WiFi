using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastWaveCreator : MonoBehaviour 
{
	public float Delay  = 0.5f;
	public GameObject BroadcastWavePrefab;

	void Start () 
	{
		StartCoroutine(SpawnWave());	
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
