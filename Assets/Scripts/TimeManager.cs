using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {
	
	public float currTimeStep;				//Current time step with any modifiers (i.e. pause)
	public float defaultTimeStep;			//Default base time step before any modifiers are added
	
	public float minTime;					//Where the current time period begins
	public float maxTime;					//Where the current time period ends
	public float currTime;					//The current time
	
	public UISlider timeBar;				//Time bar to send updates to when time changes
	
	private GameObject[] mobileBodies;


	// Use this for initialization
	void Start () {
	
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyUp(KeyCode.E)){
			
			foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Play");
		}
			
		}
	}
	
	//This function is called by the NGUIEventPasser when the play button is clicked.
	void btnPlayClick(){
		Debug.Log("Play was clicked");
		//Set the current time step to whatever the normal is
		currTimeStep = defaultTimeStep;
	}
	
	//This function is called by the NuguiEventPasser when the pause button is clicked.
	void btnPauseClick(){
		Debug.Log("Pause was clicked");	
		//Set the current time step to 0
		currTimeStep = 0f;
	}
	
	//This function is called whenever the slider is changed
	void OnSliderChange(float currValue){
		Debug.Log("Current Step is " + currValue);
		currTime = (currValue * defaultTimeStep) + minTime;
	}
}
