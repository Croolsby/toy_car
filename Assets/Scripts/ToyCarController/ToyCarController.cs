using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 * An unphysical car controller.
 * The car will be able to turn, accelerate, and deccelerate quickly, like a toy car.
 */
[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class ToyCarController : MonoBehaviour {
  public static ToyCarController playerInstance;

  public float m_TopLinearSpeed = 10;
  public float m_TopAngularSpeed = 1;
  public float m_TimeUntilTopLinear = 1;
  public float m_TimeUntilTopAngular = 1;
  public bool m_IsPlayer = false;

  [HideInInspector, SerializeField]
  private List<ToyWheelController> m_Wheels;
  [HideInInspector, SerializeField]
  private Rigidbody m_Rigidbody;
  private float m_CurrentLinearSpeed;
  private float m_CurrentAngularSpeed;
  private float m_SpeedOfLinearVelocity;
  private float m_SpeedOfAngularVelocity;

  public float Revs {
    get {
      float ratio = Mathf.Clamp01(m_Rigidbody.velocity.magnitude / m_TopLinearSpeed);
      if (ratio > 0.7f) {
        return 0.22f;
      } else {
        return ratio * 0.4f;
      }
    }
  }

  public float AccelInput {
    get {
      return Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
    }
  }

  // Used to instantiate locals.
  void Awake() {
    m_Wheels = new List<ToyWheelController>();
    m_SpeedOfLinearVelocity = 0;
    m_SpeedOfAngularVelocity = 0;
    m_CurrentLinearSpeed = 0;
    m_CurrentAngularSpeed = 0;

    if (m_IsPlayer) { // Player instance singleton
      if (playerInstance == null) {
        playerInstance = this;
      } else {
        Destroy(gameObject);
      }
    }
  }

  // Used to get references to other components.
  void Start() {
    m_Rigidbody = GetComponent<Rigidbody>();
    FindWheels();
  }

  void Update() {
    // Read input;
    float inputH = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
    float inputV = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);


    // Linear stuff:
    float targetLinearSpeed = inputV * m_TopLinearSpeed;

    // a forward vector based on the car's orientation.
    Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized; // going to overwrite the horizontal component of velocity with this
    Vector3 planarComponent = Vector3.ProjectOnPlane(m_Rigidbody.velocity, Vector3.up);
    Vector3 verticalComponent = Vector3.Project(m_Rigidbody.velocity, Vector3.up); // retain vertical component of velocity

    // get the current linear speed from the current rigidbody velocity, except that the vertical component should not contribute to the speed,
    // and since .magnitude returns an abolute value, dot the velocity with the car's forward direction.
    m_CurrentLinearSpeed = planarComponent.magnitude * Mathf.Sign(Vector3.Dot(forward, planarComponent));

    // Use spring-like behavior to ramp up to max speed.
    m_CurrentLinearSpeed = Mathf.SmoothDamp(m_CurrentLinearSpeed, targetLinearSpeed, ref m_SpeedOfLinearVelocity, m_TimeUntilTopLinear);

    // Apply changes to rigidbody.
    m_Rigidbody.velocity = m_CurrentLinearSpeed * forward + verticalComponent;

    // Angular Stuff:
    if (inputV < 0)
      inputH = -inputH; // so that holding right will go to the right when reversing

    float targetAngularSpeed = inputH * m_TopAngularSpeed;

    m_CurrentAngularSpeed = Mathf.SmoothDampAngle(m_CurrentAngularSpeed, targetAngularSpeed, ref m_SpeedOfAngularVelocity, m_TimeUntilTopAngular);

    Vector3 orthogonalComponent = m_Rigidbody.angularVelocity - Vector3.Project(m_Rigidbody.angularVelocity, Vector3.up); // retain orthogonal components to angular velocity

    // Use linear speed ratio to prevent turning when moving slow.
    m_Rigidbody.angularVelocity = Mathf.Abs(m_CurrentLinearSpeed) / m_TopLinearSpeed * m_CurrentAngularSpeed * Vector3.up + orthogonalComponent;
  }

  public Rigidbody GetRigidbody() {
    return m_Rigidbody;
  }

  /*
   * Searches for any ToyWheelControllers and then stores them.
   */
  public void FindWheels() {
    ToyWheelController[] wheelArray = GetComponentsInChildren<ToyWheelController>();

    m_Wheels.Clear();
    for (int i = 0; i < wheelArray.Length; i++) {
      m_Wheels.Add(wheelArray[i]);
    }
  }

  public ToyWheelController[] GetWheels() {
    return m_Wheels.ToArray();
  }
}
