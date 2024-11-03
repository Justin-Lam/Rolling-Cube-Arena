using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToColor : MonoBehaviour
{
	[SerializeField] float rotateSpeed = 1f;
	Renderer rr;		// getting the mesh renderer here

	void Start()
	{
		// Get renderer
		rr = GetComponent<Renderer>();

		// Initialize color
		UpdateColor();
	}

	void Update()
	{
		// Rotate
		// material.color.r/g/b is going to return a value between [0, 1]
		transform.Rotate((rr.material.color.r - 0.5f) * rotateSpeed, (rr.material.color.g - 0.5f) * rotateSpeed, (rr.material.color.b - 0.5f) * rotateSpeed, Space.World);
	}

	public void UpdateColor()
	{
		rr.material.color = new Color(PlayerPrefs.GetFloat("PlayerColor_R"), PlayerPrefs.GetFloat("PlayerColor_G"), PlayerPrefs.GetFloat("PlayerColor_B"));
	}
}
