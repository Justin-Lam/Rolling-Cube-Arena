using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
	[SerializeField] Elevator elevatorScript;

	void OnTriggerEnter(Collider other)
	{
		// Parent the elevator to the game object
		other.gameObject.transform.SetParent(transform.parent, true);
		elevatorScript.GainRider();
	}
	void OnTriggerExit(Collider other)
	{
		// Unparent the elevator from the game object
		other.gameObject.transform.SetParent(null);
		elevatorScript.LoseRider();
	}
}
