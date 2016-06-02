using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public CarController m_Car; // the car controller we want to use
		// This was private, I changed it so I could set it from the Inspector.

        void Awake()
        {
            // get the car controller -- done from Inspector
            //m_Car = GetComponent<CarController>();
			Console.WriteLine ("m_Car null: " + (m_Car == null));
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
			Debug.Log ("h is " + h + ", v is " + v);
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
