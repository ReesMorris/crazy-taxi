using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarMovement : MonoBehaviour {

	public float speed;

	private List<int> potentialDirections;
	private AICarManager carManager;
	private GameModeManager gameModeManager;
	private NoticesManager noticesManager;
	private MoneyManager moneyManager;
	private RaycastHit hit;
	private float speed_;
	private bool dead;

	void Start() {
		dead = false;
		carManager = GameObject.Find ("GameManager").GetComponent<AICarManager> ();
		gameModeManager = GameObject.Find ("GameManager").GetComponent<GameModeManager> ();
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
		moneyManager = GameObject.Find ("GameManager").GetComponent<MoneyManager> ();
		speed_ = speed; // This just remembers the default speed, so we can change it back after the raycast ends
	}

	void Update () {
		if (!dead) {
			transform.Translate ((transform.forward * speed) * Time.deltaTime);

			// Detect cars or the player in front of the car and stop if one is there. Stops cars crashing in to each other.
			if (Physics.Raycast (transform.position, transform.up, out hit, 17)) {
				if (hit.transform.name == "Player" || hit.transform.name == "Car") {
					StartCoroutine ("DestroyAfterTime"); // prevent cars getting stuck in a jam
					speed = 0f;
				} else {
					// Nothing is in front, so let's move (again)
					StopCoroutine ("DestroyAfterTime");
					speed = speed_;
				}
			} else {
				// Nothing is in front, so let's move (again)
				StopCoroutine ("DestroyAfterTime");
				speed = speed_;
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name == "Player") {
			// Collateral game mode
			if (!dead && gameModeManager.gameMode == "collateral") {
				noticesManager.ShowMainScreenNotice ("Car Crash", "- " + moneyManager.moneySymbol + "25", new Color(255 / 255, 69 / 255, 69 / 255), GameObject.Find ("SFX").transform.Find ("Failure").GetComponent<AudioSource> ());
				moneyManager.ChangeMoney (-25);
			}

			// Kill car
			dead = true;
			Rigidbody rb = GetComponent<Rigidbody> ();
			rb.constraints = RigidbodyConstraints.None;
			rb.AddForce (Vector3.up * 20f * GameObject.Find ("Player").GetComponent<Rigidbody> ().velocity.magnitude);
			gameObject.name = "Car [Dead]"; // prevents other cars stopping for the debris
			StartCoroutine ("DestroyAfterTime");
		}
	}

	void OnTriggerEnter(Collider other) {
		// When colliding with a waypoint
		if (other.tag == "Waypoint") {
			int i = 0;
			int prevRot = (int)transform.eulerAngles.y;

			// Clone the list from the Waypoint, so that we don't remove values from the actual object
			potentialDirections = new List<int>(other.GetComponent<Waypoint> ().directions); // [2]

			// Find potential directions to travel in
			if (potentialDirections.Count > 1) {
				// Remove potential for car to go back on itself, as that's silly
				for(i = 0; i < potentialDirections.Count; i++) {
					if (transform.eulerAngles.z + 90f == potentialDirections [i]) {
						potentialDirections.RemoveAt (i);
					}
				}

				// Choose a random direction from the new list
				i = Random.Range (0, potentialDirections.Count);
				transform.rotation = Quaternion.Euler (new Vector3 (-90f, 0f,  potentialDirections [i] + 180f));
			} else {
				transform.rotation = Quaternion.Euler (new Vector3 (-90f, 0f,  potentialDirections [i] + 180f));
			}

			// If turning, move closer in to the road

			if ((int)transform.eulerAngles.y != prevRot) {
				transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 55555f);
			}
		}
	}

	// Prevents cars getting stuck in a square shape, where none can move
	IEnumerator DestroyAfterTime() {
		yield return new WaitForSeconds (5f);
		carManager.KillCar ();
		Destroy (gameObject);
	}
}
