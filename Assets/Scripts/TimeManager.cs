using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class TimeManager : MonoBehaviour {
	
	public float currTimeStep;				//Current time step with any modifiers (i.e. pause)
	public float defaultTimeStep;			//Default base time step before any modifiers are added
	
	public float minTime;					//Where the current time period begins
	public float maxTime;					//Where the current time period ends
	public DateTime currTime;				//The current time
	public Timeline timeline;				//Link to the timeline so playhead can be accessed
	
	
	private GameObject[] mobileBodies;


	// Use this for initialization
	void Start () {
	    //This may need to be changed:
		currTime = timeline.GetCurrentPlayhead();
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
		Debug.Log("TimeManager: Start Time = " + currTime);
	}
	
	//Signal reciever
	void Play(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Play", currTime);
		}
	}
	//Signal reciever
	void Reverse(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reverse");
		}
	}
	//Signal reciever
	void Reset(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Reset");
		}
	}
	
	
	
}
