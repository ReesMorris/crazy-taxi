using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

	public float dragSpeed;
	public float minRight;
	public float maxRight;
	public float maxUp;
	public float minUp;
	public GameObject camera;

	private Vector3 dragOrigin;
	private bool panning;

	// Code for this script was inspired by code from [4]

	void Update () {

		// Check to see if the right mouse button is pressed
		if (Input.GetMouseButton (1) && !panning) {
			dragOrigin = Input.mousePosition;
			panning = true;
		}

		// Reset the panning variable, to allow a new pan
		if (!Input.GetMouseButton (1)) {
			panning = false;
		}

		// If the user is dragging the mouse
		if (panning) {
			Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);

			// Rotation on X dir
			// Make the camera rotation a more managable number (uses negative numbers, as opposed to letting 360 wrap around)
			float xRot = camera.transform.localEulerAngles.x;
			if (camera.transform.localEulerAngles.x > 180) {
				xRot = camera.transform.localEulerAngles.x - 360;
			}

			// Check to see if camera can move
			if (pos.y > 0 && xRot > maxUp || pos.y < 0 && xRot < minUp) {
				camera.transform.Rotate (new Vector3 (-pos.y * dragSpeed, 0f, 0f));
			}

			// Rotation on Y dir
			// Make the camera rotation a more managable number (uses negative numbers, as opposed to letting 360 wrap around)
			float yRot = camera.transform.localEulerAngles.y;
			if (camera.transform.localEulerAngles.y > 180) {
				yRot = camera.transform.localEulerAngles.y - 360;
			}

			// Check to see if camera can move
			if (pos.x > 0 && yRot < maxRight || pos.x < 0 && yRot > minRight) {
				camera.transform.Rotate (new Vector3 (0f, pos.x * dragSpeed, 0f));
			}
		}
	}
}