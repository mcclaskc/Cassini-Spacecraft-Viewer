using UnityEngine;
using System.Collections;

/*
 * This script creates the Reset, Rewind, and Play buttons and handles
 * the cases where they are clicked on by the user.  The current
 * version places them off center to accomodate the placement of the 
 * cursor text.
 * 
 * Script should be attached to an object in the scene.
 * Currently Attached to: windowManager
 */

public class PlayPauseButton : MonoBehaviour {
	
	//timeManager link MUST have "Reset()", "Play()", and "Reverse()" functions or signals will not work
	public GameObject timeManager;		//This is the scene's time manager, must be linked
	public Texture2D playTexture; 		//Texture for play button
	public Texture2D pauseTexture;		//Texture for pause button
	public Texture2D resetTexture;		//Texture for reset button
	public Texture2D reverseTexture;	//Texture for reverse button
	public float height;				//Height of the buttons
	public float width;					//Width of the buttons
	public float yPosition;				//Placement of the buttons in pixels from bottom of the screen
	
	private bool isPlaying;				//True when play is active
	private bool isReversing;			//True when reverse is active
	private Rect resetBox;				//Rect controlling the size of the reset button
	private Rect reverseBox;			//Rect controlling the size of the reverse button			
	private Rect playBox;				//Rect controlling the size of the play button


	void Start () {
		//Make sure nothing is playing at scene start
		isPlaying = false;
		isReversing = false;
		//Set up the boxes.  Change the various values to alter where the buttons are placed
		//Any changes should be done through public variables if possible, or new ones created if need be
		resetBox = new Rect((Screen.width/2f)+(width*.5f),Screen.height - yPosition,width,height);
		reverseBox = new Rect((Screen.width/2f)+(width * 1.6f),Screen.height - yPosition,width,height);
		playBox = new Rect((Screen.width/2f)+(width * 2.5f)+5,Screen.height - yPosition,width,height);
	}
	

	void OnGUI(){
		//Ensure all of the butttons are drawn on top of the scene
		GUI.depth = -1;
		//----Reset portion----//
		if(GUI.Button(resetBox, resetTexture)){
			timeManager.SendMessage("Reset");
			isReversing = isPlaying = false;
		}
		
		//----Reverse portion----//
		if(!isReversing){
			if(GUI.Button(reverseBox, reverseTexture)){
				timeManager.SendMessage("Reverse");
				isReversing = !isReversing;
				isPlaying = false;
			}
		}else{
			if(GUI.Button(reverseBox, pauseTexture)){
				timeManager.SendMessage("Reverse");
				isReversing = !isReversing;
				isPlaying = false;
			}
		}
		
	//----Play/Pause portion-----//
		if(!isPlaying){
			if(GUI.Button(playBox, playTexture)){
				timeManager.SendMessage("Play");
				isPlaying = !isPlaying;
				isReversing = false;
			}
		}else{
			if(GUI.Button(playBox, pauseTexture)){
				timeManager.SendMessage("Play");
				isPlaying = !isPlaying;
				isReversing = false;
			}
		}
	}
	
}
