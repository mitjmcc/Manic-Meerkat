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
        AWAITDEATH
    }



    public State state = State.FOLLOW;

    public float waitTime = 5f;

    protected float beginWaitTime;


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
        transitionToStateA();

    }


    void transitionToStateA()
    {

        state = State.FOLLOW;

        aiSteer.setWayPoints(waypointSetA);

        aiSteer.useNavMeshPathPlanning = true;


    }


    void transitionToStateB()
    {

        state = State.RAMPAGE;

        aiSteer.setWayPoints(waypointSetB);

        aiSteer.useNavMeshPathPlanning = true;
    }


    void transitionToStateC()
    {

        state = State.AWAITDEATH;

        aiSteer.setWayPoint(enemy.transform);

        aiSteer.useNavMeshPathPlanning = true;

    }


    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.FOLLOW:
                float threshold = 2f;
                if (Math.Abs(player.transform.position.x - rbody.position.x) < threshold &&
                Math.Abs(player.transform.position.y - rbody.position.y) < threshold &&
                Math.Abs(player.transform.position.z - rbody.position.z) < threshold)
                {
                    enemyDead = true;
                    enemy.SetActive(false);
                    AudioSource.PlayClipAtPoint(this.death, this.transform.position);
                }
                if (enemyDead)
                    transitionToStateA();
                Debug.Log(aiSteer.transform.position.ToString());
                transform.LookAt(player.transform);
                aiSteer.setWayPoint(player.transform);
                break;

            case State.RAMPAGE:
                if (aiSteer.waypointsComplete())
                    transitionToStateB();
                break;

            case State.AWAITDEATH:
                if (aiSteer.waypointsComplete())
                    transitionToStateC();
                break;

            default:

                print("Weird?");
                break;
        }


    }
}
