using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class: Timeline
/// Author: Don England
/// Last Modified: 9-Mar-2013
/// Purpose:  This script is meant to handle all display and navigation functions
/// 	for indicating the timeline of available data for Cassini
/// Usage:  Attach this script to an object within a Unity Scene, such as Timeline,
/// 	Use the mouse to navigate the timeline
/// 	Left mouse sets current selection
/// 	Right mouse pans view
/// 	Mouse wheel zooms view
///     Add TimeEvents with addEvent()
///     set Timeline times with setStart(), setEnd(), setTimeline()
/// </summary>
public class Timeline : MonoBehaviour {

	public GUISkin skin;
	
	// Toggle variable for output strings
	public bool debugStrings = false;
	
	// Variables for timeline's many ranges
	private DateTime totalTimeStart;
	private DateTime totalTimeEnd;
	private DateTime visibleTimeStart;
	private DateTime visibleTimeEnd;
	private TimeSpan visibleRange;
	private TimeSpan totalRange;

	//Variables for the timeline selection
	private DateTime potentialTimeStart;
	private DateTime potentialTimeEnd;
	private string potentialStartMonth;
	private string potentialStartDay;
	private string potentialStartYear;
	private string potentialEndMonth;
	private string potentialEndDay;
	private string potentialEndYear;
	
	// Variables for feeding object play functions
	// timelineStep represents the granularity of updates
	//    i.e., a timelineStep of 1 second means updates are per second
	//    while a timelineStep of 1 hour means updates are per hour
	public TimeSpan timelineStep;
	
	// Allow the user to call an update to the current time range
	public bool refreshCurrentTime = false;
	
	// Public access to timelineHeight Controls
	public float timelineHeight = 200;
	
	// Private value settings for timeline display
	private float timelineY = 0;
	
	//Panning Logic
	private float mousePanStart = -1;
	private float mousePanDelta = 0;
	
	// Tick Mark Variables
	// Image for the tick marks on the timeline
	public Texture2D tickMark;
	public int numberOfTickMarks = 4;
	public int spacePerTick = 5;
	public int tickWidth    = 5;
	
	
	// Tick Mark Logic
	private enum tickType {years,months,days,hours,minutes,seconds};
	private tickType myTickType;
	private double tickMarkInterval;
	private TimeSpan tickSpan;
	
	
	// Tick Mark Take 2
	private List<double> visYears   = new List<double>();
	private List<double> visMonths  = new List<double>(); 
	private List<double> visDays    = new List<double>(); 
	private List<double> visHours   = new List<double>(); 
	private List<double> visMinutes = new List<double>(); 
	private List<double> visSeconds = new List<double>();
	
	// Event Variables
	private List<TimeEvent> events = new List<TimeEvent>();
	
	// Selection Variables
	private TimeEvent selection = new TimeEvent();
	private DateTime initialSelection;
	private bool userSelect = false;
	
	// Add event
	public void addEvent(TimeEvent newEvent){
		events.Add (newEvent);
	}
	public void clearEvents(){
		events.Clear();
	}
	
