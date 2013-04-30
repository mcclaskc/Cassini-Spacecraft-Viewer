using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {
	
	public float currTimeStep;				//Current time step with any modifiers (i.e. pause)
	public float defaultTimeStep;			//Default base time step before any modifiers are added
	
	public float minTime;					//Where the current time period begins
	public float maxTime;					//Where the current time period ends
	public float currTime;					//The current time
	
	
	private GameObject[] mobileBodies;


	// Use this for initialization
	void Start () {
	
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
		
	}
	
	//Signal reciever
	void Play(){
		foreach (GameObject mover in mobileBodies){
			mover.SendMessage ("Play");
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
