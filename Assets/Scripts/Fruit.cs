using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour {

	public Text fruit;
	static int num;

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.GetComponent<MovementController>() != null) { // replace with player tag
			num++;
			fruit.text = "Fruit: " + num;
			gameObject.SetActive(false);
		}
	}
}
