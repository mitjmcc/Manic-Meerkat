using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public GameObject[] menu;
	public GameObject[] credits;
	public Animator cameraAnimator;
	public ControllerUI uiHandler;
	public Button backButton;
	public Button creditsButton;

	public AudioClip whoosh;

	private AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource> ();
	}

	public void toCredits()
	{
		cameraAnimator.SetBool ("ViewingCredits", true);
		foreach (GameObject obj in menu) {
			obj.SetActive (false);
		}
		foreach (GameObject obj in credits) {
			obj.SetActive (true);
		}
		uiHandler.selectButton (backButton);
		source.clip = whoosh;
		source.Play ();
	}

	public void toMenu()
	{
		cameraAnimator.SetBool ("ViewingCredits", false);
		foreach (GameObject obj in menu) {
			obj.SetActive (true);
		}
		foreach (GameObject obj in credits) {
			obj.SetActive (false);
		}
		uiHandler.selectButton (creditsButton);
		source.clip = whoosh;
		source.Play ();
	}
}
