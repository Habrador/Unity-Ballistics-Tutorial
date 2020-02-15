using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Attach this to a GUI text

public class FPS : MonoBehaviour {
	//How often to update the fps text
	public float updateInterval = 0.5f;

	//FPS accumulated over the interval
	private float accum = 0f;
	//Frames drawn over the interval
	private int frames = 0; 
	//Left time for current interval
	private float timeleft; 

	private Text TextFPS;


	//StringBuilder is faster than concatenation
	System.Text.StringBuilder sb = new System.Text.StringBuilder();


	void Start() {
		TextFPS = GetComponent<Text>();

		timeleft = updateInterval;  
	}
	

	void Update() {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		frames++;
		
		//Interval ended - update GUI text and start new interval
		if (timeleft <= 0f) {
			//FPS
			sb.Length = 0;
			//Add text
			sb.Append(" FPS: ");
			sb.Append((accum/frames).ToString("f0"));
			//Display
			TextFPS.text = sb.ToString();


			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
