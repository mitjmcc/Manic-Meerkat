using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUI : MonoBehaviour {
	public Button selectFirst;

	void Start () {
		if (selectFirst != null) {
			selectFirst.Select ();
		}
	}

	public void selectButton(Button button)
	{
		button.Select ();
	}
}
