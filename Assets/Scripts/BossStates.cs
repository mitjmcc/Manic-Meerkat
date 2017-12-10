using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AINavSteeringController))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossStates : MonoBehaviour
{

    public GameObject player;
    public AudioClip death;
    private GameObject enemy;
    public Animator anim;
    private Rigidbody rbody;
    public Transform[] waypointSetA;
    public static Boolean enemyDead = false;
    public static List<GameObject> wayPoints;
    public static int score = 0;
    private System.Random rand = new System.Random();
    private float angryTimer = 0f;

    public Transform[] waypointSetB;

    public Transform[] waypointSetC;

    public Transform waypointE;

    public Vector3 waypointF;

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


    AINavSteeringController aiSteer;
    NavMeshAgent agent;


    // Use this for initialization
    void Start()
    {
        wayPoints = new List<GameObject>();
        rbody = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        aiSteer = GetComponent<AINavSteeringController>();

        agent = GetComponent<NavMeshAgent>();

        aiSteer.Init();

        aiSteer.enabled = true;
        agent.enabled = true;

        aiSteer.waypointLoop = false;
        aiSteer.stopAtNextWaypoint = false;
        //aiSteer.useNavMeshPathPlanning = true;
        transitionToStateFollow();
        Debug.Log(transform.position);
        Debug.Log(player.transform.position);

    }


    void transitionToStateFollow()
    {

        state = State.FOLLOW;

        aiSteer.clearWaypoints();
        aiSteer.setWayPoint(player.transform);

        //aiSteer.useNavMeshPathPlanning = true;

        angryTimer = 0f;

        Debug.Log(state);
    }

    
    void transitionToStateAngry()
    {
        state = State.ANGRY;

        //aiSteer.enabled = false;
        //aiSteer.useNavMeshPathPlanning = false;
        //agent.enabled = false;

        Debug.Log(state);
    }


    void transitionToStateRampage()
    {

        state = State.RAMPAGE;
        
        //aiSteer.setWayPoint(player.transform.position);


        agent.enabled = true;
        agent.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        //aiSteer.enabled = true;
        //aiSteer.useNavMeshPathPlanning = true;
        Debug.Log(state);
    }


    void transitionToStateVictory()
    {
        Debug.Log("Player died");
        state = State.VICTORY;
        anim.SetTrigger("playerDead");
        //aiSteer.enabled = false;
        //aiSteer.useNavMeshPathPlanning = false;

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
                if (Math.Abs(player.transform.position.x - rbody.position.x) < threshold &&
                Math.Abs(player.transform.position.y - rbody.position.y) < threshold &&
                Math.Abs(player.transform.position.z - rbody.position.z) < threshold)
                {
                    enemyDead = true;
                    AudioSource.PlayClipAtPoint(this.death, this.transform.position);
                }

                if (angryTimer >= 50f)
                {
                    transitionToStateAngry();
                }

                if (enemyDead)
                {
                    transitionToStateVictory();
                    enemyDead = false;
                }
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                //aiSteer.clearWaypoints();
                aiSteer.setWayPoint(player.transform);
                break;

            case State.ANGRY:
                anim.SetTrigger("getMad");
                transitionToStateRampage();
                break;

            case State.RAMPAGE:
                if (aiSteer.waypointsComplete())
                    transitionToStateFollow();
                break;

            case State.DEATH:
                break;
            case State.VICTORY:
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
