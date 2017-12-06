using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTCrate : MonoBehaviour, IJumpable, IBashable {

	private Transform spawnPoint;
	private GameObject explosion;

	public void Start ()
	{
		explosion = transform.Find ("SpawnPoint/Explosion").gameObject;
	}

	public void Explode() {
		GetComponent<BoxCollider> ().enabled = false;
		explosion.SetActive (true);
		foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>()) {
			child.isKinematic = false;
			child.AddRelativeForce (Vector3.up * 2000);
			Destroy (child.gameObject, 2);
		}
		Destroy (transform.gameObject, 2.5f);
	}

	public void OnJump() {
		Explode ();
	}

	public void OnBash() {
		Explode ();
	}
}
