using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossStates : MonoBehaviour
{

    public GameObject player;
    public AudioClip death;
    private GameObject enemy;
    public Animator anim;
    private Rigidbody rbody;
    public static Boolean enemyDead = false;
    public static int score = 0;
    private System.Random rand = new System.Random();
    private float angryTimer = 0f;

    public enum State
    {
        FOLLOW,
        ANGRY,
        RAMPAGE,
        DEATH,
        VICTORY,
        TIMER
    }



    public State state = State.FOLLOW;

    public float waitTime = 5f;

    protected float deltat;

    NavMeshAgent agent;


    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;
		agent.updateRotation = false;

        transitionToStateFollow();
        Debug.Log(transform.position);
        Debug.Log(player.transform.position);

    }


    void transitionToStateFollow()
    {

        state = State.FOLLOW;

        angryTimer = 0f;

        Debug.Log(state);
    }

    
    void transitionToStateAngry()
    {
        state = State.ANGRY;

        Debug.Log(state);
    }


    void transitionToStateRampage()
    {
        state = State.RAMPAGE;

        Debug.Log(state);
    }


    void transitionToStateVictory()
    {
        Debug.Log("Player died");
        state = State.VICTORY;
        anim.SetTrigger("playerDead");

        Debug.Log(state);
    }

    void transitionToStateDeath()
    {
        state = State.DEATH;

        Debug.Log(state);
    }

    void transitionToTimer()
    {
        state = State.TIMER;
        deltat = 0f;
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
		case State.FOLLOW:
			float threshold = 2.5f;
			angryTimer += Time.deltaTime;
			if (Math.Abs (player.transform.position.x - rbody.position.x) < threshold &&
			    Math.Abs (player.transform.position.y - rbody.position.y) < threshold &&
			    Math.Abs (player.transform.position.z - rbody.position.z) < threshold) {
				enemyDead = true;
				AudioSource.PlayClipAtPoint (this.death, this.transform.position);
			}

			if (angryTimer >= 50f) {
				transitionToStateAngry ();
			}

			if (enemyDead) {
				transitionToStateVictory ();
				enemyDead = false;
			}

			anim.SetFloat ("magnitude", 0.7f);

			agent.SetDestination (player.transform.position);

			//Agent turns to walk along NavMeshAgent's desired path
			transform.LookAt (transform.position + new Vector3 (agent.desiredVelocity.x, 0, agent.desiredVelocity.z));
			agent.nextPosition = transform.position;
            	break;

            case State.ANGRY:
				anim.SetFloat ("magnitude", 0.0f);
                anim.SetTrigger("getMad");
                transitionToStateRampage();
                break;

            case State.RAMPAGE:
				anim.SetFloat ("magnitude", 1.0f);

				//Agent turns to walk straight toward the player
				transform.LookAt(player.transform.position);
                break;

            case State.DEATH:
				anim.SetFloat ("magnitude", 0.0f);
                break;
            case State.VICTORY:
				anim.SetFloat ("magnitude", 0.0f);
                enemyDead = false;
                transitionToTimer();
                break;

            case State.TIMER:
                deltat += Time.deltaTime;
                if (deltat > waitTime)
                {
                    CharacterController pl = player.gameObject.GetComponent<CharacterController>();
                    pl.Death();
                    transitionToStateFollow();
                }
                break;

            default:

                print("Weird?");
                break;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("tntbox") && state.Equals(State.RAMPAGE))
        {
            Debug.Log("Boom");
        }
    }
}
