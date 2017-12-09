using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingEnemy : MonoBehaviour, IJumpable, IBashable {

	public Transform[] points = new Transform[2];
	public float speed;
	Vector3 target;
	Vector3 start;
	float paceAmount;

	void Start() {
		target = points [0].position;
		start = points [1].position;
	}

	// Update is called once per frame
	void Update () {
		paceAmount += speed * Time.deltaTime;
		if (paceAmount >= 1) {
			paceAmount = 0;
			Vector3 temp = target;
			target = start;
			start = temp;
		}
		transform.position = Vector3.Lerp(start, target, paceAmount);
	}

	public void OnJump() {
		gameObject.SetActive(false);
	}

	public void OnBash() {
		gameObject.SetActive (false);
	}
}
