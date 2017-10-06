using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


	#region PublicVariables
    public float Velocity;

    [Range(0f, 200f)] public float moveForce;
    [Range(0f, 400f)] public float jumpForce;
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

    float x, y, z;
    bool isGrounded;
    #endregion

	// Use this for initialization
	void Awake () {
		body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		x = (isGrounded) ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical") / airControlFactor;
		y = Input.GetButtonDown("Jump") && isGrounded ? jumpForce : 0;
		z = (isGrounded) ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal") / airControlFactor;

		speed = ((x /*+ y / 4*/) * transform.forward + z * transform.right
			+ y * transform.up) * moveForce * Time.deltaTime;
		
		body.velocity += speed;

		HorizontalDrag(body.velocity, drag);
	}

	void OnCollisionStay(Collision col)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }

	void HorizontalDrag(Vector3 velocity, float drag)
    {
        velocity.x *= 1 - drag;
        velocity.z *= 1 - drag;
		if (body.velocity.y < 0 && !isGrounded) {
			velocity.y += .02f * Physics.gravity.y;
		}
        body.velocity = velocity;
    }
}
