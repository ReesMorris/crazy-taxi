using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public GameObject tutorialParent;
	public bool tutorialEnabled;

	private bool tutorialShowing;
	public bool TutorialShowing {
		get {
			return tutorialShowing;
		}
	}

	private PauseManager pauseManager;
	private List<bool> tutorialShown;
	private GameModeManager gameModeManager;

	void Start() {
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();

		tutorialEnabled = PlayerPrefs.GetInt ("tutorial", 1) == 1 ? true : false;
		tutorialShowing = false;
		if (tutorialEnabled && gameModeManager.showTutorial) {
			StartCoroutine (TutorialNotShowing ());
			pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
			tutorialShown = new List<bool> ();
			for (int i = 0; i < tutorialParent.transform.childCount; i++) {
				tutorialShown.Add (false);
			}

			ShowTutorial (0);
		}
	}

	void Update() {
		tutorialEnabled = PlayerPrefs.GetInt ("tutorial", 1) == 1 ? true : false;
		if (tutorialEnabled && gameModeManager.showTutorial) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				// There is a small chance that two tutorials will show at once
				// And so, the script might not think a tutorial is showing when it is.
				// This prevents this from happening by running every time ESC is pressed.
				HideTutorial ();
			}
		}
	}

	public void ShowTutorial(int index) {
		if (tutorialEnabled && gameModeManager.showTutorial) {
			if (index < tutorialParent.transform.childCount) {
				for (int i = 0; i < tutorialParent.transform.childCount; i++) {
					tutorialParent.transform.GetChild (i).gameObject.SetActive (false);
					if (i == index) {
						tutorialParent.transform.GetChild (i).gameObject.SetActive (true);
					}
				}
				tutorialShowing = true;
				tutorialShown [index] = true;
			} else {
				Debug.LogError ("Tutorial index does not exist!");
			}
			pauseManager.PauseGame ();
		}
	}

	public void HideTutorial() {
		if (tutorialEnabled && gameModeManager.showTutorial) {
			for (int i = 0; i < tutorialParent.transform.childCount; i++) {
				tutorialParent.transform.GetChild (i).gameObject.SetActive (false);
			}
			StartCoroutine (TutorialNotShowing ());
			pauseManager.UnPauseGame ();
		}
	}

	public bool TutorialIsShown(int index) {
		if (tutorialEnabled && gameModeManager.showTutorial) {
			if (tutorialShown.Count > index && !tutorialShown[index]) {
				return false;
			} else {
				return true;
			}
		} else {
			return true;
		}
	}

	// This is purely because the escape menu doesn't work without a delay
	IEnumerator TutorialNotShowing() {
		yield return new WaitForSeconds(0.1f);
		tutorialShowing = false;
	}
}