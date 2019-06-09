using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationCollision : MonoBehaviour {

	private Rigidbody rb;
	private JobManager jobManager;
	private GameObject player;
	private CarDrive carDrive;
	private TutorialManager tutorialManager;

	private int state; // 0 = pickup, 1 = dropoff
	public int State {
		get {
			return state;
		}
		set {
			state = value;
		}
	}

	void Start () {
		rb = GetComponent<Rigidbody> ();
		jobManager = GameObject.Find ("GameManager").GetComponent<JobManager> ();
		player = GameObject.Find ("Player").gameObject;
		carDrive = player.GetComponent<CarDrive> ();
		tutorialManager = GameObject.Find ("GameManager").GetComponent<TutorialManager> ();
		state = 0;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Destination") {
			if (state == -1) {
				state = 0;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Destination") {

			// Tutorial
			if (!tutorialManager.TutorialIsShown(1)) {
				tutorialManager.ShowTutorial (1);
			}

			// Player is stopped inside destination marker
			if (carDrive.Handbrake && player.GetComponent<Rigidbody>().velocity.magnitude < 1f) {
				if (state == 0) {
					// Picking up
					if (jobManager.CurrentPassenger != null && jobManager.CurrentPassenger.transform.parent != player.transform) {
						StartCoroutine ("CameraTutorial");
						StartCoroutine ("AutoPickupTimer");
						carDrive.ForceHandbrake = true;
						jobManager.CurrentPassenger.transform.LookAt (player.transform.position);
						jobManager.CurrentPassenger.GetComponent<GuyMovement> ().Direction = (int)jobManager.CurrentPassenger.transform.eulerAngles.y;
						jobManager.CurrentPassenger.GetComponent<GuyMovement> ().walking = true;
					}
				} else if (state == 1 && player.GetComponent<Rigidbody>().velocity.magnitude < 1f) {
					// Dropping person off
					state = 2;
					jobManager.EndMission (true);
				}
			}
		}
	}

	IEnumerator CameraTutorial() {
		yield return new WaitForSeconds (15f);
		if (!tutorialManager.TutorialIsShown(3)) {
			tutorialManager.ShowTutorial (3);
		}
	}

	// Prevent a NPC taking too long, or getting stuck somewhere
	IEnumerator AutoPickupTimer() {
		yield return new WaitForSeconds (3f);
		if (state == 0) {
			jobManager.CurrentPassenger.GetComponent<GuyNPC> ().EnterCar ();
		}
	}
}
