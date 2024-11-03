using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 offset = new Vector3(0f, 1.0f, -3.5f);

    void Update()
    {
		// Follow the player but only the position as to not rotate
		transform.position = playerTransform.position + offset;             // this post jogged my memory on how to do this https://discussions.unity.com/t/prevent-camera-rotation-attached-to-character/775776/4
	}
}
