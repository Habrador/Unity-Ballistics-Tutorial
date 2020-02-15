using UnityEngine;
using System.Collections;

//Move the target the gun is trying to hit with keys
public class TargetController : MonoBehaviour 
{
    private float moveSpeed = 10f;
	
	

	void Update() 
	{
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }
	}
}
