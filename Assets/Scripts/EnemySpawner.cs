using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
	// Singleton Pattern
	static EnemySpawner instance;
	public static EnemySpawner Instance { get { return instance; } }
	void Awake()
	{
		// Singleton Pattern
		if (instance != null && instance != this)
		{
			Destroy(instance);
		}
		else
		{
			instance = this;
		}
	}

	// Attributes
	[SerializeField] Transform playerTransform = null;		// to give to enemies so they can follow the player
	[SerializeField] GameObject enemyPrefab = null;
	[SerializeField] float spawnDelay = 5f;
	[SerializeField] int poolDefaultSize = 10;
	[SerializeField] int poolMaxSize = 20;
	[SerializeField] float minEnemySize = 0.5f;
	[SerializeField] float maxEnemySize = 1.5f;
	ObjectPool<GameObject> enemyPool = null;
	public void ReleasePooledEnemy(GameObject go)
	{
		enemyPool.Release(go);
	}
	IEnumerator SpawnEnemiesCoroutine = null;

	// Methods
	void Start()
	{
		// Initialize the enemy pool
		// learned how to use ObjectPool from https://thegamedev.guru/unity-cpu-performance/object-pooling/#how-to-use-the-new-object-pooling-api-in-unity-2021 and https://www.youtube.com/watch?v=7EZ2F-TzHYw&t=595s
		enemyPool = new ObjectPool<GameObject>(
			createFunc: () =>
			{
				return Instantiate(enemyPrefab, transform);
			},
			actionOnGet: (enemy) =>
			{
				enemy.gameObject.SetActive(true);
			},
			actionOnRelease: (enemy) =>
			{
				enemy.gameObject.SetActive(false);
			},
			actionOnDestroy: (enemy) =>
			{
				Destroy(enemy.gameObject);
			},
			collectionCheck: false,
			defaultCapacity: poolDefaultSize,
			maxSize: poolMaxSize
		);

		// Set SpawnEnemiesCoroutine
		SpawnEnemiesCoroutine = SpawnEnemiesLoop(spawnDelay);
	}

	IEnumerator SpawnEnemiesLoop(float delay)
	{
		while (true)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(delay);
		}
	}
	void SpawnEnemy()
	{
		GameObject go = enemyPool.Get();
		go.GetComponent<Enemy>().Initialize(transform.position, playerTransform, Random.Range(minEnemySize, maxEnemySize));
	}

	public void ReleaseAll()
	{
		// learned you can foreach loop like this from https://discussions.unity.com/t/finding-all-children-of-object/653529/2
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Enemy"))
			{
				enemyPool.Release(child.gameObject);
			}
		}
	}
	public void StartSpawning()
	{
		StartCoroutine(SpawnEnemiesCoroutine);
	}
	public void StopSpawning()
	{
		StopCoroutine(SpawnEnemiesCoroutine);
	}
}
