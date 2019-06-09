using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	/*
	 * This is used for the waypoint gameobject to store an angle (northwards) of which
	 * an NPC will move on collision. It will be randomly selected.
 	*/

	public List<int> directions;
	public enum WaypointType{Pedestrian, Car}; // [8]
	public WaypointType type;

	private PedestrianManager pedManager;
	private AICarManager carManager;

	void Start() {
		if (type == WaypointType.Pedestrian) {
			pedManager = GameObject.Find ("GameManager").GetComponent<PedestrianManager> ();
			pedManager.AddSpawner (gameObject);
		} else if (type == WaypointType.Car) {
			carManager = GameObject.Find ("GameManager").GetComponent<AICarManager> ();
			carManager.AddSpawner (gameObject);
		}
	}

}
