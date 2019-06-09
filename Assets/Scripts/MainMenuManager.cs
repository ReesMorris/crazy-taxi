using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {

	public AudioSource clickSfx;
	public List<GameObject> modals;

	// Loads level with build index parameter
	public void LoadLevel(int index) {
		// Unpause the game to prevent issues
		Time.timeScale = 1f;

		clickSfx.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		clickSfx.Play ();

		SceneManager.LoadScene (index);
	}

	// Quits the game
	public void QuitGame() {
		Application.Quit();
	}

	// Shows the modal of index which is defined in the public gameobject
	public void ShowModal(int index) {
		for (int i = 0; i < modals.Count; i++) {
			modals [i].SetActive (false);
			if (i == index) {
				modals [index].SetActive (true);
			}
		}

		clickSfx.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		clickSfx.Play ();
	}
}
