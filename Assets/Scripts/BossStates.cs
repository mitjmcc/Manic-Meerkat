using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossStates : MonoBehaviour
{
    #region public variables
    public GameObject player;
    public AudioClip death;
    #endregion

    #region private variables
    private GameObject enemy;
    private Animator anim;
    private Rigidbody rbody;
    private NavMeshAgent agent;
    private State state;
    private float angryTimer = 0f;
    private int health;
    private float angryTrigger = 8f;
    private float sadTrigger = 5f;
    private float waitTime = 3f;
    protected float deltat;
    #endregion

    public enum State
    {
        FOLLOW,
        ANGRY,
        RAMPAGE,
        MISSED,
        DEATH,
        VICTORY,
		HIT,
        TIMER
    }


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

        health = 3;
        state = State.FOLLOW;
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

        angryTimer = 0f;

        Debug.Log(state);
    }


    void transitionToStateMissed()
    {
        state = State.MISSED;

        anim.SetTrigger("getSad");
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
        anim.SetTrigger("isDead");

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
            // Kills the player and causes the player to scream if the boss touches them
			if (Math.Abs (player.transform.position.x - rbody.position.x) < threshold &&
			    Math.Abs (player.transform.position.y - rbody.position.y) < threshold &&
			    Math.Abs (player.transform.position.z - rbody.position.z) < threshold) {
                    transitionToStateVictory();
                    AudioSource.PlayClipAtPoint (this.death, this.transform.position);
			}

			if (angryTimer >= angryTrigger) {
				transitionToStateAngry ();
			}

			anim.SetFloat ("magnitude", health == 1 ? 1.0f : 0.9f);

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
                angryTimer += Time.deltaTime;
                if (angryTimer >= sadTrigger)
                {
                    transitionToStateMissed();
                }

                //Agent turns to walk straight toward the player
                transform.LookAt(player.transform.position);
                break;

            case State.MISSED:
                transitionToStateFollow();
                break;

            case State.DEATH:
				anim.SetFloat ("magnitude", 0.0f);
                transitionToTimer();
                break;

            case State.VICTORY:
				anim.SetFloat ("magnitude", 0.0f);
                transitionToTimer();
                break;

			case State.HIT:
				anim.SetFloat ("magnitude", 0.0f);
				agent.SetDestination (transform.position);
				angryTimer += Time.deltaTime;
				if (angryTimer >= 3) {
					transitionToStateFollow ();
				}
				break;

            case State.TIMER:
				if (health > 0)
				{
					CharacterController pl = player.gameObject.GetComponent<CharacterController>();
					pl.Death();
					transitionToStateFollow();
				}
				else
				{
					GameObject.FindObjectOfType<LevelChanger>().loadLevel("MainMenu");
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
            collision.gameObject.GetComponent<TNTCrate>().Explode();
            health--;
            if (health <= 0)
            {
                transitionToStateDeath();
            }
            else
            {
                anim.SetTrigger("isHit");
				angryTimer = 0;
				state = State.HIT;
                //transitionToStateFollow();
            }
            Debug.Log(health);
        }
    }
}
