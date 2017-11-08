using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


	#region PublicVariables
    [Range(0f, 200f)] public float moveForce;
    [Range(0f, 15f)] public float jumpHeight;
    [Range(0f, 1f)] public float drag = .1f;
	public float airControlFactor = 2;
    #endregion

	#region PrivateVariables
    Rigidbody body;
    Camera cam;
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

		Vector3 direction = new Vector3 (z, 0, x);

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

	//Crate jump acceleration
    public float jumpAcceleration(float height, float time) {
		return 23f;
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
