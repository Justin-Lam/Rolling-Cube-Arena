using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	[SerializeField] float forwardMultiplier = 5f;
	[SerializeField] float upMultiplier = 1f;
	[SerializeField] float launchStrength = 10f;

	void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();

		if (rb != null)
		{
			rb.AddForce((transform.forward * forwardMultiplier + transform.up * upMultiplier) * launchStrength * rb.mass, ForceMode.Impulse);
		}
	}
}
