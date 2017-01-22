using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastWave : MonoBehaviour 
{
	[HideInInspector]
	public Transform Limit;

	[HideInInspector]
	public float maxSize;
	public float GrowSpeed = 0.1f;

	void Start()
	{
		transform.localScale = Vector3.zero;
	}

	void Update()	
	{
		if(Limit == null)
			return;

		float scaleGrowth = GrowSpeed * Time.deltaTime;	
        Vector3 localScale = transform.localScale;
		
//		if(transform.localScale.x <= maxSize)
//			transform.localScale = new Vector3(localScale.x + scaleGrowth, localScale.y + scaleGrowth, localScale.z + scaleGrowth);

		//print("local scale: " + transform.localScale);

		transform.localScale = new Vector3(localScale.x + scaleGrowth, localScale.y + scaleGrowth, localScale.z + scaleGrowth);

		if(transform.localScale.x >= Limit.localScale.x)
			Destroy(gameObject);
	}
}
