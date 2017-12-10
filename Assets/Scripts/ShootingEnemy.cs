using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour, IJumpable, IBashable {

	[Range(0, 60)]
	public float angle = 15;
	public float shootTime;
	public GameObject projectile;
	public Vector2 projectileVelocity;

	Vector3 eulerA;
	Animator anim;
	float shootTimer;
	float[] angles = new float[3];
	int index;
	bool isDead;

    // Use this for initialization
    void Start () {
		anim = GetComponentInChildren<Animator>();
		shootTimer = shootTime;
		LaunchProjectile();
		// Store +angle, 0, and -angle
		float r = transform.rotation.eulerAngles.y;
		for (int i = 0; i < 3; ++i)
			angles[i] = r + angle - i * angle;
	}
	
	// Update is called once per frame
	void Update () {
		if ((shootTimer -= Time.deltaTime) < 0 && !isDead) {
			eulerA = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(eulerA.x, angles[index++ % angles.Length], eulerA.z);
			shootTimer = shootTime;
			LaunchProjectile();
		}
	}

	void LaunchProjectile() {
		anim.SetTrigger("attack");
		GameObject p = Instantiate(projectile, transform.position + transform.up, Quaternion.identity);
		p.transform.SetParent(transform);
		p.GetComponent<Rigidbody>().velocity = transform.forward * projectileVelocity.x
			+ transform.up * projectileVelocity.y;
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
				gameObject.SetActive(false);
			yield return null;
		}
	}
}
