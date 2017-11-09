using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {


	#region PublicVariables
    [Range(0f, 200f)] public float moveSpeed;
    [Range(0f, 30f)] public float jumpHeight;
	[Range(0f, 30f)] public float boxJumpHeight;
	public float airControlFactor = 2;
	public Camera cam;
    #endregion

	#region PrivateVariables
    Rigidbody body;
	Transform model;
    Animator anim;
    Vector3 speed;
    Vector3 forward;

    float x, y, z, jumpTime;
    bool isGrounded;
    bool isJumping;
	bool isBashing;
    #endregion

	// Use this for initialization
    void Start() {
		body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate() {
		isJumping = Input.GetButton("Jump") && isGrounded;
		isBashing = Input.GetButtonDown ("Fire1");

		x = (isGrounded) ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical") / airControlFactor;
		z = (isGrounded) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal") / airControlFactor;

		Vector3 direction = cam.transform.TransformVector(new Vector3 (z, 0, x));

		if (direction.magnitude > 0) {
			speed = direction.normalized * moveSpeed;
		} else {
			speed = new Vector3 ();
		}

		body.velocity = new Vector3 (speed.x, body.velocity.y, speed.z);

		if (isJumping) {
			if (Time.time > jumpTime + 0.2f) {
				body.velocity = new Vector3 (body.velocity.x, jumpHeight, body.velocity.y);
				jumpTime = Time.time;
			}
		}

		if (isBashing) {
			BashAttack ();
		}

		Debug.DrawRay (transform.position + Vector3.down * 0.5f, Vector3.down * 0.5f);
	}

	void BashAttack() {
		foreach(Collider c in Physics.OverlapSphere(transform.position, 2)) {
			IBashable bashable = c.gameObject.GetComponent<IBashable> ();
			if (bashable != null) {
				bashable.OnBash ();
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		IJumpable jumpable = col.gameObject.GetComponent<IJumpable> ();
		if (jumpable != null) {
			foreach (ContactPoint contact in col.contacts) {
				if (body.transform.position.y > col.gameObject.transform.position.y && col.relativeVelocity.y > 1f) {
					jumpable.OnJump ();
					body.velocity = new Vector3(body.velocity.x, boxJumpHeight, body.velocity.y);
					break;
				} else if (body.transform.position.y < col.gameObject.transform.position.y && col.relativeVelocity.y < 1f) {
					jumpable.OnJump ();
					body.velocity = new Vector3(body.velocity.x, -boxJumpHeight, body.velocity.y);
					break;
				}
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		ICollectable collectable = col.gameObject.GetComponent<ICollectable> ();
		if (collectable != null) {
			collectable.OnCollect ();
		}
	}

	void OnCollisionStay(Collision col)
	{
		isGrounded = true;
	}

    void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }
}
