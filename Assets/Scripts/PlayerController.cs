using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Game
	Vector3 startPosition = Vector3.zero;

	// Physics
	Rigidbody rb;
	Vector3 moveDirection = Vector3.zero;
	Vector3 rotateDirection = Vector3.zero;
	float distToGround = 0f;	// really the distance from the center of the cube to any corner

	// Projectiles
	[SerializeField] PlayerProjectilePool projectilePool;

	// Parameters
	[SerializeField] float moveSpeed = 75f;
	[SerializeField] float rotateSpeed = 75f;
	[SerializeField] float jumpStrength = 75f;
	[SerializeField] float groundedDistFromGroundPadding = 0.1f;

	[SerializeField] float explosionStrength = 10f;
	[SerializeField] float explosionRadius = 5f;
	[SerializeField] float explosionUpwardsModifier = 3f;

	[SerializeField] float throwStrength = 100f;

	// Methods
	private void Awake()
	{
		// Set variables
		startPosition = transform.position;
		rb = GetComponent<Rigidbody>();
		// got this from https://discussions.unity.com/t/using-raycast-to-determine-if-player-is-grounded/85134/2
		distToGround = (gameObject.GetComponent<BoxCollider>().size.y / 2) * Mathf.Sqrt(2);

		// Initialize color
		GetComponent<Renderer>().material.color = new Color(PlayerPrefs.GetFloat("PlayerColor_R"), PlayerPrefs.GetFloat("PlayerColor_G"), PlayerPrefs.GetFloat("PlayerColor_B"));
	}

	public void OnStartGame()
	{
		gameObject.SetActive(true);
		transform.position = startPosition;
		transform.rotation = Quaternion.identity;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
	public void OnDeath()
	{
		gameObject.SetActive(false);
	}

	void Update()
	{
		// Move
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			rotateDirection = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
		}
		else
		{
			moveDirection = Vector3.zero;
			rotateDirection = Vector3.zero;
		}

		// Jump
		if (Input.GetButtonDown("Jump") && IsGrounded())
		{
			rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
		}

		// Explosion
		if (Input.GetButtonDown("Make Explosion"))
		{
			// From https://docs.unity3d.com/ScriptReference/Rigidbody.AddExplosionForce.html

			Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach (Collider hit in colliders)
			{
				if (hit.gameObject.CompareTag("Player"))
				{
					continue;
				}

				Rigidbody rb = hit.GetComponent<Rigidbody>();
				if (rb != null)
				{
					rb.AddExplosionForce(explosionStrength, transform.position, explosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
				}
			}
		}

		// Throw
		if (Input.GetButtonDown("Throw Ball"))
		{
			GameObject projectile = projectilePool.GetObject();

			Renderer rr = projectile.GetComponent<Renderer>();
			if (rr)
			{
				rr.material.color = new Color(PlayerPrefs.GetFloat("PlayerColor_R"), PlayerPrefs.GetFloat("PlayerColor_G"), PlayerPrefs.GetFloat("PlayerColor_B"));
			}

			projectile.transform.position = transform.position + Vector3.forward * distToGround;
			projectile.GetComponent<Rigidbody>().AddForce(Vector3.forward * throwStrength, ForceMode.Impulse);
			projectile.GetComponent<Rigidbody>().AddTorque(projectile.transform.right * throwStrength, ForceMode.Impulse);
		}
	}

	void FixedUpdate()
	{
		rb.AddForce(moveDirection * moveSpeed);
		rb.AddTorque(rotateDirection * rotateSpeed);
	}

	bool IsGrounded()
	{
		// got this from https://discussions.unity.com/t/using-raycast-to-determine-if-player-is-grounded/85134/2
		return Physics.Raycast(transform.position, Vector3.down, distToGround + groundedDistFromGroundPadding);
	}
}
