using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterManagerScript : MonoBehaviour {

	public Transform[] routerPrefab;
	private Transform chosenRouter;

	// [HideInInspector]
	public int BroadcasterAmount = 5;
	// [HideInInspector]
	public int ExpanderAmount =  3; 
	// [HideInInspector]
	public int SocketAmount = 4;
	
	public Transform broadcastWave;
	public Transform expandWave;
	public Transform socketWave;
	
	private bool hasPlacedBroadcaster = false;
	private bool hasPlacedExpander = false;

	public void CreateRouter(SelectionManager.Selections selection) {
		
		if (chosenRouter == null) {
			
			switch (selection) {
				case SelectionManager.Selections.Broadcaster:
					if (BroadcasterAmount > 0) {
						chosenRouter = routerPrefab[0];
						BroadcasterAmount--;
						if (!hasPlacedBroadcaster)
							hasPlacedBroadcaster = true;
					}
				break;
				case SelectionManager.Selections.Expander:
					if (ExpanderAmount > 0) {
						chosenRouter = routerPrefab[1];
						ExpanderAmount--;
						if (!hasPlacedExpander)
							hasPlacedExpander = true;
					}
				break;
				case SelectionManager.Selections.Socket:
					if (SocketAmount > 0) {
						chosenRouter = routerPrefab[2];
						SocketAmount--;
					}
				break;
				default:
					chosenRouter = null;
				break;
			}
			
			if (hasPlacedBroadcaster) {
				StartCoroutine(FadeIn(GameObject.Find("Expander Button").GetComponent<CanvasGroup>()));
			}
			if (hasPlacedExpander) {
				StartCoroutine(FadeIn(GameObject.Find("Socket Button").GetComponent<CanvasGroup>()));
			}
			
			if (chosenRouter != null) {
				GameObject go = Instantiate(chosenRouter.gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
				go.transform.parent = this.transform;
			}
			
		}
		
	}
	
	public void ResetRouterState() {
		
		chosenRouter = null;
		
	}
	
	IEnumerator FadeIn(CanvasGroup _cg) {
		
		for (float t = 0; t <= 1; t += (Time.deltaTime)) {
			
			_cg.alpha = t;
			yield return null;
			
		}
		
		_cg.alpha = 1;
		_cg.blocksRaycasts = true;
		
	}
	
}