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
	}
	
	
	public bool OpenDoor(RRDoor door){
		if( RRGame.SharedInstance.HasOpenDoors ) {
			door.gameObject.audio.PlayOneShot( door.lockedSounds );
			// You can't open more than one door at a time
			return false;
		}

		Vector3 nextCubePosition = Vector3.zero;
		if( this.doorUp == door ){
			nextCubePosition.y++;
		}else if( this.doorDown == door ){
			nextCubePosition.y--;
		}else if( this.doorFront == door ){
			nextCubePosition.x--;
		}else if( this.doorBack == door ){
			nextCubePosition.x++;
		}else if( this.doorRight == door ){
			nextCubePosition.z++;
		}else if( this.doorLeft == door ){
			nextCubePosition.z--;
		}
		
		RRGame.SharedInstance.SpawnCubeInDirection( nextCubePosition, this );
		
		door.Open();
		
		return true;
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
