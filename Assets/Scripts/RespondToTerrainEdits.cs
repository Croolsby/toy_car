using UnityEngine;
using System.Collections;

// Attach this script to a game objects with children.
// It will update all the children's positions to be at the surface of the terrain.
[ExecuteInEditMode]
public class RespondToTerrainEdits : MonoBehaviour {
	public Collider terrainCollider;
	
	// Update is called once per frame
	void Update () {
		Ray ray;
		
		float disp = 1000;
		
		// int i = 0;
		foreach (Transform child in transform) {
			Vector3 rayOrigin = new Vector3(child.position.x, child.position.y - disp, child.position.z);
			ray = new Ray(rayOrigin, Vector3.up);
			
			RaycastHit hitInfo;
			bool hit = terrainCollider.Raycast(ray, out hitInfo, 2*disp);
			
			// Debug.DrawRay(rayOrigin, Vector3.up * 2 *disp);
			
			if (hit) {
				child.position = hitInfo.point; 
			}
			
			// i++;
		}
		
		// Debug.Log("i: " + i);
	}
}
