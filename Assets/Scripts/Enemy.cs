using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Enemy : MonoBehaviour
{
	// Movement Variables
	Transform playerTransform;
	Rigidbody rb;
	Vector3 moveDirection = Vector3.zero;
	Vector3 rotateDirection = Vector3.zero;
	float distToGround = 0f;

	// Movement Parameters
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float rotateSpeed = 10f;
	[SerializeField] float groundedDistFromGroundPadding = 0.1f;

	// Methods
	public void Initialize(Vector3 spawnLocation, Transform playerTransform, float size)
	{
		// Set position
		transform.position = spawnLocation;

		// Set playerTransform
		this.playerTransform = playerTransform;

		// Set size
		if (!rb)
		{
			rb = GetComponent<Rigidbody>();
		}
		transform.localScale = new Vector3(size, size, size);
		rb.mass *= size;
		moveSpeed *= size;
		rotateSpeed *= size;
	}

	void Start()
	{
		// Get rb
		if (!rb)
		{
			rb = GetComponent<Rigidbody>();
		}
		// Calculate distToGround
		// got this from https://discussions.unity.com/t/using-raycast-to-determine-if-player-is-grounded/85134/2
		distToGround = (gameObject.GetComponent<BoxCollider>().size.y / 2) * Mathf.Sqrt(2);
	}

	void Update()
	{
		// Set move and rotation direction
		Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
		moveDirection = playerDirection;
		rotateDirection = Vector3.Cross(Vector3.up, playerDirection).normalized;	// gave ChatGPT my code and asked it how to calculate this

	}

	void FixedUpdate()
	{
		// Move and rotate
		if (IsGrounded())
		{
			// only move if on the ground
			rb.AddForce(moveDirection * moveSpeed);
		}
		rb.AddTorque(rotateDirection * rotateSpeed);
	}

	bool IsGrounded()
	{
		// got this from https://discussions.unity.com/t/using-raycast-to-determine-if-player-is-grounded/85134/2
		return Physics.Raycast(transform.position, Vector3.down, distToGround + groundedDistFromGroundPadding);
	}
}
