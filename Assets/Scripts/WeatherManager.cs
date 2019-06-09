using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

	public int minSunny;
	public int maxSunny;
	public Color sunnySunColour;
	public List<GameObject> weathers;

	private int countdown;
	private bool weatherEnabled;
	private bool weatherClear;

	// Sunlight
	private Light sun;
	private float lerpT = 1f; // lerp time, between 0 and 1
	private float lerpD = 2f; // lerp duration
	private Color lerpOC; // original colour of sun
	private Color lerpNC; // new colour of sun

	void Start() {
		//weatherEnabled = PlayerPrefs.GetInt ("weather", 1) == 1 ? true : false;
		sun = GameObject.Find ("Sunlight").GetComponent<Light>();
		weatherEnabled = true;
		StopWeather ();
	}

	void Update() {
		// Detect changes of the setting
		if (weatherEnabled != (PlayerPrefs.GetInt ("weather", 1) == 1 ? true : false)) {
			weatherEnabled = (PlayerPrefs.GetInt ("weather", 1) == 1 ? true : false);
			StopWeather ();
		}

		// Changes the sun colour on weather change. Works alongside ChangeSunColour()
		if(lerpT < 1f) {
			sun.color = Color.Lerp(lerpOC, lerpNC, lerpT); // [11]
			lerpT += Time.deltaTime/lerpD;
		}
	}

	// Starts the weather
	void StartWeather(int index) {
		weathers [index].GetComponent<ParticleSystem> ().Play ();
		weatherClear = false;
		ChangeSunColour (weathers [index].GetComponent<WeatherParticles> ().sunColour);
		StartCountdown (weathers[index].GetComponent<WeatherParticles>().minTime, weathers[index].GetComponent<WeatherParticles>().maxTime);
	}

	// Stops the weather
	void StopWeather() {
		for (int i = 0; i < weathers.Count; i++) {
			weathers [i].GetComponent<ParticleSystem> ().Stop ();
			weathers [i].GetComponent<ParticleSystem> ().Clear ();
		}
		weatherClear = true;
		ChangeSunColour (sunnySunColour);
		if (weatherEnabled) {
			StartCountdown (minSunny, maxSunny);
		}
	}

	// Start the countdown
	void StartCountdown(int min, int max) {
		StopCoroutine ("CountdownTimer"); // Stop coroutine case it is running already
		countdown = Random.Range (min, max); // Decide a random time for the weather to last
		StartCoroutine ("CountdownTimer"); // Start the coroutine
	}

	// Changes the colour of the sun, for ambience. Works along side Update()
	void ChangeSunColour(Color colour) {
		lerpT = 0f;
		lerpOC = sun.color;
		lerpNC = colour;
	}

	// Countdown to weather end
	IEnumerator CountdownTimer() {
		while (countdown >= 0) {
			if (countdown == 0) {
				CountdownEnded ();
			}
			countdown--;
			yield return new WaitForSeconds(1f);
		}
	}

	// Manage the weather after countdown ends
	void CountdownEnded() {
		if (weatherEnabled) {
			// Weather feature is enabled
			if (weatherClear) {
				// It's time to start weather
				StartWeather (Random.Range (0, weathers.Count));
			} else {
				// Let's be sunny again
				StopWeather ();
			}
		}
	}
}