	/// <summary>
	/// Function: setStart()
	/// Author: Don England
	/// Last Modified: 9-Mar-2013
	/// Purpose:  This function is meant to set the initial timelines start date
	/// Usage:  call when user requests new start date for timeline
	public void setStart(DateTime start){
		if(start < totalTimeEnd){
			totalTimeStart = start;
			totalRange     = totalTimeEnd-totalTimeStart;
			visibleTimeStart = totalTimeStart;
			visibleTimeEnd   = totalTimeEnd;
			visibleRange     = visibleTimeEnd - visibleTimeStart;
		}
		else{
			if(debugStrings)
				Debug.Log ("Error: setStart() given start time that occurs after current end time");
		}
	}
	/// <summary>
	/// Function: setStart()
	/// Author: Don England
	/// Last Modified: 9-Mar-2013
	/// Purpose:  This function is meant to set the initial timeline's end date
	/// Usage:  call when user requests new end date for timeline
	public void setEnd(DateTime end){
		if(end > totalTimeStart){
			totalTimeEnd = end;
			totalRange   = totalTimeEnd-totalTimeStart;
			visibleTimeStart = totalTimeStart;
			visibleTimeEnd   = totalTimeEnd;
			visibleRange     = visibleTimeEnd - visibleTimeStart;
		}
		else{
			if(debugStrings)
				Debug.Log ("Error: setEnd() given end time that occurs before current start time");
		}
	}
	/// <summary>
	/// Function: setTimeline()
	/// Author: Don England
	/// Last Modified: 9-Mar-2013
	/// Purpose:  This function is meant to set the initial timeline's start and end dates
	/// Usage:  call when user requests new start and end dates for timeline
	public void setTimeline(DateTime start, DateTime end){
		if(start < end){
			totalTimeStart = start;
			totalTimeEnd   = end;
			totalRange     = totalTimeEnd-totalTimeStart;
			visibleTimeStart = totalTimeStart;
			visibleTimeEnd   = totalTimeEnd;
			visibleRange     = visibleTimeEnd - visibleTimeStart;
		}
		else{
			if(debugStrings)
				Debug.Log ("Error: setTimeline() given start time that occurs after end time");
		}
	}
	
	/// <summary>
	/// Function: InitTimeline()
	/// Author: Don England
	/// Last Modified: 18-Feb-2013
	/// Purpose:  This function is meant to initialize the date ranges for
	///    displaying events and tick marks used for navigating Cassini Data
	/// Usage:  Call when script is initialized in Awake()
	/// </summary>
	void InitTimeline(){
		// Cassini Launch Date 1997 Oct 15th 084300 UTC
		// Cassini Orbit Insertion Date 2004 July 1st 024800 UTC
		totalTimeStart = new DateTime(2004,7,1,2,48,0, DateTimeKind.Utc);
		totalTimeEnd = DateTime.UtcNow;
		totalRange   = totalTimeEnd - totalTimeStart;
		
		// Visible time range describes what is visible on the timeline
		visibleTimeStart = totalTimeStart;
		visibleTimeEnd   = totalTimeEnd;
		visibleRange     = visibleTimeEnd - visibleTimeStart;

		//Initiate the selection boxes
		potentialTimeStart = totalTimeStart;
		potentialTimeEnd = totalTimeEnd;
		potentialStartMonth = totalTimeStart.Month.ToString();
		potentialStartDay = totalTimeStart.Day.ToString();
		potentialStartYear = totalTimeStart.Year.ToString();
		potentialEndMonth = totalTimeEnd.Month.ToString();
		potentialEndDay = totalTimeEnd.Day.ToString();
		potentialEndYear = totalTimeEnd.Year.ToString();
		

		// User Selection, uses  TimeEvent
		selection.setStart(new DateTime(2008,6,1,0,1,0, DateTimeKind.Utc));
		selection.setEnd(new DateTime(2008,7,1,0,1,0, DateTimeKind.Utc));
		selection.setLabel("Selection");
		selection.setColor(new Color(1.0f,1.0f,0.6f,0.5f));
		addEvent(selection);
		
		// Temp Event 
		addEvent (new TimeEvent(new DateTime(2009,1,1,0,0,0, DateTimeKind.Utc),
			new DateTime(2010,1,1,0,0,0, DateTimeKind.Utc), "Test Event",
			new Color(0.0f,1.0f,0.0f,0.5f)));
	}
	
