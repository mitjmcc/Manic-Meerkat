using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, ICollectable {
	private ScoreManager scoreManager;

	public void OnCollect() {
		scoreManager = GameObject.FindObjectOfType<ScoreManager> ();
		scoreManager.CollectFruit ();
		gameObject.SetActive(false);
	}
}
