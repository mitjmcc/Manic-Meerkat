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
        isJumping = Input.GetButton("Jump") && isGrounded;
        jumpTime = isJumping ? Time.time : jumpTime;
		x = (isGrounded) ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical") / airControlFactor;
	    y = Time.time < jumpTime + .3333f ? jumpAcceleration(jumpHeight, .6666f) : -9.81f;
		z = (isGrounded) ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal") / airControlFactor;

		speed = ((x * transform.forward + z * transform.right) * moveForce
			+ y * transform.up) * Time.deltaTime;
		
		body.velocity += speed;
		HorizontalDrag(body.velocity, drag);
	}

    public float jumpAcceleration(float height, float time) {
        return (2 * (height + 1)) / Mathf.Pow(time, 2) + Physics.gravity.magnitude;
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
		// if (body.velocity.y < 0 && !isGrounded) { 
		// 	velocity.y *= 1.1f;
		// }
        
        body.velocity = velocity;
    }
}
