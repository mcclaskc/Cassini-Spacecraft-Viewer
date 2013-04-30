using UnityEngine;
using System.Collections;

/*
 * This script manages the various windows.  It expects to have a direct link to a viewer for Cassini,
 * as well as a general purpose viewer.
 */

public class windowManagerScript : MonoBehaviour {
	
//	public UIPopupList planetList;
//	public UIPopupList sensorList;
	public GameObject currentViewer;
	public GameObject cassiniViewer;
	private Camera viewerCamera;
	//private VectorFollow viewerScript;
	
	private bool pipIsNotMain = false;  //False if pip is NOT main screen
	private float lastPipSwitch;		//Use to keep track of when the user last did something
	
	//Initialize the PiP camera here
	void Start () {
		//Grab the viewerScript once since it should not change
		//viewerScript = currentViewer.GetComponent<VectorFollow>();
		//Grab the viewer camera so we don't have to do it again
		viewerCamera = currentViewer.GetComponent<Camera>();
		//Make sure the camera is displaying to the screen
		viewerCamera.enabled = true;
		//Update it with the default selection
		//currentViewer.SendMessage("SetTarget", planetList.selection);
	}
	
	// Update is called once per frame, use to update positions
	void Update() {
		
		RaycastHit hitObj;		//Holder for any objects hit by a ray
		
		//Determine if the user right-clicked anywhere in the scene
		if(Input.GetAxis("UserSelect") > 0f){
			
			
			//If the user clicked within the PiP and it has been more that .3 seconds since the last time, switch the two cameras
			if((Input.mousePosition.x < .3f * Screen.width) && (Input.mousePosition.y > .7f * Screen.height) && (Time.time - lastPipSwitch > .3f)){
				if(!pipIsNotMain){
					//Swap depth and screen display coordinates of the PiP and main cameras
					Camera.main.pixelRect = new Rect(0f,.7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
					Camera.main.depth = 1;
					viewerCamera.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
					viewerCamera.depth = -1;
					pipIsNotMain = !pipIsNotMain;
					lastPipSwitch = Time.time;
				} else {
					//Swap depth and screen display coordinates of the PiP and main cameras
					Camera.main.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
					Camera.main.depth = -1;
					viewerCamera.pixelRect = new Rect(0f, .7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
					viewerCamera.depth = 1;
					pipIsNotMain = !pipIsNotMain;
					lastPipSwitch = Time.time;
				}
				Debug.Log("User clicked in the PiP");
				
			//If the user clicks within the center area of the screen (i.e. where Cassini will always be) then the cassini viewer is either enabled or disabled
			} else if ((Input.mousePosition.x < .52f * Screen.width) && (Input.mousePosition.x > .48f * Screen.width)
							&& (Input.mousePosition.y < .52f * Screen.height) && (Input.mousePosition.y > .48f * Screen.height) && (Time.time - lastPipSwitch > .3f)){
				cassiniViewer.GetComponent<Camera>().enabled = !cassiniViewer.GetComponent<Camera>().enabled;
				lastPipSwitch = Time.time;	
				Debug.Log("User clicked on Cassini");				
			} else {
			
				//Translate current mouse position to a ray
				Ray mouseRay = 	Camera.main.ScreenPointToRay(Input.mousePosition);
				//Use that ray to determine if the user was clicking on a body of interest
				if(Physics.Raycast(mouseRay, out hitObj, 1000f)){
					GameObject hitBody = hitObj.transform.gameObject;
					
					//Check if we hit something with a viewer attached
					if(GameObject.Find(hitBody.name + "Viewer") != null){	
						//If we did, then change the current picture in picture to that object
						currentViewer.SendMessage("SetTarget", hitBody.name);
						if(!pipIsNotMain){
							//Sanity check to make sure camera is where we expect it to be
							viewerCamera.pixelRect = new Rect(0f, .7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
							viewerCamera.depth = 1;
						} else {
							//Sanity check to make sure camera is where we expect it to be
							viewerCamera.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
							viewerCamera.depth = -1;
						}
						Debug.Log("Hit " + hitObj.transform.gameObject.name);	
					}
				}
			}
		}
	}
	
	//This function will be called when a different planet is selected through the drop-down by the user
	void OnPlanetSelectionChange(){
		if(!pipIsNotMain){
			//Change the current object being viewed to whatever the user selected
			//currentViewer.SendMessage("SetTarget", planetList.selection);
			//Sanity check to make sure camera is where it should be
			viewerCamera.pixelRect = new Rect(0f, .7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
			viewerCamera.depth = 1;
			//Debug.Log("Selection: " + planetList.selection + "Viewer" + " Actual: " + viewerCamera);
		} else {
			//Change the current object being viewed to whatever the user selected
			//currentViewer.SendMessage("SetTarget", planetList.selection);
			//Sanity check to make sure camera is where it should be
			viewerCamera.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
			viewerCamera.depth = -1;
			//Debug.Log("Selection: " + planetList.selection + "Viewer" + " Actual: " + viewerCamera);
		}
	}
	
	//This function will be called when a different sensor is requested by the user
	void OnSensorSelectionChange(){
		
	}
	
	void OnPlanetCameraActivate(){
		viewerCamera.enabled = !viewerCamera.enabled;	
	}
}
