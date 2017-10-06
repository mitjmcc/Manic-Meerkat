using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public float speed;
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
	}
}
