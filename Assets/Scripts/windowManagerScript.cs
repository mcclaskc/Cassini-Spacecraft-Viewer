using UnityEngine;
using System.Collections;

public class windowManagerScript : MonoBehaviour {
	
	public UIPopupList planetList;
	public UIPopupList sensorList;
	public GameObject currentViewer;
	private Camera viewerCamera;
	
	private bool pipMain = false;  //False if pip is NOT main screen
	private float lastPipSwitch;
	
	// Use this for initialization
	void Start () {
		//Initialize the PiP camera
		currentViewer = GameObject.Find(planetList.selection+"Viewer");
		viewerCamera = currentViewer.GetComponent<Camera>();
		viewerCamera.enabled = true;
		Debug.Log(viewerCamera);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		RaycastHit hitObj;		//Holder for any objects hit by a ray
		
		//Determine if the user right-clicked anywhere in the scene
		if(Input.GetAxis("UserSelect") > 0f){
			
			
			//If the user clicked within the PiP and it has been more that .3 seconds since the last time, switch the two cameras
			if((Input.mousePosition.x < .3f * Screen.width) && (Input.mousePosition.y > .7f * Screen.height) && (Time.time - lastPipSwitch > .3f)){
				if(!pipMain){
					Camera.main.pixelRect = new Rect(0f,.7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
					Camera.main.depth = 0;
					viewerCamera.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
					viewerCamera.depth = -1;
					pipMain = !pipMain;
					lastPipSwitch = Time.time;
				} else {
					Camera.main.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
					Camera.main.depth = -1;
					viewerCamera.pixelRect = new Rect(0f, .7f * Screen.height, .3f * Screen.width, .3f * Screen.height);
					viewerCamera.depth = 0;
					pipMain = !pipMain;
					lastPipSwitch = Time.time;
				}
				Debug.Log("User clicked in the PiP");
			} else {
			
				//Translate current mouse position to a ray
				Ray mouseRay = 	Camera.main.ScreenPointToRay(Input.mousePosition);
				//Use that ray to determine if the user was clicking on a body of interest
				if(Physics.Raycast(mouseRay, out hitObj, 1000f)){
					GameObject hitBody = hitObj.transform.gameObject;
					//Check if we hit something with a viewer attached
					if(GameObject.Find(hitBody.name + "Viewer") != null){	
						//If we did, then change the current picture in picture to that object
						viewerCamera.enabled = false;
						currentViewer = GameObject.Find(hitBody.name + "Viewer");
						viewerCamera = currentViewer.GetComponent<Camera>();
						viewerCamera.enabled = true;
						Debug.Log("Hit " + hitObj.transform.gameObject.name);	
					}
				}
			}
		}
	}
	
	//This function will be called when a different planet is selected through the drop-down by the user
	void OnPlanetSelectionChange(){
		viewerCamera.enabled = false;
		currentViewer = GameObject.Find(planetList.selection + "Viewer");
		viewerCamera = currentViewer.GetComponent<Camera>();
		viewerCamera.enabled = true;
		Debug.Log("Selection: " + planetList.selection + "Viewer" + " Actual: " + viewerCamera);
	}
	
	//This function will be called when a different sensor is requested by the user
	void OnSensorSelectionChange(){
		
	}
	
	void OnPlanetCameraActivate(){
		viewerCamera.enabled = !viewerCamera.enabled;	
	}
}
