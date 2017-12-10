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
    public PhysicMaterial groundMaterial;
    public PhysicMaterial jumpMaterial;
    public Transform groundPlane;
    public Transform[] spawnPoints;
    public TrailRenderer trail;
    #endregion

    #region PrivateVariables
    Rigidbody body;
    Transform model;
    Animator anim;
    Vector3 speed;
    Vector3 forward;

    float x, y, z, jumpTime;
    bool isGrounded;
    bool isTouchingWall;
    bool isJumping;
    bool isBashing;
    bool isDead;
    CapsuleCollider collider;

	private AudioSource footstep;
	private AudioSource jump;
	private AudioSource bash;
	private AudioSource death;
	private AudioSource bashsource;
    #endregion

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
		footstep = transform.Find ("Footstep").GetComponent<AudioSource> ();
		jump = transform.Find ("Jump").GetComponent<AudioSource> ();
		death = transform.Find ("Death").GetComponent<AudioSource> ();
		bashsource = transform.Find ("Bash").GetComponent<AudioSource> ();
		bash = GetComponent<AudioSource> ();
    }

    void FixedUpdate() {
		if (isDead) {
			return;
		}

        isJumping = Input.GetButton("Jump") && isGrounded;
		isBashing = Input.GetButton("Fire1") && !bash.isPlaying;

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("kick")) {
            isGrounded = false;
			isBashing = false;
            BashAttack();
            trail.enabled = true;
        } else {
            trail.enabled = false;
        }

        x = (isGrounded) ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical") / airControlFactor;
        z = (isGrounded) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal") / airControlFactor;

        Vector3 direction = cam.transform.TransformVector(new Vector3(z, 0, x));

        if (direction.magnitude > 0) {
            speed = direction.normalized * moveSpeed;
        } else {
            speed = Vector3.zero;
            anim.speed = 1;
        }

        anim.SetFloat("magnitude", Mathf.Lerp(anim.GetFloat("magnitude"), Mathf.Min(1, direction.magnitude), Time.fixedDeltaTime * 12f));

        if (!isGrounded) {
            body.velocity = new Vector3(speed.x, body.velocity.y, speed.z);
        }

        AdjustRigidbodyForward(direction, cam.transform.forward, 20f);

        if (isJumping) {
            if (Time.time > jumpTime + 0.2f) {
                anim.SetTrigger("jump");
				jump.Play ();
                body.velocity = new Vector3(body.velocity.x, jumpHeight, body.velocity.y);
                jumpTime = Time.time;
            }
        }

        anim.SetBool("grounded", isGrounded);

        if (isBashing) {
			bash.Play();
            anim.SetTrigger("bash");
        }

        if (transform.position.y < groundPlane.position.y) {
            Death();
        }
    }

    void BashAttack() {
        foreach (Collider c in Physics.OverlapSphere(transform.position, 2)) {
            IBashable bashable = c.gameObject.GetComponent<IBashable>();
            if (bashable != null) {
                bashable.OnBash();
				bashsource.Play ();
				TNTCrate tnt = c.gameObject.GetComponent<TNTCrate>();
				if (tnt != null)
					Death();
            }
        }
    }

    public void Death() {
        if (!isDead) {
            anim.SetTrigger("dying");
			death.Play ();
            GameObject.FindObjectOfType<LevelChanger>().restartLevel(false);
        }
        isDead = true;
    }

    public void FootStep() {
        footstep.Play();
    }

    void OnAnimatorMove() {
        if (isGrounded) {
            transform.position = transform.position + anim.deltaPosition * 1.4f;
        }
    }

    void AdjustRigidbodyForward(Vector3 direction, Vector3 camForward, float speed) {
        // Only rotate the body when there is motion
        if (direction.magnitude > 0) {
            // Adjust the direction of movement
            body.transform.forward = new Vector3(direction.x, 0, direction.z).normalized;
        }
    }

    void OnCollisionEnter(Collision col) {
        IJumpable jumpable = col.gameObject.GetComponent<IJumpable>();
        TNTCrate tnt = col.gameObject.GetComponent<TNTCrate>();
        if (jumpable != null) {
            foreach (ContactPoint contact in col.contacts) {
                // Landing on object
                if (body.transform.position.y > col.gameObject.transform.position.y
					&& col.relativeVelocity.y > 0.1f) {
                    jumpable.OnJump();
					bashsource.Play ();
                    if (tnt == null)
                        body.velocity = new Vector3(body.velocity.x, boxJumpHeight, body.velocity.y);
                    else
                        Death();
                    break;
                    // Below the object
                } else if (body.transform.position.y < col.gameObject.transform.position.y
					&& col.relativeVelocity.y < -0.1f) {
                    jumpable.OnJump();
                    if (tnt == null)
                        body.velocity = new Vector3(body.velocity.x, -boxJumpHeight, body.velocity.y);
                    else
                        Death();
                    break;
                    // Hit TNT Crate
                } else if (tnt != null) {
                    tnt.Explode();
                    Death();
                    // Hit by an enemy
                } else if (col.gameObject.CompareTag("Enemy")
					&& !anim.GetCurrentAnimatorStateInfo(0).IsName("kick")) {
                    Death();
                }
            }
        }
    }

    void OnTriggerEnter(Collider col) {
        ICollectable collectable = col.gameObject.GetComponent<ICollectable>();
        if (collectable != null) {
            collectable.OnCollect();
        } else if (col.gameObject.CompareTag("Enemy")
			&& !anim.GetCurrentAnimatorStateInfo(0).IsName("kick")) {
            Death();
        }
    }

    void OnCollisionStay(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            collider.material = groundMaterial;
        } else {
            isTouchingWall = true;
        }
    }

    void OnCollisionExit(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            isGrounded = false;
            collider.material = jumpMaterial;
        } else {
            isTouchingWall = false;
        }
    }
}
