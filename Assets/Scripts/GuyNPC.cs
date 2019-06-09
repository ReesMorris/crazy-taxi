using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyNPC : MonoBehaviour {

	public List<Texture> textures;
	public GameObject torso;
	public GameObject deathParticles;
	public GameObject sittingInCarPrefab;

	private Rigidbody rb;
	private Animator anim;
	private JobManager jobManager;
	private GameObject player;
	private CarDrive carDrive;
	private TutorialManager tutorialManager;
	private PedestrianManager pedestrianManager;
	private AudioSource passengerEnter;
	private GameModeManager gameModeManager;
	private NoticesManager noticesManager;
	private MoneyManager moneyManager;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		jobManager = GameObject.Find ("GameManager").GetComponent<JobManager> ();
		player = GameObject.Find ("Player").gameObject;
		carDrive = player.GetComponent<CarDrive> ();
		tutorialManager = GameObject.Find ("GameManager").GetComponent<TutorialManager> ();
		pedestrianManager = GameObject.Find ("GameManager").GetComponent<PedestrianManager> ();
		passengerEnter = GameObject.Find ("SFX").transform.Find ("Passenger Enter").GetComponent<AudioSource> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
		moneyManager = GameObject.Find ("GameManager").GetComponent<MoneyManager> ();

		// Set the skin to be a random texture
		torso.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textures [Random.Range (0, textures.Count)]);
	}

	void OnCollisionEnter(Collision other) {
		// On collision with the player
		if (other.gameObject.tag == "Player") {
			if (jobManager.CurrentPassenger == gameObject && carDrive.Handbrake && player.GetComponent<Rigidbody>().velocity.magnitude < 1f) {
				// If the NPC is the player's customer and the player is stopped
				if (transform.parent != player.transform) {
					// Enter the car
					EnterCar ();
				}
			} else if(player.GetComponent<Rigidbody>().velocity.magnitude > 1f) {

				// Collateral game mode
				if (gameObject.tag != "Untagged" && gameModeManager.gameMode == "collateral") {
					noticesManager.ShowMainScreenNotice ("Pedestrian Killed", "- " + moneyManager.moneySymbol + "25", new Color(255 / 255, 69 / 255, 69 / 255), GameObject.Find ("SFX").transform.Find ("Failure").GetComponent<AudioSource> ());
					moneyManager.ChangeMoney (-25);
				}

				// Kill the NPC
				gameObject.tag = "Untagged";
				GetComponent<GuyMovement> ().walking = false;
				GetComponent<GuyMovement> ().Dead = true;
				rb.constraints = RigidbodyConstraints.None;
				rb.AddForce (Vector3.up * 20f * GameObject.Find ("Player").GetComponent<Rigidbody> ().velocity.magnitude);

				// If the NPC was the passenger, end the mission
				if (jobManager.CurrentPassenger == gameObject) {
					jobManager.EndMission (false);
				}

				// Tutorial
				if (!tutorialManager.TutorialIsShown(2)) {
					tutorialManager.ShowTutorial (2);
				}

				StartCoroutine (RespawnNew ());
			}
		}
	}

	public void EnterCar() {
		carDrive.ForceHandbrake = false;
		player.transform.Find ("Passenger").gameObject.SetActive (true);
		player.transform.Find("Passenger").Find("Torso").GetComponent<Renderer> ().material.SetTexture ("_MainTex", torso.GetComponent<Renderer>().material.GetTexture("_MainTex"));
		jobManager.SetPassengerDestination ();
		player.GetComponent<DestinationCollision> ().State = 1;

		passengerEnter.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		passengerEnter.Play ();

		Destroy (gameObject);
	}

	IEnumerator RespawnNew() {

		// Destroy & respawn
		yield return new WaitForSeconds (8f);
		pedestrianManager.KillPed ();
		Destroy (gameObject);
	}
}
