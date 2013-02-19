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
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float desiredX = cassini.position.x - target.position.x;
		float desiredY = cassini.position.y - target.position.y;
		float desiredZ = cassini.position.z - target.position.z;
		
		Vector3 desiredVector = new Vector3(desiredX,desiredY,desiredZ);
		desiredVector.Normalize();
		Vector3 scaledVector =  radius * desiredVector;
		gameObject.transform.position = scaledVector + target.transform.position;
		gameObject.transform.LookAt(target);
	}
	
			
	//Call this function to set a new target		
	void SetTarget(string newTarget){
		
		target = GameObject.Find(newTarget).transform;
		ViewerAttributes targetAttributes = target.GetComponent<ViewerAttributes>();
		radius = targetAttributes.radius;
		self.fov = targetAttributes.fov;
		Debug.Log("Set viewer target to " + newTarget + ". Radius is now " + radius);
	}
}
