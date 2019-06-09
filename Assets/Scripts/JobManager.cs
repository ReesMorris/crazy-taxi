using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobManager : MonoBehaviour {

	public int fareMultiplier;
	public int passengerTime;
	public int killPenaltyAmount;
	public GameObject passengerTimer;
	public GameObject destinationArrow;
	public GameObject destinationPrefab;

	private GameObject[] passengers;
	private GameObject[] roads;
	private GameObject currentPassenger;
	public GameObject CurrentPassenger {
		get {
			return currentPassenger;
		}
	}

	private GameObject destinationPrefab_;
	private GameObject player;
	private PedestrianManager pedestrianManager;
	private AudioSource passengerLeave;
	private AudioSource successSound;
	private AudioSource failSound;
	private NoticesManager noticesManager;
	private MoneyManager moneyManager;
	private TutorialManager tutorialManager;
	private GameModeManager gameModeManager;

	private int timeTaken;
	public int TimeTaken {
		get {
			return timeTaken;
		}
		set {
			timeTaken = value;
		}
	}

	void Start () {
		roads = GameObject.FindGameObjectsWithTag("Road");
		player = GameObject.Find ("Player").gameObject;
		pedestrianManager = GameObject.Find ("GameManager").GetComponent<PedestrianManager> ();
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
		passengerLeave = GameObject.Find ("SFX").transform.Find ("Passenger Leave").GetComponent<AudioSource> ();
		successSound = GameObject.Find ("SFX").transform.Find ("Success").GetComponent<AudioSource> ();
		failSound = GameObject.Find ("SFX").transform.Find ("Failure").GetComponent<AudioSource> ();
		moneyManager = GameObject.Find ("GameManager").GetComponent<MoneyManager> ();
		tutorialManager = GameObject.Find ("GameManager").GetComponent<TutorialManager> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		passengerTimer.GetComponent<Text> ().text = "";

		if (gameModeManager.gameMode == "default" || gameModeManager.gameMode == "countdown" || gameModeManager.gameMode == "collateral") {
			StartCoroutine ("FindNewPassenger");
		} else {
			destinationArrow.SetActive (false);
		}
	}

	void Update() {
		if (destinationPrefab_ != null) {
			destinationArrow.transform.LookAt (destinationPrefab_.transform);
		}
	}

	// Finds a new person who the taxi will need to pick up
	public IEnumerator FindNewPassenger() {
		while (true) {
			if (pedestrianManager.CurrentPeds > 0) {
				passengers = GameObject.FindGameObjectsWithTag ("Customer");
				currentPassenger = passengers [Random.Range (0, passengers.Length)];
				currentPassenger.GetComponent<GuyMovement> ().walking = false;
				StopCoroutine ("FindNewPassenger");
				FindNearestRoad ();
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	// Finds the road nearest to the current passenger
	void FindNearestRoad() {
		// Set the variables and their defaults
		GameObject nearestRoad = roads[0];
		float nearestDistance = Vector3.Distance (currentPassenger.transform.position, roads [0].transform.position);

		// Cycle through each road and compare its distance to the passenger
		for (int i = 0; i < roads.Length; i++) {
			float distance = Vector3.Distance (currentPassenger.transform.position, roads [i].transform.position);

			// If the road is closer to the passenger than the last, set it as the new closest
			if (distance < nearestDistance) {
				nearestDistance = distance;
				nearestRoad = roads [i];
			}
		}

		// Make the passenger look at the road
		currentPassenger.transform.LookAt(nearestRoad.transform.position);
		destinationPrefab_ = Instantiate (destinationPrefab, nearestRoad.transform.position, Quaternion.identity) as GameObject;
		destinationPrefab_.name = "Destination";

		// Set the destination prefab to position itself nearer to the customer, and rotate itself based on the road
		destinationPrefab_.transform.rotation = Quaternion.Euler (destinationPrefab_.transform.rotation.x, nearestRoad.transform.eulerAngles.y - 90f, destinationPrefab_.transform.rotation.z);
		destinationPrefab_.transform.position = Vector3.MoveTowards (destinationPrefab_.transform.position, currentPassenger.transform.position, 12f);

		GameObject.Find ("Player").GetComponent<DestinationCollision> ().State = -1;
	}

	public void SetPassengerDestination() {
		GameObject destination = roads [Random.Range (0, roads.Length)];

		// Rotates and positions the destination towards the passenger in the road
		destinationPrefab_.transform.position = destination.transform.position;
		destinationPrefab_.transform.rotation = Quaternion.Euler (destinationPrefab_.transform.rotation.x, destination.transform.eulerAngles.y - 90f, destinationPrefab_.transform.rotation.z);
		destinationPrefab_.transform.position = Vector3.MoveTowards (destinationPrefab_.transform.position, currentPassenger.transform.position, 12f);

		StopCoroutine ("IncreaseTimeTaken");
		StartCoroutine ("IncreaseTimeTaken");
	}

	// Ends the current mission
	public void EndMission(bool completed) {
		StopCoroutine ("IncreaseTimeTaken");
		passengerTimer.GetComponent<Text> ().text = "";

		if (completed) {
			// Passenger delivered successfully
			passengerLeave.volume = PlayerPrefs.GetFloat ("sfx", 1);
			passengerLeave.Play ();

			if (passengerTime - timeTaken > 0) {
				// If the time left is positive
				noticesManager.ShowMainScreenNotice ("Passenger Delivered", "+ " + moneyManager.moneySymbol + ((passengerTime - timeTaken) * fareMultiplier), new Color(93 / 255, 255 / 255, 0 / 255), successSound);
			} else {
				// If the time left is negative
				noticesManager.ShowMainScreenNotice ("Passenger Refunded", "- " + moneyManager.moneySymbol + (((passengerTime - timeTaken) * fareMultiplier) * -1), new Color(255 / 255, 69 / 255, 69 / 255), failSound);
			}
			moneyManager.ChangeMoney ((passengerTime - timeTaken) * fareMultiplier);
			timeTaken = 0;
		} else {
			// Passenger not delivered successfully
			noticesManager.ShowMainScreenNotice ("Passenger Killed", "- " + moneyManager.moneySymbol + killPenaltyAmount, new Color(255 / 255, 69 / 255, 69 / 255), failSound);
			moneyManager.ChangeMoney (-killPenaltyAmount);
		}

		Destroy(GameObject.Find ("Destination").gameObject);
		GameObject.Find ("Player").transform.Find ("Passenger").gameObject.SetActive (false);
		GameObject.Find ("Player").GetComponent<CarDrive> ().ForceHandbrake = false;
		pedestrianManager.KillPed ();
		StartCoroutine("FindNewPassenger");
	}

	public IEnumerator IncreaseTimeTaken() {
		while (true) {
			timeTaken++;
			passengerTimer.GetComponent<Text> ().text = "FARE: " + moneyManager.moneySymbol + (passengerTime - timeTaken) + " (x" + fareMultiplier + ")";

			if (passengerTime - timeTaken < 0) {
				// Tutorial
				if (!tutorialManager.TutorialIsShown (5)) {
					tutorialManager.ShowTutorial (5);
				}
			}
			yield return new WaitForSeconds (1f);
		}
	}
}
