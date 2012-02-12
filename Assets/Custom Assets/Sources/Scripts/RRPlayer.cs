using UnityEngine;
using System.Collections;

public class RRPlayer : MonoBehaviour {
	private RRCube  _cube;
	
	
	public RRCube Cube {
		set { _cube = value; }
		get { return _cube;  }
	}
	
}
