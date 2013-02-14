using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Movement : MonoBehaviour {
	

	public FileLoader fileloader; //This needs to be set to the object in scene that has FileLoader on it
	private Vector3 target; //This is updated after every movement while playing
	private float updateRate = 1.0f; //updateRate in seconds, how often the target changes
	private float nextUpdate = 0.0f; //placeholder that tracks how much time passes
	private bool play = false; //whether or not to keep updating the target
	private int iterator; //iterator for counting the updates
	List<EphemerisData> Data; //Compiled list of targets
	
	// Use this for initialization
	void Start () {
		//Debugging purposes
		Debug.Log (transform.name);
		//Initialize the iterator
		iterator = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		//This function gets called continously
		//Changes the position to movetowards the next target (next data pos in the binary)
		transform.position = Vector3.MoveTowards(transform.position, target, .3f);
		
		
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
	
	void Loaded(){
		
		//This is the Data we request in the begining
		//These values will need to come from the timeline
		DateTime Start = new DateTime(2009,6,22,12, 0,0);
		DateTime End = new DateTime(2009,6,22,15,0,0);
		
		//Debug.Log (Start);
		//Debug.Log (End);
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
		//This function is called by the timebar
		//Sets the play variable
		if(play){
			play = false;
		}
		else{
			play = true;
		}
	}
}
