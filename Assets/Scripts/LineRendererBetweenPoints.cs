using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererBetweenPoints : MonoBehaviour 
{
	// Creates a line renderer that follows a Sin() function
	// and animates it.
	public float Amplitude = 0.01f;
	public float Frequency = 5;

	public Transform StartPoint;
	public Transform EndPoint;

	public float SegmentLenght;

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	//public int LengthOfLineRenderer = 20;

	private LineRenderer _lineRenderer;

	private Vector3 _prevStartPoint;
	private Vector3 _prevEndPoint;

	private List<Vector3> _positions; 

	private Vector3 _perpendicular; 

	void Awake()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	void Start() 
	{
		_lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		_lineRenderer.widthMultiplier = 0.2f;
		//_lineRenderer.numPositions = 2;

		// A simple 2 color gradient with a fixed alpha of 1.0f.
		float alpha = 1.0f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
			);
		_lineRenderer.colorGradient = gradient;
	}

	void Update()
	{
		if((_prevStartPoint != StartPoint.position) || (_prevEndPoint != EndPoint.position))
		{
			Vector3 endToStart = EndPoint.position - StartPoint.position;
			float distance = endToStart.magnitude;
	
			Vector3 dir = endToStart.normalized;
	
			int pointsAmount = Mathf.FloorToInt(distance / SegmentLenght);
	
			_lineRenderer.numPositions = pointsAmount + 2;
	
			Vector3 pos = StartPoint.position;
	
			_positions = new List<Vector3>(_lineRenderer.numPositions);
			_positions.Add(pos);
	
			for(int i = 1; i <= pointsAmount; i++)
			{
				pos += dir * SegmentLenght;
				_positions.Add(pos);
			} 
	
			_positions.Add(EndPoint.position);
	
			_perpendicular = new Vector3(dir.y, -dir.x, dir.z);
		}

		var t = Time.time;
		for(int i = 0; i < _lineRenderer.numPositions; i++) 
		{
			Vector3 finalPos = _positions[i];
			if((i != 0) && (i != (_lineRenderer.numPositions - 1)))
				finalPos += _perpendicular * Amplitude * Mathf.Sin(i + Frequency * t);				

			_lineRenderer.SetPosition(i, finalPos);
		}

		_prevStartPoint = StartPoint.position;
		_prevEndPoint = StartPoint.position;
	}
}
