using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ToyCarController))]
public class ToyCarEditor : Editor {
    // Called when user interacts with inspector.
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        //Debug.Log("ToyCarEditor.OnInspectorGUI");

        // Provide a way to add wheels to the car besides using ExecuteInEditMode
        ToyCarController car = (ToyCarController) target;
        if (GUILayout.Button("Find wheels")) {
            car.FindWheels();
        }
    }

    void OnSceneGUI() {
        //Debug.Log("ToyCarController.OnSceneGUI");

        ToyCarController car = (ToyCarController) target;

        DrawWheels(car);
    }

    public static void DrawWheels(ToyCarController car) {
        ToyWheelController[] wheels = car.GetWheels();

        for (int i = 0; i < wheels.Length; i++) {
            ToyWheelController wheel = wheels[i];

            ToyWheelEditor.DrawWheel(wheel);

            Debug.DrawRay(wheel.transform.position, wheel.radius * wheel.GetNormal(), Color.cyan);
            Debug.DrawLine(car.transform.position, wheel.transform.position, Color.cyan);
        }
    }
}
