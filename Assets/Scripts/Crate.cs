using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IKillable {

	public GameObject spawn;

	void OnCollisionEnter(Collision col) {
		var m = col.gameObject.GetComponent<MovementController>();
		foreach (ContactPoint contact in col.contacts) {
			Debug.Log(Vector3.Dot(transform.up, contact.normal));
			float d = Vector3.Dot(transform.up, contact.normal);
			if (d < 0 && d >= -1) {
				// Check for player eventually
				col.gameObject.GetComponent<Rigidbody>().velocity += 
					Vector3.up * m.jumpAcceleration(1.5f, 1 + Time.deltaTime);
				Kill();
			} else if (d > 0 && d <=1) {
				col.gameObject.GetComponent<Rigidbody>().velocity +=
					Vector3.up * -m.jumpAcceleration(.005f, 1 + Time.deltaTime);
				Kill();
			}
		}
	}

	public void Kill() {
		// Instantiate(spawn, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}
