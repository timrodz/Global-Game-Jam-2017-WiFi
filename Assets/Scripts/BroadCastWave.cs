using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastWave : MonoBehaviour 
{
	public Transform Limit;
	public float maxSize;
	public float GrowSpeed = 0.1f;

	void Update()	
	{
		float scaleGrowth = GrowSpeed * Time.deltaTime;	
        Vector3 localScale = transform.localScale;
		
		if(transform.localScale.x <= maxSize)
			transform.localScale = new Vector3(localScale.x + scaleGrowth, localScale.y + scaleGrowth, localScale.z + scaleGrowth);

		//print("local scale: " + transform.localScale);

		// if(transform.localScale.x >= Limit.localScale.x)
		
			// Destroy(gameObject);
	}
}
