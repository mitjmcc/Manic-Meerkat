using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


	#region PublicVariables
    [Range(0f, 200f)] public float moveForce;
    [Range(0f, 15f)] public float jumpHeight;
    [Range(0f, 1f)] public float drag = .1f;
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
    #endregion

	// Use this for initialization
    void Start() {
		body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate() {
        isJumping = Input.GetButtonDown("Jump") && isGrounded;
        jumpTime = isJumping ? Time.time : jumpTime;
		x = (isGrounded) ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical") / airControlFactor;
		z = (isGrounded) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal") / airControlFactor;

		Vector3 direction = cam.transform.TransformVector(new Vector3 (z, 0, x));

		if (direction.magnitude > 0) {
			speed = direction.normalized * moveForce;
		} else {
			speed = new Vector3 ();
		}

		speed = Vector3.Lerp (body.velocity, speed, 1);

		body.velocity = new Vector3 (speed.x, body.velocity.y, speed.z);
		if (isJumping) {
			body.velocity += Vector3.up * 20f;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		IKillable killable = col.gameObject.GetComponent<IKillable> ();
		if (killable != null) {
			foreach (ContactPoint contact in col.contacts) {
				float d = Vector3.Dot (transform.up, contact.normal);
				if (d > 0 && d <= 1) {
					killable.Kill ();
					body.velocity += Vector3.up * 20f;
					break;
				} else if (d >= -1 && d < 0) {
					killable.Kill ();
					body.velocity -= Vector3.up * 20f;
					break;
				}
			}
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
