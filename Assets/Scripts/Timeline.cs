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

	//Playhead position and texture
	public DateTime playhead = new DateTime(2008, 10, 3, 10, 12, 34);
	public Texture2D playheadTop;
	public Color playheadColor;
	
	// Toggle variable for output strings
	public bool debugStrings = false;
	
	// Variables for timeline's many ranges
	private DateTime totalTimeStart;
	private DateTime totalTimeEnd;
	private DateTime visibleTimeStart;
	private DateTime visibleTimeEnd;
	private TimeSpan visibleRange;

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
	private DateTime panStartTime;
	private DateTime panEndTime;
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

	private struct TickDescription {
		public DateTime time;
		public string description;
		public float yOffset;
	}
	
	private List<TickDescription> ticks = new List<TickDescription>();
	
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

	//Only add ticks that are within a certain distance away from already extant ones
	void SafeAddTicks(TickDescription t, TimeSpan distance) {
		bool safe = true;
		for(int i = 0; i < ticks.Count && safe; i++) {
			safe = (ticks[i].time - t.time).Duration() > distance;
		}

		if(safe)
			ticks.Add(t);
	}

	void AddTickLevel(float dayStartRange, float dayEndRange, float finalY, TimeSpan delta, DateTime start, Func<DateTime, DateTime> increment, string descriptor) {
		//Make sure this level should be visible
		if(visibleRange.TotalDays < dayStartRange) {

			//Figure out where the height of the text should be
			float ratio = (dayStartRange - (float)visibleRange.TotalDays) / (dayStartRange - dayEndRange);
			ratio = ratio > 1.0f ? 1.0f : ratio;
			float yOffset = timelineY + finalY + (1-ratio) * (timelineHeight - finalY + 10.0f);

			//And loop through the available spots
			DateTime current = start;
			while(current < visibleTimeEnd) {
				DateTime added = increment(current);
				TickDescription t = new TickDescription();
				t.time = added;
				t.description = added.ToString(descriptor);
				t.yOffset = yOffset;
				SafeAddTicks(t, delta);
				current = added;
			}
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
		ticks.Clear();

		//Which markers should we be looking for?
		//years
		if(visibleTimeStart.Year < visibleTimeEnd.Year) {
			//Add in year changes
			for(int i = visibleTimeStart.Year + 1; i <= visibleTimeEnd.Year; i++) {
				TickDescription t = new TickDescription();
				t.time = new DateTime(i, 1, 1);
				t.description = i + "";
				t.yOffset = timelineY + 40.0f;
				ticks.Add(t);
			}
		}

		//months
		AddTickLevel(1200, 600, 50.0f, 
				new TimeSpan(10,0,0,0),
				new DateTime(visibleTimeStart.Year,
					visibleTimeStart.Month,
					1),
				delegate(DateTime t) { return t.AddMonths(1); },
				"MMMM");

		//days
		AddTickLevel(120, 60, 60.0f,
				new TimeSpan(0,10,0,0),
				new DateTime(visibleTimeStart.Year,
					visibleTimeStart.Month,
					visibleTimeStart.Day),
				delegate(DateTime t) { return t.AddDays(1); },
				"dd");

		//hours
		AddTickLevel(4.0f, 1.0f, 70.0f,
				new TimeSpan(0, 0, 20, 0),
				new DateTime(visibleTimeStart.Year,
					visibleTimeStart.Month,
					visibleTimeStart.Day,
					visibleTimeStart.Hour,
					0, 0),
				delegate(DateTime t) { return t.AddHours(1); },
				"HH:mm:ss");

		//minutes
		AddTickLevel(0.1f, 0.04f, 80.0f,
				new TimeSpan(0, 0, 0, 20),
				new DateTime(visibleTimeStart.Year,
					visibleTimeStart.Month,
					visibleTimeStart.Day,
					visibleTimeStart.Hour,
					visibleTimeStart.Minute, 0),
				delegate(DateTime t) { return t.AddMinutes(1); },
				":mm:ss");

		//Seconds
		AddTickLevel(0.0013f, 0.00069f, 90.0f,
				new TimeSpan(0, 0, 0, 0, 20),
				new DateTime(visibleTimeStart.Year,
					visibleTimeStart.Month,
					visibleTimeStart.Day,
					visibleTimeStart.Hour,
					visibleTimeStart.Minute,
					visibleTimeStart.Second),
				delegate(DateTime t) { return t.AddSeconds(1); },
				":ss");

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
					visibleTimeStart = visibleTimeStart.AddDays ((visibleRange.TotalDays/10.0f));
					visibleTimeEnd = visibleTimeEnd.AddDays (-(visibleRange.TotalDays/10.0f));
				}
			}
			else{
				visibleTimeStart = visibleTimeStart.AddDays (-(visibleRange.TotalDays/10.0f));
				visibleTimeEnd = visibleTimeEnd.AddDays ((visibleRange.TotalDays/10.0f));
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
			panStartTime = visibleTimeStart;
			panEndTime = visibleTimeEnd;
		}
		if(mousePanStart >= 0){
			mousePanDelta = Input.mousePosition.x - mousePanStart;
			// Resetting mousePanStart on each pass so that
			//    changes are permenant at this level
			//mousePanStart = Input.mousePosition.x;
			double mouseToTimeline = visibleRange.TotalDays/(double)Screen.width;
			// panAmount can be positive for negative, works with DateTime.Add()
			float panAmount = (float)(mouseToTimeline*mousePanDelta);
			if(panEndTime.AddDays(-panAmount) > totalTimeEnd){
				float smallPan   = (float)((totalTimeEnd - panEndTime).TotalDays);
				visibleTimeEnd   = totalTimeEnd;
				visibleTimeStart = panStartTime.AddDays(smallPan);
			}else if(panStartTime.AddDays(-panAmount) < totalTimeStart){
				float smallPan   = (float)((totalTimeStart - panStartTime).TotalDays);
				visibleTimeStart = totalTimeStart;
				visibleTimeEnd   = panEndTime.AddDays(smallPan);
			}else{
				visibleTimeEnd   = panEndTime.AddDays(-panAmount);
				visibleTimeStart = panStartTime.AddDays(-panAmount);
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
		float countToTimelineY = (timelineHeight-20.0f) / (Screen.width / (tickWidth*spacePerTick));
		if(debugStrings)
			Debug.Log("countToTimelineY: " + countToTimelineY);
		
		//Draw the various ticks
		foreach(TickDescription t in ticks) {
			float myX = (float)(((double)(t.time.Ticks - visibleTimeStart.Ticks) / visibleRange.Ticks) * Screen.width);
			GUI.DrawTexture(new Rect(myX, t.yOffset, tickWidth, timelineHeight), tickMark);
			GUI.Label(new Rect(myX - 32, t.yOffset - 20, 64, 20), t.description);
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
																	Convert.ToInt32(potentialStartDay),
																	visibleTimeStart.Hour,
																	visibleTimeStart.Minute,
																	visibleTimeStart.Second,
																	visibleTimeStart.Millisecond);
					potentialTimeEnd = new DateTime(Convert.ToInt32(potentialEndYear), 
																	Convert.ToInt32(potentialEndMonth), 
																	Convert.ToInt32(potentialEndDay),
																	visibleTimeEnd.Hour,
																	visibleTimeEnd.Minute,
																	visibleTimeEnd.Second,
																	visibleTimeEnd.Millisecond);

					//Make sure the new datetime makes sense
					if(potentialTimeEnd.CompareTo(potentialTimeStart) > 0 &&
						potentialTimeStart.CompareTo(totalTimeStart) >= 0 &&
						potentialTimeEnd.CompareTo(totalTimeEnd) <= 0) {

						visibleTimeStart = potentialTimeStart;
						visibleTimeEnd = potentialTimeEnd;
						FindVisibleTickmarks();
					}
				} catch(Exception) {
				}

			GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	//Draws the playhead, if visible
	private void DrawPlayhead() {
		if(playhead > visibleTimeStart &&
				playhead < visibleTimeEnd) {
			//Get the x position
			float myX = (float)(((double)(playhead.Ticks - visibleTimeStart.Ticks) / visibleRange.Ticks) * Screen.width);
			GUI.color = playheadColor;
			GUI.DrawTexture(new Rect(myX - 5, timelineY + 30.0f, 10.0f, 10.0f), playheadTop);
			GUI.DrawTexture(new Rect(myX - 1, timelineY + 40.0f, 2.0f, timelineHeight), tickMark);
			GUI.color = Color.white;
		}
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

		DrawPlayhead();
		
		// Label is the area in which to display the hover tooltip
		//    for any GUIContent that indicates a tooltip
		GUI.Label(new Rect((Screen.width/2f)-75,
			timelineY,100, 20),GUI.tooltip);
	}
}
