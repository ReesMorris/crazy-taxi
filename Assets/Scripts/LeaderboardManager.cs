using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {
	public string highscoreURL;
	public GameObject panelPrefab;
	public GameObject instantiateLocation;

	private GameObject errorMessage;

	void Start() {
		errorMessage = instantiateLocation.transform.Find("ErrorMessage").gameObject;
		StartCoroutine(GetScores());
	}

	public void RefreshScores() {
		StartCoroutine(GetScores());
	}

	// Get the scores from the database to display in a GUIText.
	IEnumerator GetScores() {

		using(UnityWebRequest webRequest = UnityWebRequest.Get(highscoreURL)) {
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError) {
				errorMessage.GetComponent<Text>().text = "Unable to load scores at this time\n" + webRequest.error;
				errorMessage.SetActive(true);
			} else {

				// Delete any scores already there (for refreshing)
				for (int i = 0; i < instantiateLocation.transform.childCount; i++) {
					Destroy(instantiateLocation.transform.GetChild(i).gameObject);
				}

				// Take the results
				JSONNode results = JSON.Parse(webRequest.downloadHandler.text);

				// Create panels to show the results
				for (int i = 0; i < results.Count; i++) {
					GameObject highScore = Instantiate(panelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					highScore.transform.SetParent(instantiateLocation.transform);

					highScore.transform.Find("Rank").transform.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
					highScore.transform.Find("Score").transform.Find("Score").GetComponent<Text>().text = results[i]["name"];
					highScore.transform.Find("Name").transform.Find("Name").GetComponent<Text>().text = "$" + results[i]["score"];
				}

				// Change the scrollView to be the height of all of the results, to allow the scroll bar to work as intended.
				instantiateLocation.GetComponent<RectTransform>().sizeDelta = new Vector2(instantiateLocation.GetComponent<RectTransform>().sizeDelta.x, (results.Count - 1) * 23);
			}
		}
	}
}