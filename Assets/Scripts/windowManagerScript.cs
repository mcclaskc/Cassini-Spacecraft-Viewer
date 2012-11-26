using UnityEngine;
using System.Collections;

public class windowManagerScript : MonoBehaviour {
	
	public UIPopupList planetList;
	public UIPopupList sensorList;
	public GameObject currentViewer;
	private Camera viewerCamera;
	
	// Use this for initialization
	void Start () {
		currentViewer = GameObject.Find(planetList.selection+"Viewer");
		viewerCamera = currentViewer.GetComponent<Camera>();
		viewerCamera.enabled = true;
		Debug.Log(viewerCamera);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//This function will be called when a different planet is selected by the user
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
