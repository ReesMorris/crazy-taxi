using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject optionsMenu;

	private TutorialManager tutorialManager;
	private GameModeManager gameModeManager;
	private bool paused;
	public bool Paused {
		get {
			return paused;
		}
	}

	private bool waiting;

	void Start() {
		tutorialManager = GameObject.Find ("GameManager").GetComponent<TutorialManager> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		paused = false;
		waiting = false;
	}

	void Update() {
		if (!tutorialManager.tutorialEnabled || !gameModeManager.showTutorial) {
			waiting = false;
		}

		if(Input.GetKeyDown (KeyCode.Escape) && (!tutorialManager.tutorialEnabled || !tutorialManager.TutorialShowing) && !waiting) {
			waiting = true;

			StartCoroutine ("WaitingLoop");
			if (paused) {
				UnPauseGame ();
			} else {
				pauseMenu.SetActive (true);
				PauseGame ();
			}
		}
	}

	public void PauseGame() {
		paused = true;
		Time.timeScale = 0f;
	}

	public void UnPauseGame() {
		paused = false;
		pauseMenu.SetActive (false);
		optionsMenu.SetActive (false);
		Time.timeScale = 1f;
	}

	// This prevents ESC from unpausing and repausing the game instantly.
	IEnumerator WaitingLoop() {
		yield return new WaitForSeconds (0.1f);
		waiting = false;
	}
}