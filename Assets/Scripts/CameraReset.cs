using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour {

	public GameObject camerasParent;

	private List<Vector3> rotations;
	private GameObject player;

	void Start () {
		rotations = new List<Vector3> ();
		player = GameObject.Find ("Player");
		for (int i = 0; i < camerasParent.transform.childCount; i++) {
			rotations.Add (camerasParent.transform.GetChild (i).transform.eulerAngles);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			for (int i = 0; i < camerasParent.transform.childCount; i++) {
				camerasParent.transform.GetChild (i).transform.rotation = Quaternion.Euler(new Vector3(rotations [i].x, rotations [i].y + player.transform.eulerAngles.y, rotations [i].z));
			}
		}
	}
}
