using UnityEngine;
using System.Collections;

public class PlayPauseButton : MonoBehaviour {
	
	public GameObject timeManager;
	public Texture2D playTexture;
	public Texture2D pauseTexture;
	public Texture2D resetTexture;
	public Texture2D reverseTexture;
	public float height;
	public float width;
	public float yPosition;
	
	private bool isPlaying;
	private bool isReversing;
	private Rect resetBox;
	private Rect reverseBox;
	private Rect playBox;


	// Use this for initialization
	void Start () {
		isPlaying = false;
		isReversing = false;
		resetBox = new Rect((Screen.width/2f)+(width*.5f),Screen.height - yPosition,width,height);
		reverseBox = new Rect((Screen.width/2f)+(width * 1.6f),Screen.height - yPosition,width,height);
		playBox = new Rect((Screen.width/2f)+(width * 2.5f)+5,Screen.height - yPosition,width,height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		//Ensure all of the butttons are drawn on top of the scene
		GUI.depth = -1;
		//----Reset portion----//
		if(GUI.Button(resetBox, resetTexture)){
			timeManager.SendMessage("Reset");
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
			}
		}
	}
	
}
