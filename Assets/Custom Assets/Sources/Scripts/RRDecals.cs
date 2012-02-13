using UnityEngine;
using System.Collections;

public static class RRDecals {

	public static void PlaceDecal( GameObject gameObject, RaycastHit hit ){
		// This one works for cube - Quaternion.LookRotation(hit.normal)
		// This one DON'T work for cube - Quaternion.FromToRotation(Vector3.back, hit.normal)
		gameObject = (GameObject)GameObject.Instantiate(gameObject, hit.point +(hit.normal *0.01f), Quaternion.FromToRotation(Vector3.back, hit.normal));
		gameObject.transform.parent = hit.transform;
		
		
		// HACK: Wonder what is correct way
		Vector3 r = gameObject.transform.eulerAngles;
		r.z = 0;
		gameObject.transform.eulerAngles = r;
	
		
		// how to place
		// http://axe78.blogspot.com/2010/11/decals-in-unity3d-part-2.html	
		
		// Acquire necessary information
		MeshFilter mf = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		Mesh m = mf.mesh;
		
		// Create a new array to store the decal mesh vertices
		Vector3[] verts = m.vertices;
		
		// Loop through all the delcal mesh vertices, convert them in local space
		// and check if any of them exceeds the hit collider bounds.
		for ( int i = 0; i < verts.Length; i++ ){
			verts[i] = gameObject.transform.TransformPoint(verts[i]);
			
			if ( verts[i].x > hit.collider.bounds.max.x ){
				verts[i].x = hit.collider.bounds.max.x;
			}
			
			if ( verts[i].x < hit.collider.bounds.min.x ){
				verts[i].x = hit.collider.bounds.min.x;
			}
			
			if ( verts[i].y > hit.collider.bounds.max.y ){
				verts[i].y = hit.collider.bounds.max.y;
			}
			
			if ( verts[i].y < hit.collider.bounds.min.y ) {
				verts[i].y = hit.collider.bounds.min.y;
			}
			
			if ( verts[i].z > hit.collider.bounds.max.z ) {
				verts[i].z = hit.collider.bounds.max.z;
			}
			
			if ( verts[i].z < hit.collider.bounds.min.z ) {
				verts[i].z = hit.collider.bounds.min.z;
			}
			
			// Convert the vertices back in world space and assign them bacl to the decal mesh.
			verts[i] = gameObject.transform.InverseTransformPoint(verts[i]);
			m.vertices = verts;
		}
		
		// Move to front abit to fix flicker
		gameObject.transform.Translate(Vector3.back *0.01f);
		
	}

}
