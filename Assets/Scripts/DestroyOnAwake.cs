using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAwake : MonoBehaviour {

	public float timeBeforeDestroy;

	/* This script is intended to destroy items after a time as they are instantiated.
	 * It was initially used for particle effects on death, but is no longer used.
	 * It is kept as it may be useful in the future.
	*/

	void Start () {
		StartCoroutine (Kill ());
	}

	IEnumerator Kill() {
		yield return new WaitForSeconds (timeBeforeDestroy);
		Destroy (gameObject);
	}
}
