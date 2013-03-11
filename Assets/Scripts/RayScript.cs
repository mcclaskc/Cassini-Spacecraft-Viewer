using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

/*
 * Script to be attached to rays.  Requires user to set a scale and target in inspector.
 */

public class RayScript : MonoBehaviour {
	
	
	public Transform myTarget;	  //What this ray is pointing to
	public float scale;			  //Value used to scale the length of the ray
	private LineRenderer lineRender;  //Own line renderer
	

	// Use this for initialization
	void Start () {
		//Grab own line render once so we don't have to do it again
		lineRender = gameObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//Get the vector from the target's position to own position
		Vector3 targetVec = myTarget.position - transform.position;
		//Normalize that vector
		targetVec.Normalize();
		//Set the line renderer to point towards it
		//Uses the scale value to determine its size
		lineRender.SetPosition(1,scale*targetVec);
	}
	
	
	
}
