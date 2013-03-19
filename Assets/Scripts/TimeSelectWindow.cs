using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class TimeSelectWindow : MonoBehaviour {
	
	public float timeBarHeight = 0f;  	//Relative height of the time bar.  Currently assumes bar is at bottom of screen
	public GameObject timeline;			//The scene's timeline should be linked here
	
	private Timeline currTimeline;		//This is the Timeline script attached to the linked timeline
	
	public float windowHeight = 0f;		//Relative height of the window.
	public float windowWidth = 0f;		//Relative width of the window.
	

	private Rect windowRect;
	private bool showWindow = false;  	//If this is true, will show the window

	
	private string startMonth = "MM";
	private string startDay = "DD";
	private string startYear = "YYYY";
	private string startHour = "hh";
	private string startMin = "mm";
	private string startSec = "ss";
	
	private string endMonth = "MM";
	private string endDay = "DD";
	private string endYear = "YYYY";
	private string endHour = "hh";
	private string endMin = "mm";
	private string endSec = "ss";
	
	private DateTime startTime;
	private DateTime endTime;
	
	private bool error = false;					//True if an error is caught

	// Use this for initialization
	void Start () {
		windowRect = new Rect(20,Screen.height/3,windowWidth*Screen.width,windowHeight*Screen.height);
		currTimeline = timeline.GetComponent<Timeline>();
	}
	
	// Update is called once per frame
	void Update () {
		//Toggle the window if user right clicks in the time bar
		if(Input.GetAxis("UserSelect") > 0f){
			if(Input.mousePosition.y < (timeBarHeight * Screen.height)){
				showWindow = true;
			}
		}
	}
	
	void OnGUI(){
		if(showWindow){
			windowRect = GUI.Window(0,windowRect,TimeWindow,"Select Time");
		}
	}
	
	//This function is called when the window is created by Unity
	void TimeWindow(int windowID){
		//Holders to reduce number of references
		float ownHeight = windowRect.height;
		float ownWidth = windowRect.width;
		

		
		//Create labels telling the user what info needs to be provided
		GUI.Label(new Rect(20,(1.5f*ownHeight/10),100,20),"Start Time");
		GUI.Label(new Rect(20,(3*ownHeight/10),100,20),"End Time");
		
		//Create textfields to recieve user input for the start time, (mm/dd/yyyy)
		startHour = GUI.TextField(new Rect((2*ownWidth/10),(1.5f*ownHeight/10),30,20),startHour,2);
		startMin = GUI.TextField(new Rect((2.75f*ownWidth/10),(1.5f*ownHeight/10),30,20),startMin,2);
		startSec = GUI.TextField(new Rect((3.5f*ownWidth/10),(1.5f*ownHeight/10),30,20),startSec,2);
		startMonth = GUI.TextField(new Rect((4.5f*ownWidth/10),(1.5f*ownHeight/10),30,20),startMonth,2);
		startDay = GUI.TextField(new Rect((5.25f*ownWidth/10),(1.5f*ownHeight/10),30,20),startDay,2);
		startYear = GUI.TextField(new Rect((6*ownWidth/10),(1.5f*ownHeight/10),60,20),startYear,4);
		
		//Create textfields to recieve user input for the end time, (mm/dd/yyyy)
		endHour = GUI.TextField(new Rect((2*ownWidth/10),(3*ownHeight/10),30,20),endHour,2);
		endMin = GUI.TextField(new Rect((2.75f*ownWidth/10),(3*ownHeight/10),30,20),endMin,2);
		endSec = GUI.TextField(new Rect((3.5f*ownWidth/10),(3*ownHeight/10),30,20),endSec,2);
		endMonth = GUI.TextField(new Rect((4.5f*ownWidth/10),(3*ownHeight/10),30,20),endMonth,2);
		endDay = GUI.TextField(new Rect((5.25f*ownWidth/10),(3*ownHeight/10),30,20),endDay,2);
		endYear = GUI.TextField(new Rect((6*ownWidth/10),(3*ownHeight/10),60,20),endYear,4);
		

		
		//Create a button that will save the data entered
		if(GUI.Button(new Rect((ownWidth/8),ownHeight-25,100,20),"Enter")){
			error = false;
			//Save the data
			try {
				startTime = new DateTime(Convert.ToInt32(startYear), Convert.ToInt32(startMonth), Convert.ToInt32(startDay),
									Convert.ToInt32(startHour), Convert.ToInt32(startMin), Convert.ToInt32(startSec));
				endTime = new DateTime(Convert.ToInt32(endYear), Convert.ToInt32(endMonth), Convert.ToInt32(endDay),
									Convert.ToInt32(endHour), Convert.ToInt32(endMin), Convert.ToInt32(endSec));
				if(endTime.CompareTo(startTime) <= 0) throw new ArgumentOutOfRangeException();
			} catch(ArgumentOutOfRangeException e) {
				error = true;
				Debug.Log("Argument out of range, please enter a valid time");
			} catch(FormatException e){
				error = true;
				Debug.Log("Incorrect time format.  Use only integers.");	
			}
			if(!error){
				currTimeline.setStart(startTime);
				currTimeline.setEnd(endTime);
				Debug.Log("Start Time: " + startTime + "   End Time: " + endTime);
			}
		}
		
		//Print a warning if the user has entered an incorrect time
		if(error){
			Debug.Log("error = " + error);
			GUI.color = Color.red;
			GUI.Label(new Rect((2.5f*ownWidth/10),(5*ownHeight/10),200,20),"Please enter a valid time frame.");
			GUI.color = Color.white;
		}
		
		//Create a button that will closet the window
		if(GUI.Button(new Rect((6*ownWidth/8)-50,ownHeight-25,100,20),"Close")){
			showWindow = false;	
		}
	}
	
}
