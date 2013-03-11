using UnityEngine;
using System.Collections;

/// <summary>
/// Class: POVCamera
/// Author: Jacob Rieger
/// Last Modified: 11-Mar-2013
/// Purpose:  This script handles the camera controls for POV
/// Usage:  Attach this script to your main camera and parent the main camera to 
/// the object you want to have perspective from. Your camera will change it's view
/// in the direction you drag your mouse while clicking.
/// </summary>
public class POVCamera : MonoBehaviour {
	
	//Variables to control panning speeds
	public float xSpeed;
	public float ySpeed;
	
	//Inverts the y controls, set in inspector
	//Could add selection for this in GUI
	public bool invert = false;
	
	//Variables for camera angles
	private float x = 0.0f;
	private float y = 0.0f;
	
	//Variable for Camera control
	private Camera main;

	// Use this for initialization
	void Start () {
		
		//Default speeds if inspector isn't set
		if(xSpeed == 0) xSpeed = 120;
		if(ySpeed == 0) ySpeed = 200;
		
		//Initilize our angles
		var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
		
		main = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Only reacts to mouse movement if mouse is clicked
		if(Input.GetMouseButton(0)){
			
			//Set the camera's angles according to mouse movement
			x += Input.GetAxis("Mouse X") * xSpeed *Time.deltaTime;
			if(invert)y += Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
			else y -= Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
			
			transform.rotation = Quaternion.Euler(y, x, 0);	
		}
	}
}