	/// <summary>
	/// Function: FindTickMarks()
	/// Author: Don England
	/// Last Modified: 19-Feb-2013
	/// Purpose:  This function is meant to find the tickType used
	///    for generating tickMarkInterval which combine to produce 
	///    a tickOffset for computing visible tick marks
	/// Usage:  Call when ever the visible range has been modified
	/// </summary>
	void FindTickMarks(){
		// Find the Current Visible Range
		visibleRange = visibleTimeEnd - visibleTimeStart;
		double typeRange = visibleRange.TotalDays;
		if( typeRange > (365.25*numberOfTickMarks)){
			//use years
			myTickType = tickType.years;
			for( int k = 0; k < (totalRange.Days/365.25); k++){
				if(typeRange/(365.25*k) <= numberOfTickMarks)
					totalTimeStart.AddYears(1);
			}
		}else if( typeRange > (30.5*numberOfTickMarks)){
			//use months
			myTickType = tickType.months;
		}else if( typeRange > numberOfTickMarks){
			//use days
			myTickType = tickType.days;
		}else{
			typeRange = visibleRange.TotalMinutes;
			if( typeRange > (60 * numberOfTickMarks)){
				//use hours
				myTickType = tickType.hours;
			}else if( typeRange > numberOfTickMarks){
				//use minutes
				myTickType = tickType.minutes;
			}
			else{
				//use seconds
				myTickType = tickType.seconds;
			}
		}
		
		FindTickMarkInterval();			
	}
	
	/// <summary>
	/// Function: FindTickMarkInterval()
	/// Author: Don England
	/// Last Modified: 19-Feb-2013
	/// Purpose:  This function is meant to find the tick interval
	///    for use in computing the visible ticks. Separated from
	///    FindTickMarks for readability/responsibility of function
	/// Usage:  Call when ever the tick mark type has changed
	/// </summary>
	void FindTickMarkInterval(){
		if( myTickType == tickType.years ||
			myTickType == tickType.months ||
			myTickType == tickType.days){
				tickMarkInterval = visibleRange.TotalDays/numberOfTickMarks;
		}else if(myTickType == tickType.hours ||
				myTickType == tickType.minutes){
				tickMarkInterval = visibleRange.TotalMinutes/numberOfTickMarks;
		}else{
			tickMarkInterval = visibleRange.TotalSeconds/numberOfTickMarks;
		}
	}
	
