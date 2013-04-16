using UnityEngine;
using System.Collections;
/// <summary>
/// Class: Gimbal
/// Author: Jacob Rieger
/// Last Modified: 15-April-2013
/// Purpose:  This script handles the camera controls for the view of Gimbaling an object
/// Usage:  Attach this script to your the camera you wish to gimbal with (normally main)
/// and then set the target of what you want your center of gimbal to be in the inspector.
/// </summary>
    public class Gimbal : MonoBehaviour 
    {
        public Transform target; //Center of Focus
	
        public float distance = 10.0f;  //Default distance away from target
		public float distanceMin; 		//ZoomIn limit
		public float distanceMax; 		//ZoomOut Limit

        public float xSpeed = 250.0f; 	//Panning speeds
        public float ySpeed = 120.0f;

        public float yMinLimit = -20;   //Limits at how far an angle you can go over or under the object
        public float yMaxLimit = 80;

        private float x = 0.0f;
        private float y = 0.0f;
	
		private float timelineHeight;
		public GameObject timeline;
	
		private bool MouseDownPanning = false; //Variable if we are currently moving the camera
		private bool notOverTimebar   = false;
		
        void Start () 
        {
			//Set our initial angles
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
			
			//Get the timeline height
			timelineHeight = timeline.GetComponent<Timeline>().timelineHeight;

        }

        void Update () 
        {
			notOverTimebar = false;
		
			//Checking to see if mouse is over the timeline
			if(Input.GetMouseButtonDown(0) && Input.mousePosition.y > timelineHeight)
			{
				MouseDownPanning = true;
			}
		
			if(Input.mousePosition.y > timelineHeight)
			{
				notOverTimebar = true;
			}

			if(Input.GetMouseButtonUp(0))
			{
				MouseDownPanning = false;
			}
		
			//Only move camera if you're not over the timeline, or are already moving the camera
			if(MouseDownPanning)
			{
				if (target && Input.GetMouseButton(0))
	            {
					//This is called whenever the user is holding down the mouse button
					//Sets the x and y axis to the new rotation desired.
	             	setAxis();
					y = ClampAngle(y, yMinLimit, yMaxLimit);
	            }
			}
			if(notOverTimebar)
			{
				if(target)
				{
					//This is called when the user is scrolling, or not doing anything
					distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel"), distanceMin, distanceMax);
					setTransforms ();
				}
			}
        }
	
	
		void setTransforms()
		{
			//Sets the desired position and rotations
			transform.rotation = Quaternion.Euler(y, x, 0);
			transform.position = Quaternion.Euler(y, x, 0) * new Vector3(0.0f, 0.0f, -distance) + target.position;	
		}
	
		void setAxis()
		{
			//Sets the desired angles
			x += Input.GetAxis("Mouse X") * xSpeed *Time.deltaTime;
			y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
		}

		static float ClampAngle(float angle, float min, float max) 
		{
			//Keeps our angles in workable ranges
			if (angle < -360)
			{
					angle += 360;
			}
			if (angle > 360)
			{
					angle -= 360;
			}
			return Mathf.Clamp(angle, min, max);
		}
    }
