using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class fadeAwayScript : MonoBehaviour
{
    float duration = 8;
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        if (Time.time > duration)
        {
            Destroy(gameObject);
        }

        Text theText = GetComponent<Text>();
        Color myColor = theText.color;
        float ratio = Time.time / duration;
        myColor.a = Mathf.Lerp(1, 0, ratio);
        GetComponent<Text>().color = myColor;

    }
}
