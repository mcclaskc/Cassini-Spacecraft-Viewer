using UnityEngine;
using System.Collections;

public class TitanViewer : MonoBehaviour {
	
	public int l1data = 12345;
	public int l2data = 54321;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {	
	}
	void OnGUI(){
		GUI.Label(new Rect(Screen.width*0.3f,0.0f,
			Screen.width*0.7f,Screen.height*.3f), "Titan UVIS\nL1: " + l1data + "\nL2: " + l2data);
	}
}