	/// <summary>
	/// Function: FindVisibleTickMarks()
	/// Author: Don England
	/// Last Modified: 5-Mar-2013
	/// Purpose:  This function is meant to find the visible tick marks
	///    and add them to the appropriate lists for display in OnGUI()
	/// Usage:  Call when ever the visible range has been modified
	/// </summary>
	void FindVisibleTickmarks(){
		//Also update the selection dates
		potentialTimeStart = visibleTimeStart;
		potentialTimeEnd = visibleTimeEnd;
		potentialStartDay = visibleTimeStart.Day + "";
		potentialStartMonth = visibleTimeStart.Month + "";
		potentialStartYear = visibleTimeStart.Year + "";

		potentialEndDay = visibleTimeEnd.Day + "";
		potentialEndMonth = visibleTimeEnd.Month + "";
		potentialEndYear = visibleTimeEnd.Year + "";

		visibleRange = visibleTimeEnd - visibleTimeStart;
		
		visYears.Clear ();
		visMonths.Clear ();
		visDays.Clear ();
		visHours.Clear ();
		visMinutes.Clear ();
		visSeconds.Clear ();
		
		// Nearest (usually) non-visible tick marks
		DateTime visYear   = new DateTime(visibleTimeStart.Year, 1,1,0,0,0,DateTimeKind.Utc);
		DateTime visMonth  = new DateTime(visibleTimeStart.Year,
			visibleTimeStart.Month,1,0,0,0,DateTimeKind.Utc);
		DateTime visDay    = new DateTime(visibleTimeStart.Year,
			visibleTimeStart.Month,visibleTimeStart.Day,0,0,0,DateTimeKind.Utc);
		DateTime visHour   = new DateTime(visibleTimeStart.Year,
			visibleTimeStart.Month,visibleTimeStart.Day,visibleTimeStart.Hour,0,0,DateTimeKind.Utc);
		DateTime visMinute = new DateTime(visibleTimeStart.Year,
			visibleTimeStart.Month,visibleTimeStart.Day,visibleTimeStart.Hour,
			visibleTimeStart.Minute,0,0,DateTimeKind.Utc);
		DateTime visSecond = new DateTime(visibleTimeStart.Year,
			visibleTimeStart.Month,visibleTimeStart.Day,visibleTimeStart.Hour,
			visibleTimeStart.Minute,visibleTimeStart.Second,0,DateTimeKind.Utc);
		
		// Cycle through all possible visible year multiples
		double kMax = (visibleRange.TotalDays/365.25) + 1.0;
		if(kMax < Screen.width/25){
			for(int k = 1; k <= kMax; k++){
				visYears.Add ((visYear.AddYears(k) - visibleTimeStart).TotalDays);
			}
		}else{
			Debug.Log("Out of range, please decrease visible range on timeline");
		}
		
		// Verify tick spacing sanity
		// Should be a property drawer item for inspector
		if(tickWidth < 0)
			tickWidth = 1;
		if(tickWidth > 20)
			tickWidth = 20;
		if(spacePerTick < 0)
			spacePerTick = 1;
		if(spacePerTick > 20)
			spacePerTick = 20;
		// Set tick spacing
		int tickSpacing = tickWidth*spacePerTick;
		// Cycle through all possible visible month multiples
		kMax = (visibleRange.TotalDays/30.5) + 1.0;
		if(kMax < Screen.width/tickSpacing){
			visMonths.Clear();
			for(int k = 1; k <= kMax; k++){
				visMonths.Add ((visMonth.AddMonths(k) - visibleTimeStart).TotalDays);
			}
		}
		
		// Cycle through all possible available day ticks
		kMax = visibleRange.TotalDays + 1.0;
		if(kMax < Screen.width/tickSpacing){
			visDays.Clear();
			for(int k = 1; k <= kMax; k++){
				visDays.Add ((visDay.AddDays(k) - visibleTimeStart).TotalDays);
			}
		}
		// Cycle through all possible available hour ticks
		kMax = visibleRange.TotalHours + 1.0;
		if(kMax < Screen.width/tickSpacing){
			for(int k = 1; k <= kMax; k++){
				visHours.Add ((visHour.AddHours(k) - visibleTimeStart).TotalHours);
			}
		}
		// Cycle through all possible available minute ticks
		kMax = visibleRange.TotalMinutes + 1.0;
		if(kMax < Screen.width/tickSpacing){
			for(int k = 1; k <= kMax; k++){
				visMinutes.Add ((visMinute.AddMinutes(k) - visibleTimeStart).TotalMinutes);
			}
		}
		// Cycle through all possible available second ticks
		kMax = visibleRange.TotalSeconds + 1.0;
		if(kMax < Screen.width/tickSpacing){
			for(int k = 1; k <= kMax; k++){
				visSeconds.Add ((visSecond.AddSeconds(k) - visibleTimeStart).TotalSeconds);
			}
		}
	}
	
	void Awake(){
		InitTimeline();
		if(debugStrings)
			Debug.Log(totalTimeStart);
	}
	
	// Use this for initialization
	void Start () {
		timelineY = Screen.height - timelineHeight; 
	}
	
