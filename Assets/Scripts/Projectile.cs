using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IBashable {

	Rigidbody body;
	private float timer = .25f;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}

	void Update() {
		timer -= Time.deltaTime;
	}

	public void OnBash() {
		body.velocity = -body.velocity;
	}

	void OnTriggerEnter(Collider col)
	{
		IBashable bashable = col.gameObject.GetComponent<IBashable>();
		if (bashable != null && !(timer > 0)) {
			bashable.OnBash();
		}
	}
}
