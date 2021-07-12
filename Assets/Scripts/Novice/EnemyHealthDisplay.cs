using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour
{

	Text healthText;
	Enemy enemy;

	// Use this for initialization
	void Start()
	{
		healthText = GetComponent<Text>();
		enemy = FindObjectOfType<Enemy>();
	}

	// Update is called once per frame
	void Update()
	{
		healthText.text = enemy.GetHealth().ToString();
	}
}


