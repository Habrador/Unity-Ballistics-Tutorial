using UnityEngine;
using System.Collections;

public class SidewaysCam : MonoBehaviour {

	float height = 10f;
	float distanceBack = 10f;

	float camMoveSpeed = 30f;
	float zoomSpeed = 3f;

    float minZoomDistance = 5f;
    float maxZoomDistance = 100f;


	void Start() 
	{
		transform.position = Vector3.zero;
		//Move up
		transform.position += new Vector3(0f, height, 0f);
		//Move back
		transform.position -= new Vector3(0f, 0f, distanceBack);
		//Look at the center to get an angle
		transform.LookAt(Vector3.zero);
	}



	void LateUpdate() 
	{
		//Move camera with keys
		//Move left/right
		if (Input.GetKey(KeyCode.A)) {
			transform.position -= new Vector3(camMoveSpeed * Time.deltaTime, 0f, 0f);
		}
		else if (Input.GetKey(KeyCode.D)) {
			transform.position += new Vector3(camMoveSpeed * Time.deltaTime, 0f, 0f);
		}

		//Move forward/back
		if (Input.GetKey(KeyCode.S)) {
			transform.position -= new Vector3(0f, 0f, camMoveSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.W)) {
			transform.position += new Vector3(0f, 0f, camMoveSpeed * Time.deltaTime);
		}

		//Zoom
		float currentHeight = transform.position.y;

		float zoomDistance = 0f;

        if (currentHeight > minZoomDistance && currentHeight < maxZoomDistance)
        {
			if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.I)) {
				zoomDistance += zoomSpeed;
			} 
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.O)) {
				zoomDistance -= zoomSpeed;
			} 
		}
		//Can only zoom in
        else if (currentHeight > maxZoomDistance)
        {
			if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.I)) {
				zoomDistance += zoomSpeed;
			} 
		}
		//Can only zoom out
        else if (currentHeight < minZoomDistance)
        {
			if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.O)) {
				zoomDistance -= zoomSpeed;
			} 
		}

		transform.Translate(Vector3.forward * zoomDistance);
	}
}
