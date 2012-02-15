using UnityEngine;
using System.Collections;

public static class RRDecal {
	
	private static Mesh _planeMesh;
	
	
	public static GameObject PlaceDecal( Texture texture, RaycastHit hit ){
		return RRDecal.PlaceDecal( texture, hit, new Vector2(1, 1) );
	}
	

	public static GameObject PlaceDecal( Texture texture, RaycastHit hit, Vector2 scale ){
        GameObject gameObject = new GameObject("Decal");

        gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = RRDecal.PlaneMesh;
			
        gameObject.transform.localScale = new Vector3(scale.x, scale.y, 1);
        gameObject.transform.position = hit.point;
        gameObject.transform.LookAt(hit.point +hit.normal);
        gameObject.transform.Translate(Vector3.forward *0.0001f);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
		gameObject.transform.parent = hit.transform;

		gameObject.renderer.material = new Material( Shader.Find("Transparent/Diffuse") );
    	gameObject.renderer.material.mainTexture = texture;
		
		RRDecal.PositionMeshInBounds( gameObject, hit.collider.bounds );
		
		return gameObject;
	}
	
	
	private static Mesh PlaneMesh {
		get {
			if (_planeMesh != null) return _planeMesh;

	        _planeMesh = new Mesh();
	
	        _planeMesh.name = "Decal Plane Mesh";
	
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
	
	        _planeMesh.vertices = verts;
	        _planeMesh.triangles = tris;
	        _planeMesh.uv = uvs;
	        _planeMesh.RecalculateNormals();
	
	        return _planeMesh;
		}
	}
	
	
	private static void PositionMeshInBounds( GameObject gameObject, Bounds bounds ){
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

		Vector3[] verts = meshFilter.mesh.vertices;
		for ( int i = 0; i < verts.Length; i++ ){
			verts[i] = gameObject.transform.TransformPoint(verts[i]);
			
			if ( verts[i].x > bounds.max.x ){
				verts[i].x = bounds.max.x;
			}
			
			if ( verts[i].x < bounds.min.x ){
				verts[i].x = bounds.min.x;
			}
			
			if ( verts[i].y > bounds.max.y ){
				verts[i].y = bounds.max.y;
			}
			
			if ( verts[i].y < bounds.min.y ) {
				verts[i].y = bounds.min.y;
			}
			
			if ( verts[i].z > bounds.max.z ) {
				verts[i].z = bounds.max.z;
			}
			
			if ( verts[i].z < bounds.min.z ) {
				verts[i].z = bounds.min.z;
			}
			
			// Convert the vertices back in world space and assign them bacl to the decal mesh.
			verts[i] = gameObject.transform.InverseTransformPoint(verts[i]);
		}
					
		meshFilter.mesh.vertices = verts;
		meshFilter.mesh.RecalculateNormals();
		
		gameObject.transform.Translate(Vector3.forward *0.0001f);
	}
	
}