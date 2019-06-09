using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyMovement : MonoBehaviour {

	public float speed;
	public bool walking;

	private bool dead;
	public bool Dead {
		get {
			return dead;
		}
		set {
			dead = value;
		}
	}

	private int direction;
	public int Direction {
		get {
			return direction;
		}
		set {
			direction = value;
		}
	}

	private List<int> potentialDirections;
	private Animator anim;
	private RaycastHit hit;
	private bool stoppedWalkingForCar;
	private JobManager jobManager;

	void Start() {
		dead = false;
		direction = (int) transform.eulerAngles.y; // [1]
		stoppedWalkingForCar = false;
		jobManager = GameObject.Find("GameManager").GetComponent<JobManager> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {
		// If the AI is walking
		anim.SetBool("walking", walking);
		anim.SetBool("dead", dead);
		if (walking && !dead) {
			// Either walk or turn towards new direction
			if ((int)transform.eulerAngles.y == direction) {
				transform.Translate ((Vector3.forward * speed) * Time.deltaTime);
			} else {
				transform.eulerAngles = new Vector3 (0f, direction, 0f);
			}

			if (Physics.Raycast (new Vector3 (transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hit, 80)) {
				if (hit.transform.name == "Player") {
					walking = false;
					stoppedWalkingForCar = true;
				}
			}
		} else if (stoppedWalkingForCar && jobManager.CurrentPassenger != gameObject) {
			if (Physics.Raycast (new Vector3 (transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hit, 80)) {
				if (hit.transform.name != "Player") {
					StartWalkingAgain ();
				}
			} else {
				StartWalkingAgain ();
			}
		}
	}

	void StartWalkingAgain() {
		walking = true;
		stoppedWalkingForCar = false;
	}

	void OnTriggerEnter(Collider other) {
		// When colliding with a waypoint
		if (other.tag == "Waypoint") {

			// Create a reference to the script
			potentialDirections = new List<int>(other.GetComponent<Waypoint> ().directions); // [2]

			// Pick a new direction
			if (potentialDirections.Count > 1) {
				// More than one possible direction
				for (int i = 0; i < potentialDirections.Count; i++) {
					// Remove the possibility to walk back on ourselves, since that's silly
					if (potentialDirections [i] - 180 == direction || potentialDirections [i] + 180 == direction) {
						potentialDirections.RemoveAt (i);
					}
				}
				direction = potentialDirections[Random.Range (0, potentialDirections.Count)];
			} else {
				// Only one direction to go
				direction = potentialDirections[0];
			}
		}
	}
}
