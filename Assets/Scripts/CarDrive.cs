using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarDrive : MonoBehaviour {

	public Sprite handbrakeSprite;
	public AudioSource hornSound;
	public AudioSource drivingSound;

	public float maxMotorTorque;
	public float maxSteeringAngle;

	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public GameObject[] wheels;
	public List<AudioSource> handbrakeSounds;
	private Rigidbody rb;

	private NoticesManager noticesManager;
	private PauseManager pauseManager;
	private bool handbrake;
	public bool Handbrake {
		get {
			return handbrake;
		}
	}

	private bool forceHandbrake;
	public bool ForceHandbrake {
		get {
			return forceHandbrake;
		}
		set {
			forceHandbrake = value;
		}
	}

	void Start() {
		handbrake = false;
		rb = GetComponent<Rigidbody> ();
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
	}

	void Update () {
		if (!pauseManager.Paused) {
			
			// Horn sound
			if (Input.GetKeyDown (KeyCode.H)) {
				if (!hornSound.isPlaying) {
					hornSound.volume = PlayerPrefs.GetFloat ("sfx");
					hornSound.Play ();
				}
			}

			// Handbrake toggle
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (forceHandbrake) {
					handbrake = true;
				} else {
					handbrake = !handbrake;

					// Pick a random sound from the list, set its volume, and play it
					AudioSource handbrakeSound = handbrakeSounds [Random.Range (0, handbrakeSounds.Count)];
					handbrakeSound.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
					handbrakeSound.Play ();

					// Handbrake toggle
					if (handbrake) {
						leftWheel.brakeTorque = 400f;
						rightWheel.brakeTorque = 400f;
						noticesManager.ShowNotice (handbrakeSprite, "Handbrake Active");
					} else {
						leftWheel.brakeTorque = 0f;
						rightWheel.brakeTorque = 0f;
						noticesManager.RemoveNotice ("Handbrake Active");
					}
				}
			}

			drivingSound.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		} else {
			drivingSound.volume = 0f;
		}

		// Prevent the car getting stuck
		if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) && !handbrake && rb.velocity.magnitude < 1f) {
			StartCoroutine ("ResetCar");
		} else if(rb.velocity.magnitude > 1f) {
			StopCoroutine ("ResetCar");
		}

		drivingSound.pitch = 1f + (GetComponent<Rigidbody> ().velocity.magnitude * 0.025f);
	}

	// Wait for 4 seconds before resetting the player's car
	IEnumerator ResetCar() {
		yield return new WaitForSeconds (4f);
		GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
		GameObject nearestRoad = roads[0];
		float nearestDistance = Vector3.Distance (transform.position, roads [0].transform.position);

		// Cycle through each road and compare its distance to the player
		for (int i = 0; i < roads.Length; i++) {
			float distance = Vector3.Distance (transform.position, roads [i].transform.position);

			// If the road is closer to the player than the last, set it as the new closest
			if (distance < nearestDistance) {
				nearestDistance = distance;
				nearestRoad = roads [i];
			}
		}

		// Reset car position
		transform.position = nearestRoad.transform.position;
	}

	void FixedUpdate()
	{

		// Prevents the car from falling over on collision, as seen in earlier alphas
		transform.eulerAngles = new Vector3 (0f, transform.eulerAngles.y, 0f);

		float motor = maxMotorTorque * Input.GetAxis ("Vertical");
		if (Input.GetAxis ("Vertical") < 0) {
			motor = maxMotorTorque * 2 * Input.GetAxis ("Vertical");
		}
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

		// Set the steer angle of the car
		leftWheel.steerAngle = steering;
		rightWheel.steerAngle = steering;

		// Make the car move
		leftWheel.motorTorque = motor;
		rightWheel.motorTorque = motor;

		// Display a rotation for the car wheels
		for (int i = 0; i < wheels.Length; i++) {
			if (wheels [i].name == "Steering Wheel") {
				// Steering wheel
				wheels [i].transform.rotation = Quaternion.Euler(20f, GameObject.Find("Player").transform.eulerAngles.y, -steering); // [3]
			} else {
				// Car wheel
				wheels [i].transform.rotation = Quaternion.Euler(0f, GameObject.Find("Player").transform.eulerAngles.y + steering, 0f);
			}
		}
	}
}