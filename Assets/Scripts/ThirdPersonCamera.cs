using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform player;
	[SerializeField] Transform playerMesh;
	[SerializeField] Transform playerOrientation;
	[SerializeField] Rigidbody playerRb;
	[SerializeField] float rotationSpeed = 1f;

	void Update()
	{
		// rotate orientation
		Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
		playerOrientation.forward = viewDirection.normalized;

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		Vector3 inputDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

		if (inputDirection != Vector3.zero)
		{
			playerMesh.forward = Vector3.Slerp(playerMesh.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
		}
	}
}
