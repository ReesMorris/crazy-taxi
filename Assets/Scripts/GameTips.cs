using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTips : MonoBehaviour {

	public Text tipText;
	public List<string> tips;

	void Start () {
		tipText.text = tips [Random.Range (0, tips.Count - 1)];
		StartCoroutine (CycleTips ());	
	}

	// Show the game tips whilst the game is loading
	IEnumerator CycleTips() {
		while (true) {
			yield return new WaitForSeconds (5f);

			// Fade the tip out
			while (tipText.color.a > 0) {
				tipText.color = new Color (tipText.color.r, tipText.color.g, tipText.color.b, tipText.color.a - 0.2f);
				yield return new WaitForSeconds (0.1f);
			}

			// Change the tip to a random from the list
			tipText.text = tips [Random.Range (0, tips.Count - 1)];

			// Fade the tip in
			while (tipText.color.a < 1) {
				tipText.color = new Color (tipText.color.r, tipText.color.g, tipText.color.b, tipText.color.a + 0.2f);
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}
