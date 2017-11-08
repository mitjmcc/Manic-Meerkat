using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IKillable {

	public GameObject spawn;

	public void Kill() {
		// Instantiate(spawn, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}
