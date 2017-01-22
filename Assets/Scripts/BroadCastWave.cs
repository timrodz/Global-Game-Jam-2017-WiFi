using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastWave : MonoBehaviour 
{
	[HideInInspector]
	public Transform Limit;

	public float GrowSpeed = 0.1f;

	void Start() {
		transform.localScale = Vector3.zero;
	}

	void Update()	
	{
		if(Limit == null)
			return;
		
		if(transform.localScale.x >= Limit.localScale.x) {
			Invoke("FadeOutAndDestroy", 1.25f);
		}
		else {
			float scaleGrowth = GrowSpeed * Time.deltaTime;	
			Vector3 localScale = transform.localScale;
			transform.localScale = new Vector3(localScale.x + scaleGrowth, localScale.y + scaleGrowth, localScale.z + scaleGrowth);
		}
			
	}
	
	void FadeOutAndDestroy() {
		
		Destroy(gameObject);
		
	}
}
