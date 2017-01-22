using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeQuadFromSphere : MonoBehaviour 
{
	public Transform QuadTransform; 

	public float Factor = 1;

	void Update () 
	{
		QuadTransform.localScale = transform.localScale * Factor;
	}
}
