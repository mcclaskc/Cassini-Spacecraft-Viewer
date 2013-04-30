using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class TimeManager : MonoBehaviour {
	
	public float currTimeStep;				//Current time step with any modifiers (i.e. pause)
	public float defaultTimeStep;			//Default base time step before any modifiers are added
	private float updateRate = .1f; //updateRate in seconds, how often the target changes
	private float nextUpdate = 0.0f; //placeholder that tracks how much time passes

	public float minTime;					//Where the current time period begins
	public float maxTime;					//Where the current time period ends
	public DateTime resetToTime;			//Where the playhead started in this current timeframe
	public DateTime currTime;				//The current time
	public Timeline timeline;				//Link to the timeline so playhead can be accessed
	
	private bool isPlaying;
	private bool isReversing;
	
	private GameObject[] mobileBodies;


	// Use this for initialization
	void Start () {
		isPlaying = false;
		isReversing = false;
	    //This may need to be changed:
		resetToTime = currTime = timeline.GetCurrentPlayhead();
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
		Debug.Log("TimeManager: Start Time = " + currTime);
	}
	
	void Update(){
		if(Time.time > nextUpdate){
			if(isPlaying || isReversing){
				if(isPlaying){
					currTime = currTime.AddMinutes(1);
					Debug.Log("In Play: " + currTime);
				}
				else if(isReversing){
					if(currTime.AddMinutes(-1) > resetToTime)
						currTime = currTime.AddMinutes(-1);	
				}
				nextUpdate = Time.time + updateRate;
				timeline.SetCurrentPlayhead(currTime);
			}	
		}
	}
	
	//Signal reciever
	void Play(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Play");
		}
		isPlaying = !isPlaying;
		isReversing = false;
	}
	//Signal reciever
	void Reverse(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reverse");
		}
		isReversing = !isReversing;
		isPlaying = false;
	}
	//Signal reciever
	void Reset(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reset");
		}
		isPlaying = isReversing = false;
		currTime = resetToTime;
		timeline.SetCurrentPlayhead(currTime);
	}
	
	
	
}
