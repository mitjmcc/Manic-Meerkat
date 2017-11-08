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

	public void Kill() {
		gameObject.SetActive(false);
	}
}
