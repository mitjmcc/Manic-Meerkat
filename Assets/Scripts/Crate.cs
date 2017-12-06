using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IJumpable, IBashable {

	public GameObject spawn;
	private Transform spawnPoint;

	public void Start ()
	{
		spawnPoint = transform.Find ("SpawnPoint");
	}

	private void breakParts() {
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<AudioSource> ().Play ();
		foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>()) {
			child.isKinematic = false;
			child.AddRelativeForce (Vector3.up * 200);
			Destroy (child.gameObject, 2);
		}
		Destroy (transform.gameObject, 2.5f);
	}

	public void OnJump() {
		Instantiate(spawn, spawnPoint.position, Quaternion.identity);
		breakParts ();
	}

	public void OnBash() {
		Instantiate(spawn, spawnPoint.position, Quaternion.identity);
		breakParts ();
	}
}
