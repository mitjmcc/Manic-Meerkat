using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IBashable {

	private bool bashed = false;
	Rigidbody body;
	private float timer = .25f;
	private float destroyTime = 1.3f;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}

	void Update() {
		timer -= Time.deltaTime;
		destroyTime -= Time.deltaTime;
		if (destroyTime < 0) {
			GameObject.Destroy (gameObject);
		}
	}

	public void OnBash() {
		body.velocity = -body.velocity;
		destroyTime = 1.3f;
		bashed = true;
		gameObject.tag = "Untagged";
	}

	void OnTriggerEnter(Collider col)
	{
		IBashable bashable = col.gameObject.GetComponent<IBashable>();
		if (bashable != null && bashed) {
			bashable.OnBash();
		}
	}
}
