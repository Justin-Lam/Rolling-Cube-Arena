using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
	// Singleton Pattern
	static SceneNavigator instance;
	public static SceneNavigator Instance { get { return instance; } }
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
			DontDestroyOnLoad(gameObject);
		}
	}

	// Methods
	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void GoToOptions()
	{
		SceneManager.LoadScene("Options");
	}
	public void GoToCredits()
	{
		SceneManager.LoadScene("Credits");
	}
	public void GoToGameplay()
	{
		SceneManager.LoadScene("Gameplay");
	}
}
