using UnityEngine;
using System.Collections;


public static class GameObjectRRExtensions {
	
	public static GameObject FindParentWithTag(this GameObject gameObject, string tag) {
		return gameObject.transform.FindParentWithTag(tag);
	}
	
}


public static class ColliderRRExtensions {
	
	public static GameObject FindParentWithTag(this Collider collider, string tag) {
		return collider.transform.FindParentWithTag(tag);
	}
	
}


public static class TransformRRExtensions {
	
	public static GameObject FindParentWithTag(this Transform transform, string tag) {
		Transform parent = transform.parent;
		while( parent != null ) { 
			if ( parent.CompareTag(tag) ) break;
			parent = parent.parent;
		}	
		
		return parent.gameObject;
	}
	
}