using UnityEngine;
using System.Collections;

    public class MouseOrbit : MonoBehaviour 
    {
        public Transform target; //What you're looking at
	
        public float distance = 10.0f; //Default distance away from target
		public float distanceMin; //how closely your allowed to zoom
		public float distanceMax; //how far away your allowed to zoom

        public float xSpeed = 250.0f; //Panning speeds
        public float ySpeed = 120.0f;

        public float yMinLimit = -20; //Limits at how far an angle you can go over or under the object
        public float yMaxLimit = 80;

        private float x = 0.0f;
        private float y = 0.0f;

        void Start () 
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

        }

        void Update () 
        {
		
			if (target && Input.GetMouseButton(0))
            {
				//This is called whenever the user is holding down the mouse button
				//Sets the x and y axis to the new rotation desired.
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
		
			if(target)
			{
				//This is called when the user is scrolling, or not doing anything
				transform.rotation = Quaternion.Euler(y, x, 0);
                transform.position = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position;
				distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel"), distanceMin, distanceMax);	
			
			}
          
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