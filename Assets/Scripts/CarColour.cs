using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColour : MonoBehaviour {

	public GameObject carBody;
	public List<GameObject> carTrims;
	public List<Color> colors;

	void Start () {
		carBody.GetComponent<Renderer> ().material.color = colors [Random.Range (0, colors.Count)];

		Color randomColor = colors [Random.Range (0, colors.Count)];
		for (int i = 0; i < carTrims.Count; i++) {
			carTrims [i].GetComponent<Renderer> ().material.color = randomColor;
		}
	}
}
