using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (AudioSource))]
    public class WheelEffects : MonoBehaviour
    {
        public Transform SkidTrailPrefab;
        public static Transform skidTrailsDetachedParent;
        public ParticleSystem skidParticles;
        public bool skidding { get; private set; }
        public bool PlayingAudio { get; private set; }


        public AudioSource m_AudioSource;
        public Transform m_SkidTrail;
        public WheelCollider m_WheelCollider;
		// Faris: I changed to public because these were always null, the lines commmented below didn't work...

        private void Start()
        {
            skidParticles = transform.root.GetComponentInChildren<ParticleSystem>();

            if (skidParticles == null)
            {
                Debug.LogWarning(" no particle system found on car to generate smoke particles");
            }
            else
            {
                skidParticles.Stop();
            }

            // ... these ones. (So instead, I made it public and set it in the Inspector.)
			//m_WheelCollider = GetComponent<WheelCollider>();
            m_AudioSource = GetComponent<AudioSource>();
            PlayingAudio = false;

            if (skidTrailsDetachedParent == null)
            {
                skidTrailsDetachedParent = new GameObject("Skid Trails - Detached").transform;
            }
        }


		public void logNull(string s, Object o)
		{
			if (o == null)
				Debug.LogWarning (s + " is null");
			else
				Debug.LogWarning (s + "is not null");
		}


        public void EmitTyreSmoke()
        {
            logNull("skidParticles", skidParticles);
			logNull("m_WheelCollider", m_WheelCollider);

			skidParticles.transform.position = transform.position - transform.up*m_WheelCollider.radius;
            skidParticles.Emit(1);
            if (!skidding)
            {
                StartCoroutine(StartSkidTrail());
            }
        }


        public void PlayAudio()
        {
            m_AudioSource.Play();
            PlayingAudio = true;
        }


        public void StopAudio()
        {
            m_AudioSource.Stop();
            PlayingAudio = false;
        }


        public IEnumerator StartSkidTrail()
        {
            skidding = true;
            m_SkidTrail = Instantiate(SkidTrailPrefab);
            while (m_SkidTrail == null)
            {
                yield return null;
            }
            m_SkidTrail.parent = transform;
            m_SkidTrail.localPosition = -Vector3.up*m_WheelCollider.radius;
        }


        public void EndSkidTrail()
        {
            if (!skidding)
            {
                return;
            }
            skidding = false;
            m_SkidTrail.parent = skidTrailsDetachedParent;
            Destroy(m_SkidTrail.gameObject, 10);
        }
    }
}
