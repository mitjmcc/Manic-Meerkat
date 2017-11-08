using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	private bool gamePaused;
	private GameObject main;
	private ControllerUI uiHandler;
	private Button resumeButton;

	private bool selectButton;

	void Start () {
		gamePaused = false;
		main = transform.Find ("Main").gameObject;
		uiHandler = GameObject.FindObjectOfType<ControllerUI> ();
		resumeButton = transform.Find ("Main/ResumeButton").GetComponent<Button> ();
	}

	void Update () {
		if (selectButton) {
			uiHandler.selectButton (resumeButton);
			selectButton = false;
			resumeButton.gameObject.GetComponent<Animator> ().SetBool("Highlighted", true);
		}
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Joystick1Button7)) {
			gamePaused = !gamePaused;
			main.SetActive (gamePaused);
			Time.timeScale = gamePaused ? 0 : 1;
			selectButton = gamePaused;
		}
	}

	public void Resume() {
		gamePaused = false;
		main.SetActive (gamePaused);
		Time.timeScale = 1;
	}
}
