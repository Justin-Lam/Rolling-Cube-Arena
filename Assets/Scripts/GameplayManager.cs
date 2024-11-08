using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	// Singleton Pattern
	static GameplayManager instance;
	public static GameplayManager Instance { get { return instance; } }
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

	// Variables
	bool paused = false;
	[SerializeField] int numKnockOffsToWin = 3;
	int numKnockOffs = 0;
	[SerializeField] PlayerController playerController;
	[SerializeField] GameObject launcherPrefab;
	[SerializeField] int numLaunchers = 1;
	[SerializeField] GameObject elevatorPrefab;
	[SerializeField] int numElevators = 1;
	[SerializeField] Transform groundTransform;
	List<GameObject> launchersAndElevators = new List<GameObject>();

	// Methods
	void Start()
	{
		StartGame();
	}
	void Update()
	{
		// Pause
		if (Input.GetButtonDown("Cancel"))
		{
			if (paused)
			{
				UnpauseGame();
				Cursor.lockState = CursorLockMode.Locked;
				UIHUDManager.Instance.OnUnpauseGame();
			}
			else
			{
				PauseGame();
				Cursor.lockState = CursorLockMode.None;
				UIHUDManager.Instance.OnPauseGame();

			}
		}
	}

	public void StartGame()
	{
		// Pause
		// unpause
		UnpauseGame();
		UIHUDManager.Instance.OnUnpauseGame();

		// Cursor
		// lock cursor
		Cursor.lockState = CursorLockMode.Locked;

		// Game
		// set num knock offs
		numKnockOffs = 0;

		// Player
		// show and reset player position, rotation, and velocity
		playerController.OnStartGame();
		// release all projectiles
		PlayerProjectilePool.Instance.ReleaseAll();


		// Enemy Spawner
		// release all enemies and start spawning
		EnemySpawner.Instance.OnStartGame();

		// Launchers & Elevators
        foreach (GameObject go in launchersAndElevators)
        {
			Destroy(go);
        }
		if (launchersAndElevators.Count > 0)
		launchersAndElevators.Clear();

		// Launchers
		// spawn launchers with random position and rotation
		for (int i = 0; i < numLaunchers; i++)
		{
			GameObject launcher = Instantiate(launcherPrefab);
			launcher.transform.position = new Vector3(Random.Range(-groundTransform.localScale.x / 2, groundTransform.localScale.x / 2), 0, Random.Range(-groundTransform.localScale.z / 2, groundTransform.localScale.z / 2));
			launcher.transform.Rotate(0, Random.Range(0, 360), 0);
			launchersAndElevators.Add(launcher);

		}

		// Elevators
		// spawn elevators with random position
		for (int i = 0; i < numElevators; i++)
		{
			GameObject elevator = Instantiate(elevatorPrefab);
			elevator.transform.position = new Vector3(Random.Range(-groundTransform.localScale.x / 2, groundTransform.localScale.x / 2), 0, Random.Range(-groundTransform.localScale.z / 2, groundTransform.localScale.z / 2));
			launchersAndElevators.Add(elevator);
		}

		// UI & HUD
		// show and set things
		UIHUDManager.Instance.OnStartGame();
		UIHUDManager.Instance.SetEnemiesToKnockOff(numKnockOffsToWin);
	}
	void PauseGame()
	{
		paused = true;
		Time.timeScale = 0;     // learned about using Time.timeScale to pause from https://discussions.unity.com/t/how-to-freeze-and-unfreeze-my-game/311091/4
	}
	void UnpauseGame()
	{
		paused = false;
		Time.timeScale = 1;     // learned about using Time.timeScale to pause from https://discussions.unity.com/t/how-to-freeze-and-unfreeze-my-game/311091/4
	}

	public void EnemyKnockedOffed()
	{
		numKnockOffs++;
		if (numKnockOffs >= numKnockOffsToWin )
		{
			GameOver(true);
		}
		UIHUDManager.Instance.SetEnemiesToKnockOff(numKnockOffsToWin - numKnockOffs);
	}
	public void PlayerDied()
	{
		playerController.OnDeath();
		GameOver(false);
	}

	void GameOver(bool won)
	{
		PauseGame();
		Cursor.lockState = CursorLockMode.None;
		EnemySpawner.Instance.StopSpawning();
		UIHUDManager.Instance.GameOver(won);
	}

	public void GoToMainMenu()
	{
		UnpauseGame();
		SceneNavigator.Instance.GoToMainMenu();
	}
}
