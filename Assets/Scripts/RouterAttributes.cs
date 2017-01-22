using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterAttributes : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	public enum RouterType {
		Broadcaster,
		Expander,
		Socket_Start,
		Socket_End
	};
	
	public RouterType routerType;

	public float BroadcastGrowSpeed = 0.1f;
	[HideInInspector]
	public RouterHandlerScript _routerHandler;
	
	// Line renderer
	LineRenderer lineRenderer;
	public Color c1 = Color.green;
	public Color c2 = Color.blue;
	private float rateOfGradientChange;
	
	//private bool hasFoundPortal = false;
	
	void Awake() {
		
		_routerHandler = GetComponent<RouterHandlerScript>();
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
		
		lineRenderer.colorGradient = gradient;
		lineRenderer.useWorldSpace = false;
		
		switch (routerType) {
			case RouterType.Broadcaster:
			{
				
				// GameObject broadcastCreatorGO = (GameObject)Instantiate(rs.BroadcastWaveCreatorPrefab, transform.position, Quaternion.identity);
//				rs.EffectGO = broadcastCreatorGO;
//				broadcastCreatorGO.GetComponent<BroadcastWaveCreator>();
//
//				//CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
//				CapsuleCollider cc = broadcastCreatorGO.AddComponent<CapsuleCollider>();
//				cc.radius = 0.3f;
//				cc.isTrigger = true;
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
			case RouterType.Socket_Start:
			{
				// CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
				// cc.isTrigger = true;
				lineRenderer.useWorldSpace = true;
			}
			break;
			case RouterType.Socket_End:
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
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
			
		if (routerType != RouterType.Socket_End) {
			
			if ((_routerHandler.hasBeenPlaced && _routerHandler.hasBeenActivated && _routerHandler.canGrow)) {
				
				Grow();
			
			}
			
		}
		else {
			
			Grow();
			
		}
		
		if (_routerHandler.hasBeenPlaced && !_routerHandler.hasBeenActivated) {
			
			switch (routerType) {
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
				if(_routerHandler.EffectGO.GetComponent<BroadcastWaveCreator>().CanGrow)
				{
					SphereCollider sc = GetComponent<SphereCollider>();
					if(sc != null)
					{
						if(_routerHandler.EffectGO.transform.localScale.x < 15) {
							float scaleGrowth = BroadcastGrowSpeed * Time.deltaTime;	
							Vector3 localScale = _routerHandler.EffectGO.transform.localScale;
							_routerHandler.EffectGO.transform.localScale = new Vector3(localScale.x + scaleGrowth, localScale.y + scaleGrowth, localScale.z + scaleGrowth);
						}
						else {
							_routerHandler.canGrow = false;
							_routerHandler.EffectGO.GetComponent<BroadcastWaveCreator>().CanGrow = false;
						}
					}
				}
			}
			break;
			case RouterType.Expander:
			{
				
				RaycastHit hit;
				int typeOfWall = DetermineMaxLineLength(0, out hit);

				if (typeOfWall == 1) {
					
					if (Mathf.Abs(lineRenderer.GetPosition(1).y) < hit.distance) {
						CreateLine(2, lineRenderer.GetPosition(1).y - 0.1f);
					}
					else
					{
						ScaleBetweenPoints scaleBetween = _routerHandler.EffectGO.GetComponent<ScaleBetweenPoints>();
						if(scaleBetween.EndPoint == null)
						{
							GameObject go = new GameObject("Expander Anchor");
							go.transform.position = hit.point;
							scaleBetween.EndPoint = go.transform;
							RouterManagerScript.Instance.HandleResourceSpent();
						}
					}
					
				}	
			}
			break;
			case RouterType.Socket_Start:
			{
				Vector3 socketEnd = GameObject.FindGameObjectWithTag("Socket End").GetComponent<Transform>().position;
				CreateLine(this.transform.position, socketEnd);
			}
			break;
			case RouterType.Socket_End: 
			{
				if (_routerHandler.hasBeenActivated) {
					AnimateWaves();
					CapsuleCollider cc = GetComponent<CapsuleCollider>();
					if (cc.radius < 3.5f) {
						int segments = 50;
						CreateEllipse(segments, cc.radius);
						cc.radius += 0.1f;
					}
				}
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
	
	void CreateLine(int _positions, float _length) {
		
		lineRenderer.numPositions = _positions;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.up * _length);
		
	}
	
	void CreateLine(Vector3 _pos1, Vector3 _pos2) {
		
		lineRenderer.numPositions = 2;
		lineRenderer.SetPosition(0, _pos1);
		lineRenderer.SetPosition(1, _pos2);
		
	}
	
	int DetermineMaxLineLength(int _startPos, out RaycastHit hit) {
		
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