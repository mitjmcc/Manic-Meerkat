using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUI : MonoBehaviour {
	public Button selectFirst;

	void Start () {
		string[] names = Input.GetJoystickNames ();
		foreach (string name in names) {
			if (name.IndexOf ("Gamepad") > -1) {
				selectFirst.Select ();
				break;
			}
		}
	}

	public void selectButton(Button button)
	{
		string[] names = Input.GetJoystickNames ();
		foreach (string name in names) {
			if (name.IndexOf ("Gamepad") > -1) {
				button.Select ();
				return;
			}
		}
	}
}
