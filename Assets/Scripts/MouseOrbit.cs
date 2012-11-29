using UnityEngine;
using System.Collections;

    public class MouseOrbit : MonoBehaviour 
    {
        public Transform target;
        public float distance = 10.0f;
		public float distanceMin;
		public float distanceMax;

        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20;
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
			if(target)
			{
				transform.rotation = Quaternion.Euler(y, x, 0);
                transform.position = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position;
				distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel"), distanceMin, distanceMax);	
			
				//Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
			
			}
            if (target && Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
			
				

            }
        }

        static float ClampAngle(float angle, float min, float max) 
        {
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