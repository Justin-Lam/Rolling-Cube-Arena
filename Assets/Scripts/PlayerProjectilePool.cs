using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerProjectilePool : MonoBehaviour
{
	// Singleton Pattern
	static PlayerProjectilePool instance;
	public static PlayerProjectilePool Instance { get { return instance; } }
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
	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] int defaultSize = 10;
	[SerializeField] int maxSize = 20;
	ObjectPool<GameObject> pool = null;
	public GameObject GetObject()
	{
		return pool.Get();
	}
	public void ReleaseObject(GameObject go)
	{
		pool.Release(go);
	}

	// Methods
	void Start()
	{
		// Initialize the pool
		// learned how to use ObjectPool from https://thegamedev.guru/unity-cpu-performance/object-pooling/#how-to-use-the-new-object-pooling-api-in-unity-2021 and https://www.youtube.com/watch?v=7EZ2F-TzHYw&t=595s
		pool = new ObjectPool<GameObject>(
			createFunc: () =>
			{
				return Instantiate(projectilePrefab, transform);
			},
			actionOnGet: (go) =>
			{
				go.SetActive(true);
			},
			actionOnRelease: (go) =>
			{
				go.SetActive(false);
			},
			actionOnDestroy: (go) =>
			{
				Destroy(go);
			},
			collectionCheck: false,
			defaultCapacity: defaultSize,
			maxSize: maxSize
		);
	}

	public void ReleaseAll()
	{
		// learned you can foreach loop like this from https://discussions.unity.com/t/finding-all-children-of-object/653529/2
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag("Player Projectile"))
			{
				pool.Release(child.gameObject);
			}
		}
	}
}