	/// <summary>
	/// Function: Update()
	/// Author: Don England
	/// Last Modified: 9-Mar-2013
	/// Purpose:  This function is called every frame (system dependant)
	///    and processes user input for scrolling and zooming functions
	/// Usage:  Call every frame for processing user input
	/// </summary>
	void Update () {
		if(refreshCurrentTime){
			totalTimeEnd = DateTime.UtcNow;
			refreshCurrentTime = false;
			if(debugStrings)
				Debug.Log(totalTimeEnd);
			
			FindVisibleTickmarks();
		}
		
		// Zoom visible timeline
		if(Input.GetAxis ("Mouse ScrollWheel") != 0 && Input.mousePosition.y < (Screen.height-timelineY)){
			var delta = Input.GetAxis("Mouse ScrollWheel");
			if(debugStrings){
				Debug.Log ("delta: " + delta);
				Debug.Log("mouseY: " + Input.mousePosition.y + ", timelineY: " + timelineY);
			}
			if(delta > 0){
				if(visibleRange.TotalSeconds > 30){
					visibleTimeStart = visibleTimeStart.AddDays ((visibleRange.TotalDays/10));
					visibleTimeEnd = visibleTimeEnd.AddDays (-(visibleRange.TotalDays/10));
				}
			}
			else{
				visibleTimeStart = visibleTimeStart.AddDays (-(visibleRange.TotalDays/10));
				visibleTimeEnd = visibleTimeEnd.AddDays ((visibleRange.TotalDays/10));
				if(visibleTimeStart < totalTimeStart)
					visibleTimeStart = totalTimeStart;
				if(visibleTimeEnd > totalTimeEnd)
					visibleTimeEnd = totalTimeEnd;
			}
			FindVisibleTickmarks();
		}
		
		// Adjust selection
		if(Input.GetMouseButtonDown(0) && Input.mousePosition.y < (Screen.height-timelineY)){			
			double mouseTime = visibleRange.TotalDays/Screen.width;
			mouseTime = mouseTime*Input.mousePosition.x;
			initialSelection = visibleTimeStart.AddDays(mouseTime);
			selection.setStart(initialSelection);
			selection.setEnd (initialSelection);
			userSelect = true;
			if(debugStrings)
				Debug.Log ("Select @ " + selection.getStart());
		}
		if(Input.GetMouseButtonUp(0)){	
			userSelect = false;		
			if(debugStrings)
				Debug.Log("End Selection @ " + selection.getEnd());
		}
		if(userSelect){
			double mouseTime = visibleRange.TotalDays/Screen.width;
			mouseTime = mouseTime*Input.mousePosition.x;
			DateTime endSelection = visibleTimeStart.AddDays (mouseTime);
			if(endSelection > initialSelection){
				selection.setEnd(endSelection);
				selection.setStart(initialSelection);
			}
			else{
				selection.setEnd(initialSelection);
				selection.setStart(endSelection);
			}
		}
		
		// Adjust Panning
		if(Input.GetMouseButtonDown(1) && Input.mousePosition.y < (Screen.height-timelineY)){
			if(debugStrings)
				Debug.Log ("Click @ " + Input.mousePosition.x);
			mousePanStart = Input.mousePosition.x;
		}
		if(mousePanStart >= 0){
			mousePanDelta = Input.mousePosition.x - mousePanStart;
			// Resetting mousePanStart on each pass so that
			//    changes are permenant at this level
			mousePanStart = Input.mousePosition.x;
			double mouseToTimeline = visibleRange.TotalDays/Screen.width;
			// panAmount can be positive for negative, works with DateTime.Add()
			float panAmount = (float)(mouseToTimeline*mousePanDelta);
			if(visibleTimeEnd.AddDays(-panAmount) > totalTimeEnd){
				float smallPan   = (float)((totalTimeEnd - visibleTimeEnd).TotalDays);
				visibleTimeEnd   = totalTimeEnd.AddDays(smallPan);
				visibleTimeStart = visibleTimeStart.AddDays(smallPan);
			}else if(visibleTimeStart.AddDays(-panAmount) < totalTimeStart){
				float smallPan   = (float)((totalTimeStart - visibleTimeStart).TotalDays);
				visibleTimeStart = totalTimeStart.AddDays(smallPan);
				visibleTimeEnd   = visibleTimeEnd.AddDays(smallPan);
			}else{
				visibleTimeEnd   = visibleTimeEnd.AddDays(-panAmount);
				visibleTimeStart = visibleTimeStart.AddDays(-panAmount);
			}
			
			// Update Tickmarks
			FindVisibleTickmarks();
		}
		if(Input.GetMouseButtonUp(1)){			
			if(debugStrings)
				Debug.Log("Release @ " + Input.mousePosition.x);
			mousePanStart = -1;
		}
	}
	
