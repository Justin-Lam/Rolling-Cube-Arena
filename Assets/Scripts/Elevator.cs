using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

	float startingY = 0f;
	int numRiders = 0;
	

	void Start()
	{
		startingY = transform.position.y;
	}

	// Update is called once per frame
	void Update()
    {
		if (numRiders > 0)
		{
			transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
		}
		else if (transform.position.y > startingY)
		{
			transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
			if (transform.position.y < startingY)
			{
				transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
			}
		}
	}

	public void GainRider()
	{
		numRiders++;
	}
	public void LoseRider()
	{
		numRiders--;
	}
}
