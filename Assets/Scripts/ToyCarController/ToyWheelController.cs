using UnityEngine;
using System.Collections;
using System;

/*
 * The wheels that go with ToyCarController.
 *
 * Wheels should be game objects whose parent is a ToyCarController. Not the same object as the ToyCarController.
 * The ToyCarController will find them when it loads.
 */
[Serializable]
public class ToyWheelController : MonoBehaviour {
    public float radius = 1;
    public Vector3 localNormal;

    public Vector3 GetNormal() {
        return transform.rotation * localNormal;
    }
}
