using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStates : MonoBehaviour
{

    public GameObject thief;
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

        A,
        B,
        C,
        D,
        E,
        F

    }



    public State state = State.A;

    public float waitTime = 5f;

    protected float beginWaitTime;


   // AINavSteeringController aiSteer;
    NavMeshAgent agent;


    // Use this for initialization
    void Start()
    {
        wayPoints = new List<GameObject>();
        rbody = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        //aiSteer = GetComponent<AINavSteeringController>();

        agent = GetComponent<NavMeshAgent>();

        Debug.Log("NavMesh:avoidancePredictionTime(default): " + NavMesh.avoidancePredictionTime);

        //NavMesh.avoidancePredictionTime = 4f;

        //aiSteer.Init();

        //aiSteer.waypointLoop = false;
        //aiSteer.stopAtNextWaypoint = false;
        pickAGoal();

    }

    void pickAGoal()
    {
        int num = rand.Next(1, 5);
        if (num == 1)
        {
            transitionToStateA();
        }
        else if (num == 2)
        {
            transitionToStateB();
        }
        else if (num == 3)
        {
            transitionToStateC();
        }
        else
        {
            transitionToStateD();
        }
    }
    void ThrowBall()
    {
        Transform handPos = this.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
        GameObject ball;
        ball = Instantiate(myBallPrefab, handPos.position, handPos.rotation) as GameObject;

        ball.GetComponent<Rigidbody>().isKinematic = false;
        float dt = 0.02f;
        NavMeshAgent mesh = enemy.GetComponent<NavMeshAgent>();
        Vector3 vel = mesh.velocity;
        Vector3 ri = mesh.transform.position;
        Vector3 rf = ri + vel * dt;
        RaycastHit hit = new RaycastHit();
        bool isHit = Physics.Raycast(ball.transform.position, ri, out hit);
        Vector3 target = rf.normalized;

        if (hit.transform.gameObject.tag == "ball")
        {
            target = hit.normal;
        }

        //Givens
        Vector3 ballRi = ball.transform.position; //Pbi
        float ballSpeed = 20f; //Sb
        Vector3 targetRi = enemy.transform.position; //Pti
        Vector3 targetVi = mesh.velocity; //Vti
        float targetAi = mesh.acceleration; //At
        float St = targetVi.magnitude;

        float length = ballSpeed * dt;
        double theta = Math.Acos((Vector3.Dot((ballRi - targetRi).normalized, (targetVi.normalized))));
        float A = (targetRi - ballRi).magnitude;
        float B = (targetVi * dt).magnitude;
        float C = ballSpeed * dt;
        float D = (targetRi - ballRi).magnitude;
        double t = (-2 * D * St * Math.Cos(theta) + Math.Sqrt(Math.Pow((2 * D * St * Math.Cos(theta)), 2) + 4 * (Math.Pow(ballSpeed, 2) - Math.Pow(St, 2) * Math.Pow(D, 2)) / (2 * (Math.Pow(ballSpeed, 2) - Math.Pow(St, 2)))));
        Vector3 dir = targetVi + ((targetRi - ballRi) / (float)(t));
        ball.GetComponent<Rigidbody>().AddForce(dir.normalized * ballSpeed, ForceMode.VelocityChange);
    }

    bool CloseEnoughToThrow(Transform obj)
    {
        float threshold = 4f;
        return (Math.Abs(obj.position.x - rbody.position.x) < threshold &&
                Math.Abs(obj.position.y - rbody.position.y) < threshold &&
                Math.Abs(obj.position.z - rbody.position.z) < threshold);
    }

    void transitionToStateA()
    {
        resetLevel();
        enemyDead = false;
        throwing = false;
        anim.SetBool("throw", false);
        print("Transition to state A");

        state = State.A;

       // aiSteer.setWayPoints(waypointSetA);

        //aiSteer.useNavMeshPathPlanning = true;


    }


    void transitionToStateB()
    {
        resetLevel();
        enemyDead = false;
        throwing = false;
        anim.SetBool("throw", false);

        print("Transition to state B");

        state = State.B;

        //aiSteer.setWayPoints(waypointSetB);

        //aiSteer.useNavMeshPathPlanning = true;
    }


    void transitionToStateC()
    {
        resetLevel();
        enemyDead = false;
        throwing = false;
        anim.SetBool("throw", false);

        print("Transition to state C");

        enemy = Instantiate(thief) as GameObject;

        state = State.C;

        //aiSteer.setWayPoint(enemy.transform);

        //aiSteer.useNavMeshPathPlanning = true;

    }

    void transitionToStateD()
    {
        resetLevel();
        enemyDead = false;
        throwing = false;
        anim.SetBool("throw", false);
        enemy = Instantiate(thief) as GameObject;
        state = State.D;
        //aiSteer.setWayPoint(enemy.transform);
        //aiSteer.useNavMeshPathPlanning = true;

    }

    void transitionToStateE()
    {
        throwing = false;
        anim.SetBool("throw", false);
        resetLevel();
        transitionToStateA();
        state = State.A;
    }

    void resetLevel()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("destroy");
        foreach (GameObject o in wayPoints)
        {
            //Instantiate(o);
            o.SetActive(true);
            Debug.Log("HELLOOOOOOOOO");
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            //case State.A:
            //    if (aiSteer.waypointsComplete())
            //        pickAGoal();
            //    break;

            //case State.B:
            //    if (aiSteer.waypointsComplete())
            //        pickAGoal();
            //    break;

            //case State.C:

            //    if (throwing)
            //    {
            //        throwing = false;
            //        anim.SetBool("throw", false);
            //    }
            //    if (CloseEnoughToThrow(enemy.transform))
            //    {
            //        throwing = true;
            //        anim.SetBool("throw", true);
            //    }
            //    if (enemyDead)
            //        pickAGoal();
            //    aiSteer.setWayPoint(enemy.transform);
            //    break;

            case State.D:
                float threshold = 2f;
                if (Math.Abs(enemy.transform.position.x - rbody.position.x) < threshold &&
                Math.Abs(enemy.transform.position.y - rbody.position.y) < threshold &&
                Math.Abs(enemy.transform.position.z - rbody.position.z) < threshold)
                {
                    enemyDead = true;
                    //EventManager.TriggerEvent<FoodCollisionEvent, Vector3>(enemy.transform.position);
                    enemy.SetActive(false);
                    AudioSource.PlayClipAtPoint(this.death, this.transform.position);
                }
                if (enemyDead)
                    pickAGoal();
                //aiSteer.setWayPoint(enemy.transform);
                break;

            default:

                print("Weird?");
                break;
        }


    }
}
