using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingEnemy : MonoBehaviour, IKillable {

	public Transform[] points = new Transform[2];
	public float speed;
	Vector3 target;
	int index;

	void Start() {
		target = points[index].position;
	}

	// Update is called once per frame
	void Update () {
		if ((transform.position - target).sqrMagnitude < 0.0001) {
			target = points[index++ % points.Length].position;
		}
		transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision col) {
		var m = col.gameObject.GetComponent<MovementController>();
		foreach (ContactPoint contact in col.contacts) {
			Debug.Log(Vector3.Dot(transform.up, contact.normal));
			float d = Vector3.Dot(transform.up, contact.normal);
			if (d < 0 && d >= -1) {
				// Check for player eventually
				col.gameObject.GetComponent<Rigidbody>().velocity += 
					Vector3.up * m.jumpAcceleration(.5f, 1 + Time.deltaTime);
				Kill();
			}
		}
	}

	public void Kill() {
		gameObject.SetActive(false);
	}
}
