using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour {

	public int timerDuration;
	public int minTime;
	public int maxTime;
	public GameObject totalPickupsText;
	public List<GameObject> pickups;

	private GameObject[] roads;
	private int timeLeft;
	private GameModeManager gameModeManager;

	private int totalCollected;
	public int TotalCollected {
		get {
			return totalCollected;
		}
		set {
			totalCollected = value;
		}
	}

	// Collect all of the roads
	void Start () {
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		roads = GameObject.FindGameObjectsWithTag("Road");
		totalCollected = 0;

		if (gameModeManager.gameMode == "default" || gameModeManager.gameMode == "collateral") {
			NewPickup ();
		}
	}

	// Coroutine for the countdown
	IEnumerator Countdown() {
		while (timeLeft >= 0) {
			if (timeLeft == 0) {
				SpawnPickup ();
			}
			timeLeft--;
			yield return new WaitForSeconds (1f);
		}
	}

	// Starts the script to spawn a new pickup
	void NewPickup() {
		StopCoroutine ("Countdown");
		timeLeft = Random.Range (minTime, maxTime);
		StartCoroutine ("Countdown");
	}

	// Spawns the pickup after countdown ended
	void SpawnPickup() {
		GameObject road = roads [Random.Range (0, roads.Length - 1)];
		GameObject pickup = pickups [Random.Range (0, pickups.Count - 1)];

		Instantiate (pickup, road.transform.position, Quaternion.identity);

		// Start the process again
		NewPickup ();
	}

	public void ChangeTotalPickups(int amount) {
		int totalShowing = int.Parse (totalPickupsText.GetComponent<Text> ().text.Split(' ')[0]) + amount;

		totalPickupsText.GetComponent<Text> ().text = totalShowing + " (" + totalCollected + ")";
	}
}
