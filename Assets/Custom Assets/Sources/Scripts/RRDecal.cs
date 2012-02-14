using UnityEngine;
using System.Collections;

public static class RRDecal {
	
	// TODO: This is fuckedup and tottaly hacked. Need to come back later and remake it (when i will learn more about Unity3D)
	public static void PlaceDecal( GameObject gameObject, RaycastHit hit ){
		// This one works for cube - Quaternion.LookRotation(hit.normal)
		// This one DON'T work for cube - Quaternion.FromToRotation(Vector3.back, hit.normal)
		gameObject = (GameObject)GameObject.Instantiate(gameObject, hit.point +(hit.normal *0.01f), Quaternion.FromToRotation(Vector3.back, hit.normal));
		gameObject.transform.parent = hit.transform;
		
		
		// HACK: Wonder what is correct way to do this
		Vector3 r = gameObject.transform.eulerAngles;
		r.z = 0;
		gameObject.transform.eulerAngles = r;
	
		
		// how to position
		// http://axe78.blogspot.com/2010/11/decals-in-unity3d-part-2.html	
		
		// Acquire necessary information
		MeshFilter mf = gameObject.GetComponent<MeshFilter>();
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


public class Decals {
	
    // no instantiating!
    private Decals() { }

    private static Mesh _plane;

    public static GameObject CreateDecal(Ray target, Texture texture) {
        GameObject g = _createDecal(target, new Vector2(1, 1));
        g.renderer.material.mainTexture = texture;
        return g;
    }

    public static GameObject CreateDecal(Ray target, Material material) {
        GameObject g = _createDecal(target, new Vector2(1, 1));
        g.renderer.material = material;
        return g;
    }

    public static GameObject CreateDecal(Ray target, Texture texture, Vector2 scale) {
        GameObject g = _createDecal(target, scale);
        g.renderer.material.mainTexture = texture;
        return g;
    }

    public static GameObject CreateDecal(Ray target, Material material, Vector2 scale) {
        GameObject g = _createDecal(target, scale);
        g.renderer.material = material;
        return g;
    }

    private static GameObject _createDecal(Ray target, Vector2 scale) {
        RaycastHit hit;
        if (Physics.Raycast(target, out hit))
        {
            GameObject go = new GameObject("Decal");

            go.AddComponent(typeof(MeshRenderer));
            MeshFilter m = go.AddComponent(typeof(MeshFilter)) as MeshFilter;

            m.mesh = _getMesh();

            go.transform.localScale = new Vector3(scale.x, scale.y, 1);

            go.transform.position = hit.point;
            go.transform.LookAt(hit.point + hit.normal);
            go.transform.Translate(Vector3.forward * 0.0001f);
            go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, 0);
            return go;
        }
        else
        {
            Debug.LogWarning("The raycast for the decal didn't hit anything!");
            return null;
        }
    }

    private static Mesh _getMesh() {
        if (_plane != null) return _plane;

        _plane = new Mesh();

        _plane.name = "Decal Plane Mesh";

        // Create verticies for "flat" plane (normal up).
        Vector3[] verts = new Vector3[4];
        verts[0] = new Vector3(-.5f, -.5f, 0);
        verts[1] = new Vector3(.5f, -.5f, 0);
        verts[2] = new Vector3(-.5f, .5f, 0);
        verts[3] = new Vector3(.5f, .5f, 0);

        int[] tris = new int[] { 0, 1, 2, 3, 2, 1 };

        Vector2[] uvs = new Vector2[verts.Length];
		
        uvs[0] = new Vector2(1, 0);
        uvs[1] = new Vector2(0, 0);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);

        _plane.vertices = verts;
        _plane.triangles = tris;
        _plane.uv = uvs;
        _plane.RecalculateNormals();

        return _plane;
    }
	
}