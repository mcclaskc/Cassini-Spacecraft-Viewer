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
	public float xPosition;
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
		resetBox = new Rect(xPosition*Screen.width,yPosition*Screen.height,width,height);
		reverseBox = new Rect((xPosition*Screen.width)+width+2.5f,yPosition*Screen.height,width,height);
		playBox = new Rect((xPosition*Screen.width)+(2*width)+5,yPosition*Screen.height,width,height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		//----Reset portion----//
		if(GUI.Button(resetBox, resetTexture)){
			timeManager.SendMessage("Reset");
		}
		
		//----Reverse portion----//
		if(!isReversing){
			if(GUI.Button(reverseBox, reverseTexture)){
				timeManager.SendMessage("Reverse");
				isReversing = !isReversing;
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
			}
		}else{
			if(GUI.Button(playBox, pauseTexture)){
				timeManager.SendMessage("Play");
				isPlaying = !isPlaying;
			}
		}
	}
	
}
