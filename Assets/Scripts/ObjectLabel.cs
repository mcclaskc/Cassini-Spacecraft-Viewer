using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (GUIText))]
public class ObjectLabel : MonoBehaviour {
 
	public Transform target;  // Object that this label should follow
	public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;   // Use the camera tagged MainCamera
	public Camera cameraToUse ;   // Only use this if useMainCamera is false
	public GUIText guitext; //Our GuiText that is attached to the object
	Camera cam ;
	Transform thisTransform;
	Transform camTransform;
 
	void Start () 
    {
		Debug.Log ("On Start Called");
		//Set the camera
	    thisTransform = transform;
	    if (useMainCamera)
			{
	        	cam = Camera.main;
			}
	    else
			{
	        	cam = cameraToUse;
	   			camTransform = cam.transform;
			}
		//Set the target
		target = transform.root;
		//Set the object Label Name
		guitext.text = transform.root.name;
		Debug.Log (transform.root.name + " IS THE ROOT NAME");
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