using UnityEngine;
using System.Collections;

public class windowManagerScript : MonoBehaviour {
	
	public UIPopupList planetList;
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
	
	void OnPlanetSelectionChange(){
		viewerCamera.enabled = false;
		currentViewer = GameObject.Find(planetList.selection + "Viewer");
		viewerCamera = currentViewer.GetComponent<Camera>();
		viewerCamera.enabled = true;
		Debug.Log("Selection: " + planetList.selection + "Viewer" + " Actual: " + viewerCamera);
	}
	
	void OnPlanetCameraActivate(){
		viewerCamera.enabled = !viewerCamera.enabled;	
	}
}
