using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Enemy : MonoBehaviour
{
	// Variables
	Transform playerTransform;
	Rigidbody rb;
	Vector3 moveDirection = Vector3.zero;
	Vector3 rotateDirection = Vector3.zero;
	float distToGround = 0f;
	bool tryingToExplode = false;

	// Parameters
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float rotateSpeed = 10f;
	[SerializeField] float groundedDistFromGroundPadding = 0.1f;

	[SerializeField] float explosionRange = 5f;
	[SerializeField] float explosionDelay = 3f;
	[SerializeField] float explosionStrength = 100f;
	[SerializeField] float explosionRadius = 5f;
	[SerializeField] float explosionUpwardsModifier = 3f;

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
		rotateDirection = Vector3.Cross(Vector3.up, playerDirection).normalized;    // gave ChatGPT my code and asked it how to calculate this

		// Use Explosion
		if (PlayerInExplosionRange() && !tryingToExplode)
		{
			StartCoroutine(ExplosionCountdown(explosionDelay));
		}
	}

	void FixedUpdate()
	{
		// Move and rotate
		if (!tryingToExplode)
		{
			if (IsGrounded())
			{
				rb.AddForce(moveDirection * moveSpeed);
			}
			rb.AddTorque(rotateDirection * rotateSpeed);
		}
	}

	bool IsGrounded()
	{
		// got this from https://discussions.unity.com/t/using-raycast-to-determine-if-player-is-grounded/85134/2
		return Physics.Raycast(transform.position, Vector3.down, distToGround + groundedDistFromGroundPadding);
	}

	bool PlayerInExplosionRange()
	{
		return (playerTransform.position - transform.position).magnitude < explosionRange;
	}

	IEnumerator ExplosionCountdown(float delay)
	{
		tryingToExplode = true;
		yield return new WaitForSeconds(delay);
		if (PlayerInExplosionRange())
		{
			Explode();
		}
		tryingToExplode = false;
	}

	void Explode()
	{
		// From https://docs.unity3d.com/ScriptReference/Rigidbody.AddExplosionForce.html

		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider hit in colliders)
		{
			if (hit.gameObject.CompareTag("Player"))
			{
				Rigidbody rb = hit.GetComponent<Rigidbody>();
				if (rb != null)
				{
					rb.AddExplosionForce(explosionStrength, transform.position, explosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
				}
			}
		}
	}
}
