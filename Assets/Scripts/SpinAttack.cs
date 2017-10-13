using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour {

	public float range;
	public bool debug;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
			StartCoroutine("Spin");
	}

	IEnumerator Spin() {
		Vector3 origin = transform.position;
		if (debug) {
			for (int i = 0; i < 8; i++)
				Debug.DrawLine(origin, origin + range * (Quaternion.Euler( 0,i * 45, 0) * transform.forward), Color.blue, .3f);
		}
		foreach(Collider c in Physics.OverlapSphere(origin, range)) {
			var killable = c.gameObject.GetComponent<IKillable>();
			if (killable != null) {
				killable.Kill();
			}
		}
		yield return null;
	}
}
