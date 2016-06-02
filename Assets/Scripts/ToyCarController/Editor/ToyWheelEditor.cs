using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ToyWheelController)), CanEditMultipleObjects]
public class ToyWheelEditor : Editor {

    private SerializedProperty m_Radius;
    private SerializedProperty m_LocalNormal;

    private void OnEnable() {
        m_Radius = serializedObject.FindProperty("radius");
        m_LocalNormal = serializedObject.FindProperty("localNormal");
    }
    
    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_LocalNormal);
        serializedObject.ApplyModifiedProperties();
    }
    
    void OnSceneGUI() {
        ToyWheelController wheel = (ToyWheelController) this.target;

        DrawWheel(wheel);
    }

    public static void DrawWheel(ToyWheelController wheel) {
        // Draw a circle to show the wheels radius.
        int N = 16;
        Vector3 wheelNormal = wheel.GetNormal();
        Quaternion rotation = Quaternion.AngleAxis(360f / N, wheelNormal);
        Vector3 displacement = wheel.radius * Vector3.Cross(wheelNormal, wheelNormal + Vector3.forward).normalized;

        for (int j = 0; j < N; j++) {
            Vector3 pointA = wheel.transform.position + displacement;
            displacement = rotation * displacement;
            Vector3 pointB = wheel.transform.position + displacement;
            Debug.DrawLine(pointA, pointB, Color.cyan);
        }
    }
}
