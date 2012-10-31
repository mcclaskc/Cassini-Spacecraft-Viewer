using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
	
	public Transform target;
	public int heightOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = target.position;
		//transform.position.y = transform.position.y + heightOffset;
	}
}
