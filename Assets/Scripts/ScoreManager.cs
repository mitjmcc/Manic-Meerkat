using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	private int fruit;
	public Text fruitText;

	void Start () {
		fruit = 0;
		fruitText.text = "0";
	}

	public void CollectFruit() {
		fruit++;
		GetComponent<AudioSource> ().Play ();
		fruitText.text = fruit.ToString();
	}
}
