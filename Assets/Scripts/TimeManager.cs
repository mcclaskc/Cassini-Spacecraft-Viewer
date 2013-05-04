using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class TimeManager : MonoBehaviour {
	
	public float currTimeStep;				//Current time step with any modifiers (i.e. pause)
	public float defaultTimeStep;			//Default base time step before any modifiers are added
	private float updateRate = .1f; 		//updateRate in seconds, how often the target changes
	private float nextUpdate = 0.0f; 		//placeholder that tracks how much time passes
	private float nextUIUpdate = 0.0f;  	//Use to make sure that user input is only accepted once
	
	public float minTime;					//Where the current time period begins
	public float maxTime;					//Where the current time period ends
	public DateTime resetToTime;			//Where the playhead started in this current timeframe
	public DateTime currTime;				//The current time
	public Timeline timeline;				//Link to the timeline so playhead can be accessed
	public GameObject windowManager;		//Link to the windowManager so any time-based changes to windows can be applied
	
	private DateTime[] visibleTime;			//The current visible range, null until user moves playhead
	private float timelineHeight;			//The height of the timeline.  Grabbed at start, never again
	
	private bool isPlaying;					//True if the scene is playing, should not be true if isReversing is true
	private bool isReversing;				//True if the scene is reversing, should not be true if isPlaying is true
	
	private GameObject[] mobileBodies;		//Array of all bodies in the scene with "Mobile" tag

	// Use this for initialization
	void Start () {
		//Get the height of the timeline
		timelineHeight = timeline.timelineHeight;
		//Nothing should be moving at the start
		isPlaying = false;
		isReversing = false;
	    //This may need to be changed in production:
		resetToTime = currTime = timeline.GetCurrentPlayhead();
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
		Debug.Log("TimeManager: Start Time = " + currTime);
	}
	
	//Currently using FixedUpdate for UIInput as it lowers number of calls 
	//and should work on the vast majority of machines
	void FixedUpdate(){
		//Detect user input, but make sure it's only once per click
		if(nextUIUpdate < Time.time){
			//NOTE: This uses the input axis setup, NOT the hardcoded buttons
			//This means it can easily be changed in the Edit>Project Settings>Axis menu
			if((Input.GetAxis("PlayheadMove")>.99f) && (Input.mousePosition.y < (timelineHeight*.8))){
		  		//Grab the current state of the Timeline, note that the getter returns a datetime array
				visibleTime = timeline.GetVisibleRange();
				//Figure out the current time range
				TimeSpan visibleRange = visibleTime[1] - visibleTime[0];
				//Determine offset to use and use it
				double mouseTime = Input.mousePosition.x * (visibleRange.TotalDays/Screen.width);
				currTime = resetToTime = visibleTime[0].AddDays(mouseTime);
				//Update the playhead
				timeline.SetCurrentPlayhead(currTime);
				//Update the data
				updateData(currTime);
				
				//Stop playing to avoid potential update issues
				isPlaying = isReversing = false;
				//Make sure everyone gets the message
				foreach (GameObject mover in mobileBodies){
					mover.SendMessage ("Reset");
				}
				windowManager.SendMessage("Stop");
				nextUIUpdate = Time.time + updateRate;
				Debug.Log("PlayheadMove: " + currTime);
			}
		}
	}
	
	void Update(){
		//Make sure we aren't playing too quickly
		if(Time.time > nextUpdate){
			if(isPlaying || isReversing){
				//If scene is playing, step forward in time by 1 minute
				if(isPlaying){
					currTime = currTime.AddMinutes(1);
					Debug.Log("In Play: " + currTime);
				}
				//If scene is playing step backward in time by 1 minute
				else if(isReversing){
					if(currTime.AddMinutes(-1) > resetToTime)
						currTime = currTime.AddMinutes(-1);	
				}
				nextUpdate = Time.time + updateRate;
				//Update the playhead
				timeline.SetCurrentPlayhead(currTime);
			}	
		}
	}
	
	//Placeholder function, end function should send out a request to update
	//all data to the new range
	void updateData(DateTime newCurrentTime){
		//Stuff to call goes here	
	}
	
	//Signal reciever for the play button
	void Play(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Play");
		}
		isPlaying = !isPlaying;
		//Make sure we can't play and reverse at the same time
		isReversing = false;
	}
	//Signal reciever for the reverse button
	void Reverse(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reverse");
		}
		isReversing = !isReversing;
		//Make sure we can't play and reverse at the same time
		isPlaying = false;
	}
	//Signal reciever for the reset button
	void Reset(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reset");
		}
		//Stop motion when we reset
		isPlaying = isReversing = false;
		currTime = resetToTime;
		timeline.SetCurrentPlayhead(currTime);
	}
	
	
	
}
