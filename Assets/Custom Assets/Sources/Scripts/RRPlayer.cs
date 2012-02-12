using UnityEngine;
using System.Collections;

public class RRPlayer : MonoBehaviour {
	private Vector3		_worldPosition = Vector3.zero;
	
	
	public Vector3 WorldPosition {
		set { _worldPosition = value; }
		get { return _worldPosition;  }
	}
	
}
