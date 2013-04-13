using UnityEngine;
using System.Collections;

public class PlayPauseButton : MonoBehaviour {
	
	public Texture2D playTexture;
	public Texture2D pauseTexture;
	public Texture2D resetTexture;
	public Texture2D reverseTexture;
	public float height;
	public float width;
	public float xPosition;
	public float yPosition;
	
	private bool isPlaying;
	private Rect resetBox;
	private Rect reverseBox;
	private Rect playBox;
	private GameObject[] mobileBodies;

	// Use this for initialization
	void Start () {
		isPlaying = false;
		
		resetBox = new Rect(xPosition*Screen.width,yPosition*Screen.height,width,height);
		reverseBox = new Rect((xPosition*Screen.width)+width+2.5f,yPosition*Screen.height,width,height);
		playBox = new Rect((xPosition*Screen.width)+(2*width)+5,yPosition*Screen.height,width,height);
		mobileBodies = GameObject.FindGameObjectsWithTag("Mobile");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(GUI.Button(resetBox, resetTexture)){
			foreach (GameObject mover in mobileBodies){
					mover.SendMessage ("Reset");
				}
		}
		if(GUI.Button(reverseBox, reverseTexture)){
			foreach (GameObject mover in mobileBodies){
				mover.SendMessage ("Reverse");
			}
		}
		
	//----Play/Pause portion-----//
		if(!isPlaying){
			if(GUI.Button(playBox, playTexture)){
				isPlaying = !isPlaying;
				foreach (GameObject mover in mobileBodies){
					mover.SendMessage ("Play");
				}
			}
		}else{
			if(GUI.Button(playBox, pauseTexture)){
				isPlaying = !isPlaying;
				foreach (GameObject mover in mobileBodies){
					mover.SendMessage ("Play");
				}
			}
		}
	}
	
}
