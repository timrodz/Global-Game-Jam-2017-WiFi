using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : SingletonMonoBehaviour<SelectionManager> 
{
	public enum Selections
	{
		None,
		Broadcaster,
		Expander,
		Socket
	}

	public RouterManagerScript RouterManager;

	public Text BroadcasterAmount;
	public Text ExpanderAmount;
	public Text SocketAmount;

	[HideInInspector]
	public Selections Selection;

	void Start()
	{
		updateBroadcasterAmount();
		updateExpanderAmount();
		updateSocketAmount();
	}

	public void OnBroadcasterButtonDown()
	{
		print("OnBroadcasterButtonDown");
		Selection = Selections.Broadcaster;

		RouterManager.CreateRouter(Selection);
		updateBroadcasterAmount();
	}

	public void OnExpanderButtonDown()
	{
		print("OnExpanderButtonDown");
		Selection = Selections.Expander;

		RouterManager.CreateRouter(Selection);
	    updateExpanderAmount();
	}

	public void OnSocketButtonDown()
	{
		print("OnSocketButtonDown");
		Selection = Selections.Socket;

		RouterManager.CreateRouter(Selection);
		updateSocketAmount();
	}

	private void updateBroadcasterAmount()
	{
		BroadcasterAmount.text = RouterManager.BroadcasterAmount.ToString();
	}

	private void updateExpanderAmount()
	{
		ExpanderAmount.text = RouterManager.ExpanderAmount.ToString();
	}

	private void updateSocketAmount()
	{
		SocketAmount.text = RouterManager.SocketAmount.ToString();
	}

}
