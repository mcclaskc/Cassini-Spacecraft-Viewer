using UnityEngine;
using System.Collections;
 
/// <summary>
/// Class: ObjectLabel
/// Author: Jacob Rieger
/// Last Modified: 11-Mar-2013
/// Purpose: This Script displays a GUIText element that follows your object.
/// Usage:  This script is part of the Planet Label prefab. This script expects a GUIText element to
/// be present on the same GameObject that it is apart of. It will assume the object you wish to display
/// a label over is the root parent. You can set the label name in the inspector, however if you leave it unset
/// it will use the root parents name.
/// </summary>
[RequireComponent (typeof (GUIText))]
public class ObjectLabel : MonoBehaviour {
	
 
	public string LabelName; 			   // Name that can be set, if unset uses Root parent name instead
	public Transform target; 		       // Object that this label should follow
	public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;     // If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;      // Use the camera tagged MainCamera
	public Camera cameraToUse ;   		   // Only use this if useMainCamera is false
	public GUIText guitext; 			   //Our GuiText that is attached to the object
	Camera cam ;
	Transform thisTransform;
	Transform camTransform;
 
	void Start () 
    {
	    thisTransform = transform;
	    if (useMainCamera)
			{
	        	cam = Camera.main;
				camTransform = cam.transform;
			}
	    else
			{
	        	cam = cameraToUse;
	   			camTransform = cam.transform;
			}
		//Set the target
		target = transform.root;
		//Set the object Label Name
		if(LabelName.Equals ("")) guitext.text = transform.root.name;
		else guitext.text = LabelName;
		//thisTransform.position = cam.WorldToScreenPoint(target.position);
	}
 
 
    void Update()
    {   
		if (clampToScreen)
        {
            Vector3 relativePosition = camTransform.InverseTransformPoint(target.position);
			
            relativePosition.z =  Mathf.Max(relativePosition.z, 1.0f);
			
            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition + offset));
			
            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize),
                                             Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize),
                                             thisTransform.position.z);
 
        }
        else
        {
            thisTransform.position = cam.WorldToViewportPoint(target.position + offset);
        }
        
    }
}