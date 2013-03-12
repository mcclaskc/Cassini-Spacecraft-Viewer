using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
/// <summary>
/// Class: Movement
/// Author: Jacob Rieger
/// Last Modified: 12-Mar-2013
/// Purpose:  This script holds the position values for the gameobject it is attached to and also
/// drives it's movement through the scene.
/// Usage: Attach this script to the gameobject you want to send position data to. You will also have to set
/// the tag of the gameobject to "mobile" or else the FileLoader won't send the play data. 
/// </summary>
public class Movement : MonoBehaviour {
	

	public FileLoader fileloader; 	 //This needs to be set to the object in scene that has FileLoader on it
	private Vector3 target; 		 //This is updated after every movement while playing
	private float updateRate = 1.0f; //updateRate in seconds, how often the target changes
	private float nextUpdate = 0.0f; //placeholder that tracks how much time passes
	private bool play = false;       //whether or not to keep updating the target
	private int iterator; 			 //iterator for counting the updates
	List<EphemerisData> Data; 		 //Compiled list of targets
	
	void Start () {
		//Debugging purposes
		Debug.Log (transform.name);
		//Initialize the iterator
		iterator = 0;
	
	}
	
	void Update () {
		//Changes the position to movetowards the next target (next data pos in the binary)
		
		//Causes bodies to move "jumpily"
		//transform.position = Vector3.MoveTowards(transform.position, target, .3f);
		
		transform.position = target;
		
		//Only changes target if enough time has passed and play is enabled
		if(Time.time > nextUpdate && play)
		{
			//Sets next update with updateRate
			nextUpdate = Time.time + updateRate;
			//New target from Data
			target = Data[iterator].position;
			iterator++;
		}
	}
	
	void Loaded(DateTime[] StartEnd){
		
		//This function is intened to be called by the timeline to let this object know when it should
		//load new data. With the provided start and end, it will call the fileloader's GetEphemeris to
		//receive it's new data.
		DateTime Start = StartEnd[0];
		DateTime End = StartEnd[1];
		iterator = 0;
		
		Debug.Log ("Received " + Start + " as Start Date");
		Debug.Log ("Received " + End + " as End Date");
		//Debug.Log (transform.name);
		
		//Fileloader returns a nice list of our data
		Data = fileloader.GetEphemeris(Start, End, transform.name);
		
		//Prints to the console of all our Data
		//Used to confirm correct data being used
		/*
		for(int i = 0; i < Data.Count; i++)
		{
			
			Debug.Log (Data[i].position + "  " + Data[i].time);
			
		}
		*/
		//Sets you to the first position of the aquired data
		//Target set to yourself as well
		transform.position = Data[0].position;
		target = Data[0].position;
	}
	
	void Play(){
		//This function is intended to be called by the timeline
		if(play){
			play = false;
		}
		else{
			play = true;
		}
	}
}
