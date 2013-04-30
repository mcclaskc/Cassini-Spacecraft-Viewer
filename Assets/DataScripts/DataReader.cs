using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
public class DataReader : MonoBehaviour {
	
	public FileLoader fileloader;
	
	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp (KeyCode.A))
		{
			DateTime Start = new DateTime(2009,6,22,12, 0,0);
			DateTime End = new DateTime(2009,6,22,19,0,0);
			Debug.Log (Start);
			Debug.Log (End);
			List<EphemerisData> Data = fileloader.GetEphemeris(Start, End, "titan");
			for(int i = 0; i < Data.Count; i++)
			{
				Debug.Log (Data[i].position + "  " + Data[i].time);
			}
			Debug.Log ("Data Size:" + Data.Count);
		}
	}
}
