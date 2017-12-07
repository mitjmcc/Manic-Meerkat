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

	IEnumerator restartScene()
	{
		anim.SetTrigger ("Close");
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	IEnumerator endGame()
	{
		anim.SetTrigger ("Close");
		yield return new WaitForSeconds (1);
		Application.Quit ();
	}

	public void restartLevel()
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (restartScene ());
	}

	public void loadLevel(string name)
	{
		if (loading) {
			return;
		}
		loading = true;
		StartCoroutine (loadScene (name));
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
