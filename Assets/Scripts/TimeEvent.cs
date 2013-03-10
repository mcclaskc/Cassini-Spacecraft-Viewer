using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class: TimeEvent
/// Author: Don England
/// Last Modified: 9-Mar-2013
/// Purpose:  This script is meant to handle events
/// Usage:  Script is not attached to any object, but is used by
///    Timeline to store events in a list for visualization
/// </summary>
public class TimeEvent {
	private DateTime mStart;
	private DateTime mEnd;
	private String mLabel;
	
	private GUIStyle mGUIStyle;
	private Texture2D mTexture;
	private Color mColor;
	
	public TimeEvent(){
	}
	
	public TimeEvent(DateTime start, DateTime end, String label, Color color){
		mStart = start;
		mEnd   = end;
		mLabel = label;
		setColor (color);
	}
	
	public DateTime getStart(){ return mStart;}
	public void setStart(DateTime date){ mStart = date;}
	
	public DateTime getEnd(){ return mEnd;}
	public void setEnd(DateTime date){ mEnd = date;}
	
	public String getLabel(){ return mLabel;}
	public void setLabel(String label){ mLabel = label;}
	
	public Color getColor(){ return mColor;}
	public void setColor(Color color){
		mGUIStyle = new GUIStyle();
		mTexture  = new Texture2D(1,1);
		mColor = color;
		mTexture.SetPixel(0,0,color);
		mTexture.Apply();
		mGUIStyle.normal.background = mTexture;
		mGUIStyle.clipping = TextClipping.Clip;
		mGUIStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	public GUIStyle getStyle(){ return mGUIStyle;}
	public void setStyle(GUIStyle style){ mGUIStyle = style;}
}
