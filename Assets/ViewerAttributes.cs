using UnityEngine;
using System.Collections;

/*
 *	This script holds the viewer info specific to this body 
 */

public class ViewerAttributes : MonoBehaviour {
	public float radius;  //Raduis that the camera will be set to, should NOT be the object's radius
	public float fov;     //FOV that the camera should be set to when viewing this object

}
