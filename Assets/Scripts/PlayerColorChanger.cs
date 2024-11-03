// The idea of using player prefs to save color came from ChatGPT when I asked it how to use three sliders to adjust the player's color and have the changes persist across various scenes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorChanger : MonoBehaviour
{
	[SerializeField] Material playerMaterial;
	[SerializeField] RotateToColor cubeRTC;
    [SerializeField] Slider rSlider;
    [SerializeField] Slider gSlider;
    [SerializeField] Slider bSlider;

    void Start()
    {
		// Initialize sliders
		SetSliders();
	}

	void SetSliders()
	{
		rSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("PlayerColor_R"));
		gSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("PlayerColor_G"));
		bSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("PlayerColor_B"));
	}
	void ChangeColor(float r, float g, float b)
	{
		// Save new color
		PlayerPrefs.SetFloat("PlayerColor_R", r);
		PlayerPrefs.SetFloat("PlayerColor_G", g);
		PlayerPrefs.SetFloat("PlayerColor_B", b);
		PlayerPrefs.Save();

		// Update cube color
		cubeRTC.UpdateColor();
	}

	public void OnSliderChanged()
	{
		ChangeColor(rSlider.value, gSlider.value, bSlider.value);
	}
	public void OnResetButtonPressed()
	{
		ChangeColor(playerMaterial.color.r, playerMaterial.color.g, playerMaterial.color.b);
		SetSliders();
	}
}
