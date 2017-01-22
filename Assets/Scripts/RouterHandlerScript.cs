// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterHandlerScript : MonoBehaviour {

	public GameObject ExpanderWavePrefab;	
	public GameObject BroadcastWaveCreatorPrefab;

	[HideInInspector]
	public GameObject EffectGO;

	private RouterAttributes _routerAttrib;

	
	// Radius
	[HideInInspector]
	public bool canGrow = true;
	[HideInInspector]
	public bool canBePlaced = false;
	[HideInInspector]
	public bool hasBeenPlaced = false;
	[HideInInspector]
	public bool hasBeenActivated = false;
	
	// private Vector3 finalPosition;
	[HideInInspector]
	public SphereCollider sCollider;
	
	// Trigger events
	private int triggerCounter;
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		
		sCollider = GetComponent<SphereCollider>();
		_routerAttrib = GetComponent<RouterAttributes>();
		
	}
	
	void Start() {
		
		if (_routerAttrib.routerType != RouterAttributes.RouterType.Broadcaster && _routerAttrib.routerType != RouterAttributes.RouterType.Socket_End) {
			canBePlaced = false;
			ChangeColor(Color.red);
		}
		
	}
	
	void Update() {
		
		if (!hasBeenPlaced && !hasBeenActivated && Input.GetMouseButtonDown(0)) {
				
			if (canBePlaced && triggerCounter == 0) {
				
				print("Placed");
				hasBeenPlaced = true;
				GetComponentInParent<RouterManagerScript>().ResetRouterState();
				return;
				
			}
			
		}
		else if (hasBeenPlaced && !hasBeenActivated) {
			
			if (_routerAttrib.routerType == RouterAttributes.RouterType.Broadcaster) {
				
				print("Activated " + transform.name);
				hasBeenActivated = true;

				GameObject broadcastCreatorGO = (GameObject)Instantiate(BroadcastWaveCreatorPrefab, transform.position, Quaternion.identity);
				EffectGO = broadcastCreatorGO;
				EffectGO.transform.localScale = transform.localScale;

				//CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
				SphereCollider sc = broadcastCreatorGO.AddComponent<SphereCollider>();
				//sc.radius = 0.3f;
				sc.isTrigger = true;
			}
			else if (_routerAttrib.routerType == RouterAttributes.RouterType.Expander) {
				
				Vector3 mouseScreenPosition = Input.mousePosition;
				mouseScreenPosition.z = transform.position.z;
				transform.LookAt(Camera.main.ScreenToWorldPoint(mouseScreenPosition), Vector3.back);
				transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
				
				if (Input.GetMouseButtonDown(0)) {
					
					print("Activated " + transform.name);
					hasBeenActivated = true;
					EffectGO = (GameObject)Instantiate(ExpanderWavePrefab);
					ScaleBetweenPoints line = EffectGO.GetComponent<ScaleBetweenPoints>();
					GameObject go = new GameObject("Temporary");
					go.transform.position = transform.position - (transform.up * 0.75f);
					line.StartPoint = go.transform;
					
				}
				
			}
			else if (_routerAttrib.routerType == RouterAttributes.RouterType.Socket_Start) {
				
				print("Activated " + transform.name);
				hasBeenActivated = true;
				GetComponentInParent<RouterManagerScript>().CreateRouter(SelectionManager.Selections.Socket_End);
				
			}
			else {
				
				print("Activated " + transform.name);
				hasBeenActivated = true;
				
			}
			
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		
		if (!hasBeenPlaced && !hasBeenActivated) {
			
			transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.eulerAngles = new Vector3(Mathf.Round(transform.eulerAngles.x), Mathf.Round(transform.eulerAngles.y), Mathf.Round(transform.eulerAngles.z));
			
		}
		
	}
	
	// Check collisions with signal disruptors
	void OnTriggerEnter(Collider other) {
		
		if (hasBeenPlaced && hasBeenActivated) {
			
			if (other.CompareTag("Signal Disruptor")) {
				canGrow = false;
				//GetComponent<CapsuleCollider>().radius -= 0.65f;
			}
			
		}
		else {
			
			if (other.GetType() != typeof(SphereCollider)) {
				triggerCounter++;
			} 
			// Check for expander and socket placement
			else {
				
				if (!hasBeenPlaced && 
				(_routerAttrib.routerType != RouterAttributes.RouterType.Broadcaster)
				) {
					
					print("canBePlaced");
					ChangeColor(Color.white);
					canBePlaced = true;
					
				}
				
			}
			
		}
		
	}
	
	void OnTriggerExit(Collider other) {
		
		if (other.GetType() != typeof(SphereCollider)) {
			triggerCounter--;
		} 
		// Check for expander and socket placement
		else {
			
			if (!hasBeenPlaced && 
			(_routerAttrib.routerType != RouterAttributes.RouterType.Broadcaster && _routerAttrib.routerType != RouterAttributes.RouterType.Socket_End)
			) {
				
				ChangeColor(Color.red);
				canBePlaced = false;
				
			}
			
		}
		
	}
	
	void ChangeColor(Color _c) {
		
		GetComponent<MeshRenderer>().material.color = _c;
		
	}

}