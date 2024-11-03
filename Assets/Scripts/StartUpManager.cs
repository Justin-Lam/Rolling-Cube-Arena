using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpManager : MonoBehaviour
{
    [SerializeField] Material playerMaterial;

    void Start()
    {
		// Initialize player pref values if this is the first time the player is opening the game
		if (!PlayerPrefs.HasKey("PlayerColor_R"))
        {
            PlayerPrefs.SetFloat("PlayerColor_R", playerMaterial.color.r);
        }
		if (!PlayerPrefs.HasKey("PlayerColor_G"))
		{
			PlayerPrefs.SetFloat("PlayerColor_G", playerMaterial.color.g);
		}
		if (!PlayerPrefs.HasKey("PlayerColor_B"))
		{
			PlayerPrefs.SetFloat("PlayerColor_B", playerMaterial.color.b);
		}

		PlayerPrefs.Save();

		// Go to main menu
		SceneNavigator.Instance.GoToMainMenu();
	}
}
