using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> 
{
	public Text WinText;
	public Text LoseText;

	public string NextLevelName;
	public Button NextLevelButton;
	public Button RetryButton;

	public float WinDelay = 1;
	public float LoseDelay = 3;

	private bool _won = false;

	[SerializeField] private int _housesLeft;

	void Start()
	{
		// WinText.gameObject.SetActive(false);
		// LoseText.gameObject.SetActive(false);
		// NextLevelButton.gameObject.SetActive(false);
		// RetryButton.gameObject.SetActive(false);
	}

	public void HandleNewHouseGotSignal()
	{
		_housesLeft--;
		if(_housesLeft == 0)
		{
			_won = true;
			StartCoroutine(Win());
		}
	}

	public void HandleResourcesOver()
	{
		StartCoroutine(Lose());
	}

	public void OnRetryButtonDown()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
	}

	public void OnNextLevelButtonDown()
	{
		SceneManager.LoadScene(NextLevelName); 
	}

	public IEnumerator Win()
	{
		yield return new WaitForSeconds(WinDelay);

		WinText.gameObject.SetActive(true);

		if(NextLevelName == "")
			RetryButton.gameObject.SetActive(true);
		else
			NextLevelButton.gameObject.SetActive(true);
	}

	public IEnumerator Lose()
	{
		yield return new WaitForSeconds(LoseDelay);
	
		if(!_won)
		{
			LoseText.gameObject.SetActive(true);
			RetryButton.gameObject.SetActive(true);
		}
	}
}
