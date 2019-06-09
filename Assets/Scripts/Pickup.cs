using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour {

	public Sprite noticeSprite;

	private bool activated;
	public bool Activated {
		get {
			return activated;
		}
		set {
			activated = value;
		}
	}

	private NoticesManager noticesManager;
	private PickupManager pickupManager;
	private GameObject pickupObject;
	private PauseManager pauseManager;
	private TutorialManager tutorialManager;

	// Defines the type that the pickup is
	public enum PickupTypes{Time, Multiplier, Patience}; // [8]
	public PickupTypes type;

	void Start() {
		activated = false;
		noticesManager = GameObject.Find ("GameManager").GetComponent<NoticesManager> ();
		pickupManager = GameObject.Find ("GameManager").GetComponent<PickupManager> ();
		pauseManager = GameObject.Find ("GameManager").GetComponent<PauseManager> ();
		tutorialManager = GameObject.Find ("GameManager").GetComponent<TutorialManager> ();
		pickupObject = transform.parent.Find ("Object").gameObject;

		// Tutorial
		if (!tutorialManager.TutorialIsShown(4)) {
			tutorialManager.ShowTutorial (4);
		}

		StartCoroutine (ShowNotice ());
	}

	// Rotate the pickup around all axis'
	void Update () {
		if (!pauseManager.Paused) {
			pickupObject.transform.Rotate (new Vector3 (1f, 1f, 1f));
		}
	}

	// Show a notice to say a pickup has spawned
	IEnumerator ShowNotice() {
		pickupManager.ChangeTotalPickups (1);
		noticesManager.ShowNotice (noticeSprite, "A pickup has spawned!");
		yield return new WaitForSeconds (3f);
		noticesManager.RemoveNotice ("A pickup has spawned!");
		yield return new WaitForSeconds (3f);
	}
}
