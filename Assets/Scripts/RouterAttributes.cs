using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterAttributes : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	public enum RouterType {
		Broadcaster,
		Expander,
		Socket
	};
	
	public RouterType routerType;
	
	private RouterPlacementScript rs;
	
	// Line renderer
	LineRenderer lineRenderer;
	public Color c1 = Color.green;
	public Color c2 = Color.blue;
	private float rateOfGradientChange;
	
	private bool hasFoundPortal = false;
	
	void Awake() {
		
		rs = GetComponent<RouterPlacementScript>();
		lineRenderer = GetComponent<LineRenderer>();
		
	}

	// Use this for initialization
	void Start () {
		
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.widthMultiplier = 0.1f;
		lineRenderer.numPositions = 0;
		
		// A simple 2 color gradient with a fixed alpha of 1.0f.
		float alpha = 1.0f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 0.45f), new GradientColorKey(c1, 0.75f), new GradientColorKey(c2, 0.1f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
		);
		
		
		switch (routerType) {
			case RouterType.Broadcaster:
			{
				Transform t = Instantiate(rs.GetComponentInParent<RouterManagerScript>().broadcastWave, transform.position, Quaternion.identity);
				t.parent = this.transform;
				t.GetComponent<BroadCastWave>().maxSize = 3.5f;
				CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
				cc.radius = 0.3f;
				cc.isTrigger = true;
			}
			break;
			case RouterType.Expander:
			{
				gradient.SetKeys(
					new GradientColorKey[] { new GradientColorKey(c1, 0.0f) },
					new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f) }
				);
				lineRenderer.colorGradient = gradient;
			}
			break;
			case RouterType.Socket:
			{
				CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
				cc.isTrigger = true;
			}
			break;
			default:
			{
				
			}
			break;
		}
		
		lineRenderer.colorGradient = gradient;
		lineRenderer.useWorldSpace = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		AnimateWaves();
		
		if (rs.hasBeenPlaced && rs.hasBeenActivated && rs.canGrow) {
			
			Grow();
		
		}
		
		if (rs.hasBeenPlaced && !rs.hasBeenActivated) {
			
			switch (routerType) {
				case RouterType.Broadcaster:
				break;
				case RouterType.Expander:
				CreateLine(2, -1.5f);
				break;
				default:
				break;
			
			}
			
		}
		
	}
	
	void Grow() {
		
		switch (routerType) {
			case RouterType.Broadcaster:
			{
				
				CapsuleCollider cc = GetComponent<CapsuleCollider>();
				if (cc.radius < 3.5f) {
					int segments = 50;
					CreateEllipse(segments, cc.radius);
					cc.radius += 0.1f;
				}
				else {
					cc.radius -= 0.6f;
					rs.canGrow = false;
				}
				
			}
			break;
			case RouterType.Expander:
			{
				
				RaycastHit hit;
				int typeOfWall = DetermineMaxLineLength(0, out hit);
				else {
					
					if (typeOfWall == 1) {
						
						if (Mathf.Abs(lineRenderer.GetPosition(1).y) < hit.distance) {
							CreateLine(2, lineRenderer.GetPosition(1).y - 0.1f);
						}
						
					}
					
				}	
				
			}
			break;
			case RouterType.Socket:
			{
				AnimateWaves();
				CapsuleCollider cc = GetComponent<CapsuleCollider>();
				if (cc.radius < 2.5f) {
					int segments = 50;
					CreateEllipse(segments, cc.radius);
					cc.radius += 0.1f;
				}
				else {
					cc.radius -= 0.6f;
				}
			}
			break;
			default:
			{
				
			}
			break;
		}
		
	}
	
	void AnimateWaves() {
		
		rateOfGradientChange += 0.5f;
		
		if (routerType != RouterAttributes.RouterType.Expander) {
			transform.Rotate(Vector3.forward * (Time.deltaTime + rateOfGradientChange));
		}
		else {
			
		}
		
	}
	
	void CreateEllipse(int _segments, float _radius) {
		
		lineRenderer.numPositions = _segments + 1;
				
		float x;
		float y;
		float z = 0f;
	   
		float angle = 20f;
	   
		for (int i = 0; i < (_segments + 1); i++) {
			
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * _radius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * _radius;
			
			lineRenderer.SetPosition (i, new Vector3(x, y, z) );
				   
			angle += (360f / _segments);
			
		}
		
	}
	
	void CheckWaveCollision() {
		
		// RaycastHit hit;
		// if (Physics.SphereCast(lineRenderer.GetPosition))
		
	}
	
	void CreateLine(int _positions, float _length) {
		
		lineRenderer.numPositions = _positions;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.up * _length);
		
	}
	
	int DetermineMaxLineLength(int _startPos, out RaycastHit hit) {
		
		// RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.up * lineRenderer.GetPosition(1).y, out hit, 100, collisionMask)) {
			
			if (hit.transform.CompareTag("Signal Disruptor")) {
			
				return 1;
				
			}
			else {
				return 0;
			}
			
		}
		
		return -1;
		
	}
	
}