using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour {

	public GameObject background;
	public List<Sprite> backgrounds;

	/*
	 * Changes the main menu background to a random one from the list on load.
	*/

	void Start () {
		background.GetComponent<SpriteRenderer> ().sprite = backgrounds [Random.Range (0, backgrounds.Count - 1)];
	}
}
