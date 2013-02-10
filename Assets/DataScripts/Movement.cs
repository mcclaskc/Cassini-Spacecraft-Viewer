using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Movement : MonoBehaviour {
	

	public FileLoader fileloader;
	private Vector3 target;
	private float updateRate = 1.0f;
	private float nextUpdate = 0.0f;
	private bool play = false;
	private int iterator;
	List<EphemerisData> Data;
	
	// Use this for initialization
	void Start () {
		iterator = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = Vector3.MoveTowards(transform.position, target, 100f);
		
		if(Time.time > nextUpdate && play)
		{
			nextUpdate = Time.time + updateRate;
			target = Data[iterator].position;
			iterator++;
		}
	}
	
	void Loaded(){
		
		DateTime Start = new DateTime(2009,6,22,12, 0,0);
		DateTime End = new DateTime(2009,6,22,15,0,0);
		Debug.Log (Start);
		Debug.Log (End);
		Data = fileloader.GetEphemeris(Start, End, "titan");
		
		for(int i = 0; i < Data.Count; i++)
		{
			Debug.Log (Data[i].position + "  " + Data[i].time);
		}
		transform.position = Data[0].position;
		target = Data[0].position;
	}
	
	void Play(){
		
		if(play){
			play = false;
		}
		else{
			play = true;
		}
	}
}
