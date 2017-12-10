using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
	private Animator anim;
	private bool loading;

	IEnumerator loadScene(string name)
	{
		anim.SetTrigger ("Close");
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene (name);
	}

	IEnumerator winScene(string name)
	{
		anim.SetTrigger ("Won");
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (name);
	}

	IEnumerator restartScene(bool fromMenu)
	{
		if (fromMenu) {
			anim.SetTrigger ("Close");
		} else {
			anim.SetTrigger ("Death");
		}
		yield return new WaitForSeconds (2);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	IEnumerator endGame()
	{
		anim.SetTrigger ("Close");
		yield return new WaitForSeconds (1);
		Application.Quit ();
	}

	public void restartLevel(bool fromMenu)
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (restartScene (fromMenu));
	}

	public void loadLevel(string name)
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (loadScene (name));
	}

	public void winLevel(string name)
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (winScene (name));
	}

	public void quitGame()
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (endGame ());
	}

	void Start ()
	{
		loading = false;
		anim = GetComponent<Animator> ();
	}
}
