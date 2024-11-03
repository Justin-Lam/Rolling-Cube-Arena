using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
	[SerializeField] EnemySpawner enemySpawner;
	[SerializeField] PlayerProjectilePool playerProjectilePool;

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "Player":
				GameplayManager.Instance.PlayerDied();
				break;

			case "Player Projectile":
				playerProjectilePool.ReleaseObject(other.gameObject);
				break;

			case "Enemy":
				enemySpawner.ReleasePooledEnemy(other.gameObject);
				GameplayManager.Instance.EnemyKnockedOffed();
				break;

			default:
				Destroy(other.gameObject);
				break;
		}
	}
}
