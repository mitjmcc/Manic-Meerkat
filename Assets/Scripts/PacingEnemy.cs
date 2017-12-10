using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingEnemy : MonoBehaviour, IJumpable, IBashable {

	public Transform[] points = new Transform[2];
	public float speed;
	Animator anim;
	Vector3 target;
	Vector3 start;
	bool isDead;
	float paceAmount;

	void Start() {
		target = points [0].position;
		start = points [1].position;
		anim = GetComponent<Animator>();
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
		if (!isDead)
		transform.position = Vector3.Lerp(start, target, paceAmount);
		transform.LookAt(target);
	}

	public void OnJump() {
		if (!isDead) {
			anim.SetTrigger("dead");
			StartCoroutine("Death");
		}
	}

	public void OnBash() {
		if (!isDead) {
			anim.SetTrigger("dead");
			StartCoroutine("Death");
		}
	}

	IEnumerator Death() {
		for (float f = 8f; f >= 0; f -= 0.1f) {
			isDead = true;
			GetComponent<Collider>().enabled = false;
			if (f < .2f) 
				transform.parent.gameObject.SetActive(false);
			yield return null;
		}
	}
}
