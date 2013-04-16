using UnityEngine;
using System.Collections;

public class labeler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameObject planetLabel = (GameObject)Instantiate(Resources.Load("Planet Label"));
		planetLabel.transform.parent = gameObject.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
