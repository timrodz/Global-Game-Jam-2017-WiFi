using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBetweenPoints : MonoBehaviour 
{
	public Transform StartPoint;
	public Transform EndPoint;

	private Vector3 _prevStartPoint;
	private Vector3 _prevEndPoint;

	void Update()
	{
		if((_prevStartPoint != StartPoint.position) || (_prevEndPoint != EndPoint.position))
		{
			float length = Vector3.Distance(EndPoint.position, StartPoint.position);

			Vector3 center = (StartPoint.position + EndPoint.position)/2;
			float sign = Mathf.Sign(EndPoint.position.x - StartPoint.position.x);
			
			transform.position = center;
			Vector3 scale = transform.localScale;
			transform.localScale = new Vector3(scale.x, scale.y, length /* * sign*/);
			transform.rotation = Quaternion.LookRotation(EndPoint.position - StartPoint.position);
		}

		_prevStartPoint = StartPoint.position;
		_prevEndPoint = StartPoint.position;
	}
}
