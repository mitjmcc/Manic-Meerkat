using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Fruit : MonoBehaviour, ICollectable {
	private ScoreManager scoreManager;

	void Start() {
		scoreManager = GameObject.FindObjectOfType<ScoreManager> ();
	}

	public void OnCollect() {
		scoreManager.CollectFruit ();
		gameObject.SetActive(false);
	}
}
