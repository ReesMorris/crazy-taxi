using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoticesManager : MonoBehaviour {

	public GameObject noticesPanel;
	public GameObject noticePrefab;
	public GameObject mainScreenTitle;

	private Text titleText;
	private Text captionText;

	void Start() {
		titleText = mainScreenTitle.transform.Find ("Title").GetComponent<Text> ();
		captionText = mainScreenTitle.transform.Find ("Caption").GetComponent<Text> ();
		HideTitles();
	}

	// Show a notice
	public void ShowNotice(Sprite icon, string text) {
		GameObject notice = Instantiate (noticePrefab, noticePrefab.transform.position, Quaternion.identity);
		notice.name = "Notice";
		notice.transform.SetParent(noticesPanel.transform);
		notice.transform.Find ("Image").GetComponent<Image> ().sprite = icon;
		notice.transform.Find ("Text").GetComponent<Text> ().text = text;
	}

	// Remove a notice
	public void RemoveNotice(string text) {
		for (int i = 0; i < noticesPanel.transform.childCount; i++) {
			if (noticesPanel.transform.GetChild(i).transform.Find ("Text").GetComponent<Text> ().text == text) {
				Destroy (noticesPanel.transform.GetChild (i).gameObject);
				break;
			}
		}
	}

	// Show a new main title
	public void ShowMainScreenNotice(string title, string caption, Color titleColor, AudioSource soundEffect) {
		StopCoroutine ("TitleNotice");
		HideTitles ();

		titleText.text = title;
		titleText.color = titleColor;
		captionText.text = caption;

		soundEffect.volume = PlayerPrefs.GetFloat ("sfx", 0.5f);
		soundEffect.Play ();

		StartCoroutine ("TitleNotice");
	}

	// Show the main titles with fade
	IEnumerator TitleNotice() {
		// Fade in
		for (int i = 0; i < 10; i++) {
			titleText.color = new Color (titleText.color.r, titleText.color.g, titleText.color.b, (float) i / 10);
			captionText.color = new Color (captionText.color.r, captionText.color.g, captionText.color.b, (float) i / 10);
			yield return new WaitForSeconds (0.05f);
		}
		yield return new WaitForSeconds (3f);
		HideTitles ();
	}

	// Hides the main titles without fade
	void HideTitles() {
		titleText.color = new Color (titleText.color.r, titleText.color.g, titleText.color.b, 0f);
		captionText.color = new Color (captionText.color.r, captionText.color.g, captionText.color.b, 0f);
	}
}
