using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianManager : MonoBehaviour {

	public int maxPeds;
	public GameObject pedPrefab;

	private int currentPeds;
	public int CurrentPeds {
		get {
			return currentPeds;
		}
	}

	private List<GameObject> pedSpawners;
	private int index;

	void Start () {
		index = 0;
	}

	void Update () {
		// While there are still pedestrians to be spawned, spawn them
		if (currentPeds < maxPeds) {
			SpawnPed ();
		}
	}

	// 'Waypoint.cs' will add itself to the list
	public void AddSpawner(GameObject spawner) {
		if (pedSpawners == null) {
			pedSpawners = new List<GameObject> ();
		}
		pedSpawners.Add (spawner);
	}

	// Will spawn a pedestrian at the 'pedSpawners' index
	void SpawnPed() {
		GameObject npc_ = Instantiate (pedPrefab, pedSpawners [index].transform.position, Quaternion.identity) as GameObject;
		npc_.name = "Pedestrian";

		// Change the index
		index = Random.Range(0, pedSpawners.Count);

		// Increase the pedestrian counter
		currentPeds++;
	}

	public void KillPed() {
		currentPeds--;
	}
}
