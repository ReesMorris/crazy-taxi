using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickupTimer : MonoBehaviour {

	private PickupManager pickupManager;
	private GameObject fill;
	private float timeLeft;
	private GameObject player;

	void Start () {
		pickupManager = GameObject.Find ("GameManager").GetComponent<PickupManager> ();
		player = GameObject.Find ("Player").gameObject;
		fill = transform.Find ("Fill").gameObject;

		timeLeft = pickupManager.timerDuration;
		StartCoroutine (Timer ());
	}

	void Update() {
		transform.LookAt(player.transform);
	}

	IEnumerator Timer() {
		while (true) {
			if (timeLeft <= 0) {
				pickupManager.ChangeTotalPickups (-1);
				Destroy(transform.parent.gameObject);
			}

			timeLeft -= 0.2f;
			fill.GetComponent<Image> ().fillAmount = timeLeft / pickupManager.timerDuration;

			yield return new WaitForSeconds (0.2f);
		}
	}
}
