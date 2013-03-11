using UnityEngine;
using System.Collections;

/*
 * This script ensures that the camera will stay on the 
 * vector between Cassini and the current target object 
 */

public class VectorFollow : MonoBehaviour {
	
	public Transform cassini;  //Cassini's transform
	public Transform target;  //Viewer camera's current target 
	public Camera self;
	
	private float radius = 1.0f;
	

	
	// Update is called once per frame
	void Update () {
		//Determine the vector from target to cassini
		float desiredX = cassini.position.x - target.position.x;
		float desiredY = cassini.position.y - target.position.y;
		float desiredZ = cassini.position.z - target.position.z;
		Vector3 desiredVector = new Vector3(desiredX,desiredY,desiredZ);
		//Normalize and then scale vector to desired value
		desiredVector.Normalize();
		Vector3 scaledVector =  radius * desiredVector;
		//Set viewer's position to the desired position
		gameObject.transform.position = scaledVector + target.transform.position;
		gameObject.transform.LookAt(target);
	}
	
			
	//Call this function to set a new target		
	public void SetTarget(string newTarget){
		//Set our new target
		target = GameObject.Find(newTarget).transform;
		//Grab that target's data
		ViewerAttributes targetAttributes = target.GetComponent<ViewerAttributes>();
		//Set camera's FOV and desired radius
		radius = targetAttributes.radius;
		self.fov = targetAttributes.fov;
		Debug.Log("Set viewer target to " + newTarget + ". Radius is now " + radius);
	}
}
