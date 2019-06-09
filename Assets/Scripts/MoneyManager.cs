using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

	public string addScoreURL;
	public string moneySymbol;
	public GameObject moneyText;
	public GameObject submitScorePanel;

	private GameModeManager gameModeManager;
	private bool countdownStarted;
	private GameTime gameTime;

	/*
	 * Based on feedback from Alpha 0.3, it was noticed that it is possible to use CheatEngine to change the score variable
	 * This is a simple workaround which sets the score value be multiplied by a random float, but the player will not know
	 * the value of this float, nor will they be able to see this score.
	 * 
	 * Ie. displayed score: 4,  actual score: 4 * 0.7282
	 */
	private float fakeMultiplier;
	private float actualMoney; // money multiplied by multiplier

	private GameObject inputField;
	private float totalMoney; // money displayed to user

	void Start() {
		gameModeManager = GameObject.Find("GameManager").GetComponent<GameModeManager>();
		gameTime = GameObject.Find("GameManager").GetComponent<GameTime>();

		fakeMultiplier = Random.Range(0.1f, 0.9f);
		inputField = submitScorePanel.transform.Find("InputField").transform.Find("Text").gameObject;

		// Countdown game mode
		if (gameModeManager.gameMode == "countdown") {
			ChangeMoney(1000);
		}
	}

	void Update() {
		// Countdown game mode
		if (!countdownStarted && gameModeManager.gameMode == "countdown" && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
			countdownStarted = true;
			StartCoroutine("CountdownMoney");
			StartCoroutine("CountdownTime");
		}
	}

	public float FinalMoney() {
		return Mathf.Ceil(actualMoney / fakeMultiplier);
	}

	public void ChangeMoney(int amount) {
		totalMoney += amount;
		moneyText.GetComponent<Text>().text = moneySymbol + totalMoney.ToString();
		actualMoney += (amount * fakeMultiplier);
	}

	public void SubmitScore() {
		// Make the text show, and make the form hide
		for (int i = 0; i < submitScorePanel.transform.childCount; i++) {
			submitScorePanel.transform.GetChild(i).gameObject.SetActive(!submitScorePanel.transform.GetChild(i).gameObject.activeSelf);
		}

		StartCoroutine(SubmitScoreCoroutine());
	}

	IEnumerator SubmitScoreCoroutine() {
		WWWForm form = new WWWForm();
		form.AddField("name", inputField.GetComponent<Text>().text);
		form.AddField("score", FinalMoney().ToString());

		using(UnityWebRequest www = UnityWebRequest.Post(addScoreURL, form)) {
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError) Debug.Log(www.error);
		}
	}

	// Countdown game mode
	IEnumerator CountdownMoney() {
		while (true) {
			if ((actualMoney / fakeMultiplier) > 1) {
				// Continue
				ChangeMoney(-1);
			} else {
				// Bankrupt
				gameTime.GameOver();
			}

			yield return new WaitForSeconds(0.4f);
		}
	}

	// Countdown game mode
	IEnumerator CountdownTime() {
		while ((actualMoney / fakeMultiplier) > 0) {
			gameTime.IncreaseTime(1);
			yield return new WaitForSeconds(1f);
		}
	}
}