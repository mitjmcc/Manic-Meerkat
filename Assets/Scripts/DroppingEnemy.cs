using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingEnemy : MonoBehaviour, IJumpable, IBashable
{

    public Transform[] points = new Transform[2];
    public float speed;
    Vector3 target;
    int index;
    public bool dropping;
    float timer = 0f;
    Vector3 dy = new Vector3(0, -1f ,0);
    Vector3 orig;
    bool reset;

    void Start()
    {
        target = points[index].position;
        dropping = false;
        reset = false;
        orig = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - target).sqrMagnitude < 0.0001 && !reset)
        {
            dropping = true;
        }
        if (dropping)
        {
            timer += Time.deltaTime;
            if (timer >= 0.75f)
            {
                dropNow();
            }
        }
        if (reset)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                transform.position = orig;
                reset = false;
                timer = 0f;
            }
        }
    }

    void dropNow()
    {
        transform.position = transform.position + dy;
        if (transform.position.y <= getGround())
        {
            dropping = false;
            reset = true;
            timer = 0f;
        }
    }
    public float getGround()
    {
        return 4f;
    }
    public void OnJump()
    {
        gameObject.SetActive(false);
    }

    public void OnBash()
    {
        gameObject.SetActive(false);
    }
}
