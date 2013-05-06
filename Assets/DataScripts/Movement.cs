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
	

	public DataAccess fileloader; 	 //This needs to be set to the object in scene that has FileLoader on it
	private Vector3 target; 		 //This is updated after every movement while playing
	private float updateRate = .1f; //updateRate in seconds, how often the target changes
	private float nextUpdate = 0.0f; //placeholder that tracks how much time passes
	private bool play = false;       //whether or not to keep updating the target
	private bool reverse = false;	 //whether or not we are going in reverse
	private int iterator; 			 //iterator for counting the updates
	List<EphemerisData> Data = null; 		 //Compiled list of targets
	
	void Start () {
		//Initialize the iterator
		iterator = 0;
	
	}
	
	void Update () {
		
		/*
		//Smooth movement when playing quickly, choppy if slow
		transform.position = target;
		
		//Only changes target if enough time has passed and play is enabled
		if(Time.time > nextUpdate)
		{
			if(play){
				//Sets next update with updateRate
				nextUpdate = Time.time + updateRate;
				//New target from Data
				target = Data[iterator].position;
				iterator++;
			} else if(reverse && iterator != 0){
				//Sets next update with updateRate
				nextUpdate = Time.time + updateRate;
				//New target from Data
				iterator--;
				target = Data[iterator].position;
			}
		}
		*/
	}
	
	public void Loaded(NetworkAccess.EphemBlock block){
		
		//This function is intened to be called by the timeline to let this object know when it should
		//load new data. 
		Data = block.getData(transform.name);
	}
	
	void UpdatePlayhead(DateTime time) {
		//Get the closest time from the block of data
		if(Data != null) {
			Vector3 pos = Vector3.zero;
			long closest = long.MaxValue;
			foreach(EphemerisData d in Data) {
				long tickTime = Math.Abs((d.time - time).Ticks);
				if(tickTime < closest) {
					closest = tickTime;
					pos = d.position;
				}
			}

			transform.position = pos;
		}
	}
	
	//Signal reciever
	void Play(){
		if(play){
			play = false;
		}
		else{
			play = true;
		}
	}
	
	//Signal reciever
	void Reverse(){
		if(play && !reverse) play = false;
		reverse = !reverse;
	}
	
	//Signal reciever
	void Reset(){
		nextUpdate = 0.0f;
		iterator = 0;
		//target = Data[iterator].position;
		transform.position = target;
		play = reverse = false;
	}
	
}
