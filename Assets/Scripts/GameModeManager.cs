using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

	// Defines the type that the gamemode is
	public bool showTutorial;
	public string gameMode;

	public GameObject submitScoreTitle;
	public GameObject submitScorePanel;
	public GameObject finalScoreText;
	public GameObject timeLastedText;

	void Start() {
		gameMode = PlayerPrefs.GetString ("mode", "default");

		switch (gameMode) {
		case "default":
			showTutorial = true;
			if (finalScoreText != null) {
				finalScoreText.SetActive (true);
				timeLastedText.SetActive (false);
			}
			break;
		case "explore":
			showTutorial = false;
			if (finalScoreText != null) {
				submitScoreTitle.SetActive (false);
				finalScoreText.SetActive (false);
				timeLastedText.SetActive (false);
			}
			Destroy (submitScorePanel);
			break;
		case "countdown":
			showTutorial = false;
			if (finalScoreText != null) {
				submitScoreTitle.SetActive (false);
				finalScoreText.SetActive (false);
				timeLastedText.SetActive (true);
			}
			Destroy (submitScorePanel);
			break;
		case "collateral":
			showTutorial = false;
			if (finalScoreText != null) {
				submitScoreTitle.SetActive (false);
				finalScoreText.SetActive (true);
				timeLastedText.SetActive (false);
			}
			Destroy (submitScorePanel);
			break;
		}
	}

	public void SetMode(string mode) {
		PlayerPrefs.SetString ("mode", mode);
	}
}
