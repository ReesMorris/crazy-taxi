using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarManager : MonoBehaviour {

	public int maxCars;
	public GameObject carPrefab;

	public int currentCars;

	private List<GameObject> carSpawners;
	private int index;

	void Start () {
		index = 0;
	}

	void Update () {
		// While there are still pedestrians to be spawned, spawn them
		if (currentCars < maxCars) {
			SpawnCar ();
		}
	}

	// 'Waypoint.cs' will add itself to the list
	public void AddSpawner(GameObject spawner) {
		if (carSpawners == null) {
			carSpawners = new List<GameObject> ();
		}
		carSpawners.Add (spawner);
	}

	// Will spawn a car at the 'carSpawners' index
	void SpawnCar() {
		GameObject car_ = Instantiate (carPrefab, carSpawners [index].transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
		car_.name = "Car";

		// Change the index
		index = Random.Range (0, carSpawners.Count);

		// Increase the car counter
		currentCars++;
	}

	public void KillCar() {
		currentCars--;
	}
}
