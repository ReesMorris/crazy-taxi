using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSetting : MonoBehaviour {

	public string playerPrefs;

	private Slider slider;
	private Toggle toggle;

	void Start () {
		
		// Using a slider
		if (GetComponent<Slider> () != null) {
			slider = GetComponent<Slider> ();
			slider.onValueChanged.AddListener(delegate {OnSliderChange ();}); // [6]

			slider.value = PlayerPrefs.GetFloat (playerPrefs, 0.5f);
		}

		// Using a toggle
		if (GetComponent<Toggle> () != null) {
			toggle = GetComponent<Toggle> ();
			toggle.onValueChanged.AddListener(delegate {OnToggleChange ();}); // [6]

			toggle.isOn = PlayerPrefs.GetInt(playerPrefs, 1) == 1 ? true : false; // [10]
		}
	}

	void Update () {
		
	}

	public void OnSliderChange()
	{
		PlayerPrefs.SetFloat (playerPrefs, slider.value);
	}

	public void OnToggleChange() {
		PlayerPrefs.SetInt (playerPrefs, toggle.isOn ? 1 : 0); // [10]
	}
}
