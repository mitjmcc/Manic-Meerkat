using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAnimator : MonoBehaviour {
	private Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();
	}

	public void setBoolOn(string name)
	{
		animator.SetBool (name, true);
	}

	public void setBoolOff(string name)
	{
		animator.SetBool (name, false);
	}

}
