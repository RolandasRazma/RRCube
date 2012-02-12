using UnityEngine;
using System.Collections;


public class RRCube : MonoBehaviour {
	private Vector3		_worldPosition = Vector3.zero;
	private bool		_worldBound;
	
	public RRDoor doorUp;
	public RRDoor doorDown;
	public RRDoor doorFront;
	public RRDoor doorBack;
	public RRDoor doorLeft;
	public RRDoor doorRight;

	
	private void Start() {
		this.gameObject.tag = "Cube";
		doorUp.Cube = doorDown.Cube = doorFront.Cube = doorBack.Cube = doorLeft.Cube = doorRight.Cube = this;
		
		this.doorUp.Direction	= new Vector3( 0, 1,  0);
		this.doorDown.Direction	= new Vector3( 0,-1,  0);
		this.doorFront.Direction= new Vector3(-1, 0,  0);
		this.doorBack.Direction = new Vector3( 1, 0,  0);
		this.doorRight.Direction= new Vector3( 0, 0,  1);
		this.doorLeft.Direction = new Vector3( 0, 0, -1);
	}
	
	
	public bool OpenDoor(RRDoor door){
		if( RRGame.SharedInstance.HasOpenDoors ) {
			door.gameObject.audio.PlayOneShot( door.lockedSounds );
			// You can't open more than one door at a time
			return false;
		}

		return door.Open();
	} 
	
	
	public Vector3 WorldPosition {
		set { _worldPosition = value; }
		get { return _worldPosition;  }
	}
		
	
	public bool WorldBound {
		set {_worldBound = value; }
		get { return _worldBound; }
	}
	
	
}
