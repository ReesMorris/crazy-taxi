using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour {

	public int startTime;
	public AudioSource countdown;
	public AudioSource gameover;
	public GameObject timeText;
	public GameObject gameOverModal;
	public GameObject finalscoreText;
	public GameObject timeLastedText;

	private int timeLeft;
	private PauseManager pauseManager;
	private MoneyManager moneyManager;
	private GameModeManager gameModeManager;
	private GameTime gameTime;
	private bool started;

	void Start () {
		timeLeft = startTime;
		started = false;
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
		moneyManager = GameObject.Find ("GameManager").GetComponent<MoneyManager> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		gameTime = GameObject.Find ("GameManager").GetComponent<GameTime> ();

		if (gameModeManager.gameMode == "explore" || gameModeManager.gameMode == "countdown") {
			timeLeft = 0;
		}
	}

	void Update() {
		// Wait until the player begins to drive before starting the game.
		// Not needed if the tutorial is active, but it stops players losing time if the tutorial is disabled and they aren't at their computer.
		if (!started && (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) && (gameModeManager.gameMode == "default" || gameModeManager.gameMode == "collateral")) {
			started = true;
			StartCoroutine ("Countdown");
		}

		timeText.GetComponent<Text> ().text = timeLeft.ToString();
	}

	// Count down every second
	IEnumerator Countdown() {
		while (true) {
			if (timeLeft > 1) {
				DecreaseTime (1);
				if (timeLeft < 11) {
					// Play a sound effect for the last 10 seconds
					countdown.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
					countdown.Play ();
				}
			} else {
				// No seconds left, game is over
				GameOver();
			}

			yield return new WaitForSeconds (1f);
		}
	}

	public void GameOver() {
		gameover.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		gameover.Play ();

		pauseManager.PauseGame ();
		finalscoreText.GetComponent<Text> ().text = "Final Score: " + moneyManager.moneySymbol + moneyManager.FinalMoney();
		timeLastedText.GetComponent<Text> ().text = "Time Lasted: " + gameTime.timeLeft + " seconds";
		gameOverModal.SetActive (true);
	}

	// Increase the time left by amount
	public void IncreaseTime(int amount) {
		timeLeft += amount;
	}

	// Decrease the time left by amount
	public void DecreaseTime(int amount) {
		timeLeft -= amount;
	}
}
