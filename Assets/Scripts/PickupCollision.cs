using System.Collections;
using UnityEngine;

public class PickupCollision : MonoBehaviour {

	public Sprite noticeSprite;
	public AudioSource pickupSound;

	private Pickup pickupScript;
	private NoticesManager noticesManager;
	private GameTime gameTime;
	private GameObject pickup;
	private PickupManager pickupManager;
	private JobManager jobManager;
	private string pickupType;

	void Start() {
		gameTime = GameObject.Find ("GameManager").GetComponent<GameTime> ();
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
		pickupManager = GameObject.Find ("GameManager").GetComponent<PickupManager> ();
		jobManager = GameObject.Find ("GameManager").GetComponent<JobManager> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pickup") {
			pickup = other.gameObject;
			pickupScript = other.GetComponent<Pickup> ();

			if (!pickupScript.Activated) {
				pickupScript.Activated = true;
				noticesManager.RemoveNotice ("A pickup has spawned!");
				noticesManager.RemoveNotice (pickupType + " pickup collected!");

				if (pickupScript.type == Pickup.PickupTypes.Time) {
					// Time
					gameTime.IncreaseTime (Random.Range (10, 20));
					pickupType = "Time";
				}
				if(pickupScript.type == Pickup.PickupTypes.Multiplier) {
					// Multiplier
					StartCoroutine (NewMultiplier());
					pickupType = "Multiplier";
				}
				if(pickupScript.type == Pickup.PickupTypes.Patience) {
					// Multiplier
					jobManager.TimeTaken -= Random.Range(15, 25);
					pickupType = "Patience";
				}

				pickupSound.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
				pickupSound.Play ();

				StartCoroutine (ShowNotice ());
				Destroy (pickup.transform.parent.gameObject);
			}
		}
	}

	IEnumerator ShowNotice() {
		pickupManager.TotalCollected++;
		pickupManager.ChangeTotalPickups (-1);
		noticesManager.ShowNotice (noticeSprite, pickupType + " pickup collected!");
		noticesManager.RemoveNotice ("A pickup has spawned!");
		yield return new WaitForSeconds (3f);
		noticesManager.RemoveNotice (pickupType + " pickup collected!");
	}

	IEnumerator NewMultiplier() {
		// Allows for multiple multipliers to be running at once, but they'll deactivate
		// after their own 20 second timer is up, rather than all at once.
		jobManager.fareMultiplier = jobManager.fareMultiplier * 2;
		yield return new WaitForSeconds (20f);
		jobManager.fareMultiplier = jobManager.fareMultiplier / 2;
		noticesManager.ShowNotice (noticeSprite, "A multiplier pickup effect has ended!");
		yield return new WaitForSeconds (3f);
		noticesManager.RemoveNotice ("A multiplier pickup effect has ended!");
	}
}
