using UnityEngine;
using System.Collections;

public class RotateAroundCam : MonoBehaviour {

	public Transform LookAtThisOBJ;
	

	float distance = 5.0f;
	float xSpeed = 180.0f;
	float ySpeed = 90.0f;
	float yMinLimit = 0.0f;
	float yMaxLimit = 80f;
	float speed = 0.14f;
	float x = 0.0f;
	float y = 0.0f;

	Quaternion rotation;

	void Start() {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		transform.position += new Vector3(0f, 5f, 0f);
	}



	void LateUpdate () {
		if (LookAtThisOBJ && Input.GetMouseButton(0)) {
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			rotation = Quaternion.Euler(y, x, 0f);
			transform.rotation = rotation;
		}

		//Always change zoom
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + LookAtThisOBJ.position;

		transform.position = position;

		transform.LookAt(LookAtThisOBJ.position + LookAtThisOBJ.up * 1f);
	}



	float ClampAngle(float angle, float min, float max) {
		if (angle < -360f)
			angle += 360f;
		if (angle > 360f)
			angle -= 360f;

		return Mathf.Clamp(angle, min, max);
	}



	void Update () {
		//transform.LookAt(LookAtThisOBJ);

		//This will move the camera slowly around the object
		//transform.position -= transform.right * speed * Time.deltaTime;

		if (Input.GetMouseButton(0)) {
			transform.LookAt(LookAtThisOBJ);
			transform.RotateAround(LookAtThisOBJ.position, Vector3.up, Input.GetAxis("Mouse X") * speed);
		}

		//Zoom in/out
		if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
			distance -= 0.2f;
		} 
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
			distance += 0.2f;
		} 

		distance = Mathf.Clamp(distance, 1f, 10f);
	}




}
