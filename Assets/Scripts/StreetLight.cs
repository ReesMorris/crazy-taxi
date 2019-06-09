using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour {

	private List<GameObject> lights;
	private DayNightCycle dayNightCycle;
	private bool isNight;

	// Start
	void Start () {
		lights = new List<GameObject> ();
		dayNightCycle = GameObject.Find ("GameManager").GetComponent<DayNightCycle>();
		isNight = false;

		// Add all of the child lights to the array
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).name == "Light") {
				lights.Add (transform.GetChild (i).gameObject);
				transform.GetChild (i).GetComponent<Light> ().enabled = false;
			}
		}
	}
	
	// Update
	void Update () {
		// Check to see if we should turn the lights off/on
		// The isNight variable isn't needed, but saves running a for loop every frame...

		if (dayNightCycle.CurrentTime < 180 && isNight) {
			// Turn the lights off
			isNight = false;
			for (int i = 0; i < lights.Count; i++) {
				lights [i].GetComponent<Light> ().enabled = false;
			}
		} else if (dayNightCycle.CurrentTime > 180 && !isNight) {
			// Turn the lights on
			isNight = true;
			for (int i = 0; i < lights.Count; i++) {
				lights [i].GetComponent<Light> ().enabled = true;
			}
		}
	}
}
