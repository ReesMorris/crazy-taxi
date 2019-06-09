using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

	public List<GameObject> cameras;

	private CameraDrag cameraDrag;

	void Start() {
		cameraDrag = GameObject.Find ("GameManager").GetComponent<CameraDrag> ();
	}

	void Update () {
		if (Input.anyKeyDown) {
			for (int i = 0; i < cameras.Count; i++) {
				if (Input.GetKeyDown ("" + (i + 1))) {
					ChangeCamera (i);
				}
			}
		}
	}

	void ChangeCamera(int index) {
		for (int i = 0; i < cameras.Count; i++) {
			cameras[i].GetComponent<Camera> ().enabled = false;
			if (i == index) {
				cameras[i].GetComponent<Camera> ().enabled = true;
				cameraDrag.camera = cameras [i];
			}
		}
	}
}
