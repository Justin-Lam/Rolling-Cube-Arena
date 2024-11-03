using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyNameGenerator : MonoBehaviour
{
	TMP_Text nameText;
	[SerializeField] string[] names;

	void Start()
	{
		// Get name text
		nameText = GetComponent<TMP_Text>();

		// Set name to a random name from names[]
		nameText.text = names[Random.Range(0, names.Length)];
	}
}