	/// <summary>
	/// Function: DrawTickmarks()
	/// Author: Don England
	/// Last Modified: 5-Mar-2013
	/// Purpose:  This function draws each tick mark from its
	///    appropriate list.  Each item is placed according to 
	///    variables that are adjustable in the inspector
	///    --side panel within Unity Editor
	/// Usage:  Call every time the GUI is updated--in OnGUI()
	/// </summary>
	void DrawTickmarks(){
		// TODO: optimize for performance, may require combining each visX list into a single list
		float countToTimelineY = (timelineHeight-20.0f) / (Screen.width / (tickWidth*spacePerTick));
		if(debugStrings)
			Debug.Log("countToTimelineY: " + countToTimelineY);
		
		
		double visToScreenX = Screen.width/visibleRange.TotalDays;
		double tickOffset = tickWidth/2;
		float yOffset = timelineY+(visYears.Count*countToTimelineY)+20.0f;
		foreach(double i in visYears){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
		yOffset = timelineY+(visMonths.Count*countToTimelineY)+20.0f;
		foreach(double i in visMonths){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
		yOffset = timelineY+(visDays.Count*countToTimelineY)+20.0f;
		foreach(double i in visDays){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
		yOffset = timelineY+(visHours.Count*countToTimelineY)+20.0f;
		visToScreenX = Screen.width/visibleRange.TotalHours;
		foreach(double i in visHours){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
		yOffset = timelineY+(visMinutes.Count*countToTimelineY)+20.0f;
		visToScreenX = Screen.width/visibleRange.TotalMinutes;
		foreach(double i in visMinutes){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
		yOffset = timelineY+(visSeconds.Count*countToTimelineY)+20.0f;
		visToScreenX = Screen.width/visibleRange.TotalSeconds;
		foreach(double i in visSeconds){
			float myX = (float)((i*visToScreenX)-(tickOffset));
			GUI.DrawTexture (new Rect (myX,yOffset, tickWidth, timelineHeight), tickMark);
		}
	}
	
	/// <summary>
	/// Function: DrawEvents()
	/// Author: Don England
	/// Last Modified: 9-Mar-2013
	/// Purpose:  This function draws each event
	/// Usage:  Call every time the GUI is updated--in OnGUI()
	/// </summary>
	void DrawEvents(){
		// Can use refactoring to move discovery of visible events to
		//    a findVisibleEvents() to be called every time visible range
		//    is updated.
		foreach(TimeEvent e in events){
			// Alternate location finder
			//double eStart = (visibleTimeEnd - e.getStart()).TotalDays;
			//double eEnd   = (visibleTimeEnd - e.getEnd()).TotalDays;
			double eStart = (e.getStart() - visibleTimeStart).TotalDays;
			double eEnd   = (e.getEnd() - visibleTimeStart).TotalDays;
			double eventToVis = Screen.width/visibleRange.TotalDays;
			
			// Basic box collision detection
			if((e.getEnd() < visibleTimeStart) || (e.getStart() > visibleTimeEnd)){
				// cannot be visible
				if(debugStrings)
					Debug.Log ("Event \"" + e.getLabel() + "\" is not visible");
			}
			else{
				// it is visible in some way
				if((eEnd-eStart)*eventToVis < 1){
					GUI.Box (new Rect((float)(eStart*eventToVis), (timelineY+(Screen.height-timelineY)*0.5f),
					1.0f, (timelineHeight/2.0f)),
					new GUIContent(e.getLabel(), e.getLabel()), e.getStyle());
				}
				else{
					GUI.Box (new Rect((float)(eStart*eventToVis), (timelineY+(Screen.height-timelineY)*0.5f),
					(float)((eEnd-eStart)*eventToVis), (timelineHeight/2.0f)),
					new GUIContent(e.getLabel(), e.getLabel()), e.getStyle());
				}
			}
		}
	}
	
	/// <summary>
	/// Function: DrawControlBar()
	/// Author: Connor Janowiak
	/// Last Modified: 13-Apr-2013
	/// Purpose: This function draws the control bar with start/end
	///		date control as well as play/pause
	/// Usage: Called by OnGUI()
	/// </summary>
	private void DrawControlBar() {
		GUILayout.BeginArea(new Rect(0, timelineY, Screen.width, 30));
			GUILayout.BeginHorizontal();
				//Get the startdate
				potentialStartMonth 	= GUILayout.TextField(potentialStartMonth, 2);
				potentialStartDay 		= GUILayout.TextField(potentialStartDay, 2);
				potentialStartYear		= GUILayout.TextField(potentialStartYear, 4);

				GUILayout.FlexibleSpace();

				//Get the enddate 
				potentialEndMonth 	= GUILayout.TextField(potentialEndMonth, 2);
				potentialEndDay 		= GUILayout.TextField(potentialEndDay, 2);
				potentialEndYear		= GUILayout.TextField(potentialEndYear, 4);

				try {
					potentialTimeStart = new DateTime(Convert.ToInt32(potentialStartYear), 
																	Convert.ToInt32(potentialStartMonth), 
																	Convert.ToInt32(potentialStartDay));
					potentialTimeEnd = new DateTime(Convert.ToInt32(potentialEndYear), 
																	Convert.ToInt32(potentialEndMonth), 
																	Convert.ToInt32(potentialEndDay));

					//Make sure the new datetime makes sense
					if(potentialTimeEnd.CompareTo(potentialTimeStart) > 0 &&
						potentialTimeStart.CompareTo(totalTimeStart) >= 0 &&
						potentialTimeEnd.CompareTo(totalTimeEnd) <= 0) {

						visibleTimeStart = potentialTimeStart;
						visibleTimeEnd = potentialTimeEnd;
						FindVisibleTickmarks();
					}
				} catch(Exception e) {
				}

			GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	
	/// <summary>
	/// Function: OnGUI()
	/// Author: Don England
	/// Last Modified: 13-Apr-2013
	/// Purpose:  This function draws each tick marks mouse label
	///    and background GUI box.
	/// Usage:  Called by Unity every time the GUI is updated
	/// </summary>
	void OnGUI(){
		
		//Set the gui skin
		GUI.skin = skin;

		// Draw Mouse Position Label (mouseTime from timeline)
		double mouseTime;
		DateTime mouseDate = new DateTime();
		String mouseTimeLocation = "";
		mouseTime = visibleRange.TotalDays/Screen.width;
		mouseTime = mouseTime*Input.mousePosition.x;
		mouseDate = visibleTimeStart.AddDays(mouseTime);
		if(visibleRange.TotalDays > 15) {
			mouseTimeLocation = mouseDate.Month + "/" + mouseDate.Day + "/" + mouseDate.Year;
		}
		else {
			mouseTimeLocation = "" + mouseDate;
		}

		// Screen Variable Controls
		GUI.Box (new Rect(-5, timelineY-1, Screen.width+10, timelineHeight+5),
			// GUIContent indicates its area name with a
			//    message that will set the global GUI.tooltip
			new GUIContent("", mouseTimeLocation));

		//Draw the control bar
		DrawControlBar();

		DrawTickmarks();
			
		DrawEvents();
		
		// Label is the area in which to display the hover tooltip
		//    for any GUIContent that indicates a tooltip
		GUI.Label(new Rect((Screen.width - (totalTimeEnd.ToString().Length*5))/2,
			timelineY,Screen.width, 20),GUI.tooltip);
	}
}
