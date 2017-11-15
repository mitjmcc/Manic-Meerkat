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
		anim = GetComponent<Animator>();
	}
	
	void FixedUpdate() {
		isJumping = Input.GetButton("Jump") && isGrounded;
		isBashing = Input.GetButtonDown("Fire1");

		x = (isGrounded) ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical") / airControlFactor;
		z = (isGrounded) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal") / airControlFactor;

		Vector3 direction = cam.transform.TransformVector(new Vector3 (z, 0, x));

		if (direction.magnitude > 0) {
			speed = direction.normalized * moveSpeed;
			anim.speed = speed.magnitude;
		} else {
			speed = Vector3.zero;
			anim.speed = 1;
		}

		anim.SetFloat("magnitude", speed.magnitude);

		if (anim.applyRootMotion)
			transform.position =  anim.rootPosition;
		else
			body.velocity = new Vector3 (speed.x, body.velocity.y, speed.z);

		AdjustRigidbodyForward(body, direction, cam.transform.forward, 20f);

		if (isJumping) {
			if (Time.time > jumpTime + 0.2f) {
				anim.SetTrigger("jump");
				anim.applyRootMotion = false;
				body.velocity = new Vector3 (body.velocity.x, jumpHeight, body.velocity.y);
				jumpTime = Time.time;
			}
		}

		anim.SetBool("grounded", isGrounded);

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

	public static void AdjustRigidbodyForward(Rigidbody body, Vector3 direction, Vector3 camForward, float speed)
    {
        //Only rotate the body when there is motion
        if (direction.magnitude > 0)
        {
            //The direction of movement
            Vector3 moveForward = new Vector3(direction.x, 0, direction.z).normalized, forward;

            if (Vector3.Dot(moveForward, camForward) >= 1)
                //If the body is moving in the direction of the camera is pointing
                forward = camForward;
            else
                //Or it is not going in the direction of the camera
                forward = moveForward;
            //Smooth the direction the body is facing to the correct direction
            body.transform.forward = Vector3.Lerp(body.transform.forward, forward, speed * Time.fixedDeltaTime);
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
		anim.applyRootMotion = true;
	}

    void OnCollisionExit(Collision col)
    {
        isGrounded = false;
		anim.applyRootMotion = false;
    }
}
