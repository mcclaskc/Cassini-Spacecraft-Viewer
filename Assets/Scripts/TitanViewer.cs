using UnityEngine;
using System.Collections;

public class TitanViewer : MonoBehaviour {
	
	public int tdata = 12345;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGui(){
		GUI.Box (new Rect (0,0,100,50), "Titan Data");
		GUI.Label(new Rect(0,0,256,256), "Titan UVIS: " + tdata);
		Debug.Log("In OnGui()");
	}
}
