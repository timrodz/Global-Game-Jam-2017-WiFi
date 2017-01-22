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

	public void CreateRouter(SelectionManager.Selections selection) {
		
		if (chosenRouter == null) {
			
			switch (selection) {
				case SelectionManager.Selections.Broadcaster:
					if (BroadcasterAmount > 0) {
						chosenRouter = routerPrefab[0];
						BroadcasterAmount--;
					}
				break;
				case SelectionManager.Selections.Expander:
					if (ExpanderAmount > 0) {
						chosenRouter = routerPrefab[1];
						ExpanderAmount--;
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
			
			if (chosenRouter != null) {
				GameObject go = Instantiate(chosenRouter.gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
				go.transform.parent = this.transform;
			}
			
		}
		
	}
	
	public void ResetRouterState() {
		
		chosenRouter = null;
		
	}
	
}