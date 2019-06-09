using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	public float daySpeed;
	public float nightSpeed;
	public GameObject sun;

	private PauseManager pauseManager;
	private float currentTime;
	public float CurrentTime {
		get {
			return currentTime;
		}
	}

	void Start () {
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
		currentTime = 0;
	}

	void Update () {
		if (!pauseManager.Paused) {
			if (currentTime > 360) {
				// If the light has done a full rotation, we'll reset it
				currentTime = currentTime - 360;
			}

			if (currentTime < 180) {
				// Day time
				sun.transform.Rotate (new Vector3 (-daySpeed, 0f, 0f));
				currentTime += daySpeed;
				sun.GetComponent<Light> ().intensity = 1f;
			} else {
				// Night time
				sun.transform.Rotate (new Vector3 (-nightSpeed, 0f, 0f));
				currentTime += nightSpeed;
				sun.GetComponent<Light> ().intensity = 0.2f;
			}
		}
	}
}