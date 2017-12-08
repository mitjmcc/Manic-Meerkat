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
    public GameObject myBallPrefab;
    public AudioClip death;
    private GameObject enemy;
    public Animator anim;
    private Rigidbody rbody;
    private bool throwing = false;
    public Transform[] waypointSetA;
    public static Boolean enemyDead = false;
    public static List<GameObject> wayPoints;
    public static int score = 0;
    private System.Random rand = new System.Random();

    public Transform[] waypointSetB;

    public Transform[] waypointSetC;

    public Transform waypointE;

    public Vector3 waypointF;

    public enum State
    {
        FOLLOW,
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

        aiSteer.waypointLoop = false;
        aiSteer.stopAtNextWaypoint = false;
        transitionToStateFollow();

    }


    void transitionToStateFollow()
    {

        state = State.FOLLOW;

        aiSteer.setWayPoints(waypointSetA);

        aiSteer.useNavMeshPathPlanning = true;


    }


    void transitionToStateRampage()
    {

        state = State.RAMPAGE;

        aiSteer.setWayPoints(waypointSetB);

        aiSteer.useNavMeshPathPlanning = true;
    }


    void transitionToStateVictory()
    {

        state = State.VICTORY;

        aiSteer.useNavMeshPathPlanning = true;

    }

    void transitionToStateDeath()
    {
        state = State.DEATH;

    }

    void transitionToTimer()
    {
        state = State.TIMER;
        deltat = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.FOLLOW:
                float threshold = 2.5f;
                if (Math.Abs(player.transform.position.x - rbody.position.x) < threshold &&
                Math.Abs(player.transform.position.y - rbody.position.y) < threshold &&
                Math.Abs(player.transform.position.z - rbody.position.z) < threshold)
                {
                    enemyDead = true;
                    AudioSource.PlayClipAtPoint(this.death, this.transform.position);
                }

                if (enemyDead)
                {
                    transitionToStateVictory();
                    enemyDead = false;
                }
                transitionToStateFollow();
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                //transform.LookAt(player.transform);
                aiSteer.setWayPoint(player.transform);
                break;

            case State.RAMPAGE:
                if (aiSteer.waypointsComplete())
                    transitionToStateRampage();
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
}
