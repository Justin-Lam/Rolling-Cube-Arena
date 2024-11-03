using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHUDManager : MonoBehaviour
{
	// Singleton Pattern
	static UIHUDManager instance;
	public static UIHUDManager Instance { get { return instance; } }
	void Awake()
	{
		// Singleton Pattern
		if (instance != null && instance != this)
		{
			Destroy(instance);
		}
		else
		{
			instance = this;
		}
	}

	// Variables
	[Header("HUD")]
	[SerializeField] Canvas hud;
	[SerializeField] TMP_Text numEnemiesToKnockOffText;

	[Header("Pause Screen")]
	[SerializeField] Canvas pauseScreen;

	[Header("Win/Lose Screen")]
	[SerializeField] Canvas winLoseScreen;
	[SerializeField] GameObject winTint;
	[SerializeField] GameObject loseTint;
	[SerializeField] GameObject winText;
	[SerializeField] GameObject loseText;

	// Methods
	public void OnStartGame()
	{
		hud.enabled = true;
		winLoseScreen.enabled = false;
	}
	public void OnPauseGame()
	{
		pauseScreen.enabled = true;
	}
	public void OnUnpauseGame()
	{
		pauseScreen.enabled = false;
	}

	public void SetEnemiesToKnockOff(int num)
	{
		numEnemiesToKnockOffText.text = num.ToString(); ;
	}

	public void GameOver(bool won)
	{
		hud.enabled = false;
		winLoseScreen.enabled = true;

		if (won)
		{
			winTint.SetActive(true);
			winText.SetActive(true);
			loseTint.SetActive(false);
			loseText.SetActive(false);
		}
        else
        {
			winTint.SetActive(false);
			winText.SetActive(false);
			loseTint.SetActive(true);
			loseText.SetActive(true);
		}
    }
}
