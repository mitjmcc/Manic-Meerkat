using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public ParticleSystem particle;
	public float teleportTime;

	private bool teleporting;
	private float timer;


	void Update() {
		if (teleporting && (timer -= Time.deltaTime) < 0) {
			GameObject.FindObjectOfType<LevelChanger>().loadLevel("MainMenu");
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag("Player")) {
			particle.gameObject.SetActive(true);
			teleporting = true;
			timer = teleportTime;
		}
	}
}
