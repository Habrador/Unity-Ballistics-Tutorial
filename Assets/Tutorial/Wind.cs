using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour 
{
	void Start() 
	{
        Vector3 windDirection = SniperController.windSpeed.normalized;
        Quaternion rotation = Quaternion.LookRotation(windDirection);
        transform.rotation = rotation;
	}
}
