using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IJumpable, IBashable {

	public GameObject spawn;

	public void OnJump() {
		// Instantiate(spawn, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}

	public void OnBash() {
		// Instantiate(spawn, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}
