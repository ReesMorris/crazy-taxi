using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioManager : MonoBehaviour {

	// Variable Declaration
	public AudioSource[] stations;
	public GameObject radioPanel;

	private int index = 0;
	private RadioStation currentStation;
	private PauseManager pauseManager;
	private float currentVolume;

	private Image stationImage;
	private Text stationName;
	private Text stationSong;

	// Start
	void Start () {

		currentVolume = PlayerPrefs.GetFloat ("music", 0.5f);

		// Declare the variables (so we can FadeOut() easier)
		pauseManager = GameObject.Find("GameManager").GetComponent<PauseManager> ();

		stationImage = radioPanel.transform.Find ("Icon Image").GetComponent<Image> ();
		stationName = radioPanel.transform.Find ("Station Name").GetComponent<Text> ();
		stationSong =  radioPanel.transform.Find ("Song Name").GetComponent<Text> ();

		for (int i = 0; i < stations.Length; i++) {
			stations [i].volume = 0f;
			stations [i].Play ();
			stations [i].time = Random.Range (0f, stations [i].clip.length - 1f);
		}
		stations [0].volume = currentVolume;
		currentStation = stations [0].GetComponent<RadioStation> ();

		StartCoroutine(CheckSongChange ());
		ChangeStationDetails ();
	}

	// Update
	void Update () {

		// Using the settings menu to change volume
		if(currentVolume != PlayerPrefs.GetFloat("music", 0.5f)) {
			currentVolume = PlayerPrefs.GetFloat ("music", 0.5f);
			stations [index].volume = currentVolume;
		}

		if (Time.timeScale != 0f && !pauseManager.Paused) {
			if (Input.GetKeyDown (KeyCode.E)) {
				CycleStations ("right");
			}
			if (Input.GetKeyDown (KeyCode.Q)) {
				CycleStations ("left");
			}
		}

		// Prevents music playing when game is paused
		if (pauseManager.Paused && stations [index].isPlaying) {
			// Game is paused and the music is playing
			for (int i = 0; i < stations.Length; i++) {
				stations [i].Pause ();
			}
		}
		if (!pauseManager.Paused && !stations [index].isPlaying) {
			// Game is unpaused and music isn't playing
			for (int i = 0; i < stations.Length; i++) {
				stations [i].UnPause ();
			}
		}
	}

	// Runs once per second
	// Updates the song name, in case the UI is open and the song changes
	IEnumerator CheckSongChange() {
		while (true) {
			ChangeSongName ();
			yield return new WaitForSeconds (1f);
		}
	}

	// When the user changes radio station
	void CycleStations(string direction) {

		// This if statement allows the station to be shown again after it's faded out
		// meaning that the user can view the current song without needing to change station again
		if (stationImage.color.a > 0) {
			// Mute the current station
			stations [index].volume = 0f;

			// Change the station index
			if (direction == "left") {
				if (index == 0) {
					index = stations.Length - 1;
				} else {
					index--;
				}
			} else if (direction == "right") {
				if (index == stations.Length - 1) {
					index = 0;
				} else {
					index++;
				}
			}
		}

		// Unmute the new station, get its script, and update the UI
		stations [index].volume = currentVolume;
		currentStation = stations [index].GetComponent<RadioStation> ();
		ChangeStationDetails ();
	}

	// Function to update the UI for the radio
	void ChangeStationDetails() {
		// Make the UI appear
		StopCoroutine("FadeOut");
		stationImage.color = new Color (stationImage.color.r, stationImage.color.g, stationImage.color.b, 1f);
		stationName.color = new Color (stationName.color.r, stationName.color.g, stationName.color.b, 1f);
		stationSong.color = new Color (stationSong.color.r, stationSong.color.g, stationSong.color.b, 1f);

		// Change the values
		stationName.text = currentStation.displayName;
		stationImage.sprite = currentStation.radioIcon;

		// Change the song name and then fade out after a few seconds
		ChangeSongName ();
		StartCoroutine ("FadeOut");
	}

	// Function to change the name of the current song
	// Because the radio is one sound file, it uses timestamps predefined in the RadioStation script
	void ChangeSongName() {
		// Set the title to be the first song, in case no other songs are found
		string title = currentStation.songName[0];

		// Loop through every song in the station's RadioStation array
		for (int i = 0; i < currentStation.songTime.Count; i++) {

			// If the station's timestamp is past the time defined, then it's not that song
			// So we update the title to the new one
			if (stations [index].time > currentStation.songTime [i]) {
				title = currentStation.songName [i];
			} else {
				// We can break out once the song hasn't reached the next timestamp
				break;
			}
		}

		// Change the song's title on the UI
		if (stationName.text != "Radio Off") {
			radioPanel.transform.Find ("Song Name").GetComponent<Text> ().text = "\"" + title + "\"";
		} else {
			radioPanel.transform.Find ("Song Name").GetComponent<Text> ().text = "";
		}
	}

	IEnumerator FadeOut() {
		yield return new WaitForSeconds (3f);
		while (stationImage.color.a > 0) {
			stationImage.color = new Color (stationImage.color.r, stationImage.color.g, stationImage.color.b, stationImage.color.a - 0.25f);
			stationName.color = new Color (stationName.color.r, stationName.color.g, stationName.color.b, stationName.color.a - 0.25f);
			stationSong.color = new Color (stationSong.color.r, stationSong.color.g, stationSong.color.b, stationSong.color.a - 0.25f);
			yield return new WaitForSeconds (0.1f);
		}
	}
}