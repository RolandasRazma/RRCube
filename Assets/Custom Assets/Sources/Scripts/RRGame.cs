using UnityEngine;
using System.Collections.Generic;


public sealed class RRGame {
	private static readonly RRGame _instance = new RRGame();
	private static readonly string _cubePrefabPath = "Prefabs/Cube";
	private static RRPlayer _player;
	private bool _hasOpenDoors;
	private HashSet<Vector3> _world = new HashSet<Vector3>();
	

	public static RRGame SharedInstance {
		get { return _instance; }
	}
	
	
	private RRGame(){
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<RRPlayer>();
		
		_world.Add(Vector3.zero);
		
		this.CalculatePlayerCube();
		
		Messenger<RRDoor>.AddListener(RRDoor.StateNotification, DoorStateChanged);
	}
	
	
	private void DoorStateChanged(RRDoor door){
		_hasOpenDoors = door.isOpen;
		
		if( door.isOpen ){
			this.SpawnCubeInDirection( door.Direction, door.Cube );
		}
		
		if( _hasOpenDoors == false ){
			this.CalculatePlayerCube();
		}
	}
	
	
	public bool HasOpenDoors {
		get { return _hasOpenDoors; }
	}
	
	
	public RRPlayer Player {
		get { return _player; }
	}
	
	
	public void SpawnCubeInDirection( Vector3 direction, RRCube relativeToCube ){
		Vector3 worldPosition = relativeToCube.WorldPosition +direction;
		
		// Did we generated this cube before?
		if( _world.Contains(worldPosition) ) return;

		// Add to generated cubes list
		_world.Add(worldPosition);
		
		// Create new Cube
		GameObject newCube = (GameObject)Resources.Load(_cubePrefabPath);
		newCube = (GameObject)GameObject.Instantiate(newCube, relativeToCube.transform.position +direction *12, relativeToCube.transform.rotation);
		
		RRCube cube = newCube.GetComponent<RRCube>();
		cube.WorldPosition = worldPosition;

		if( direction.y < 0 ){
			cube.doorUp.gameObject.SetActiveRecursively(false);
		}else if( direction.y > 0 ){
			cube.doorDown.gameObject.SetActiveRecursively(false);
		}else if( direction.x < 0 ){
			cube.doorBack.gameObject.SetActiveRecursively(false);
		}else if( direction.x > 0 ){
			cube.doorFront.gameObject.SetActiveRecursively(false);
		}else if( direction.z < 0 ){
			cube.doorRight.gameObject.SetActiveRecursively(false);
		}else if( direction.z > 0 ){
			cube.doorLeft.gameObject.SetActiveRecursively(false);
		}
		
	}
	
	
	private void CalculatePlayerCube(){
		RaycastHit hit;
		Physics.Raycast(_player.transform.position, Vector3.down, out hit);
					
		// TODO: i could save current and cube next door so wouldnt need check all cubes - just 2
		Transform parent = hit.collider.transform.parent;
		while( parent != null ) { 
			if ( parent.CompareTag("Cube") ) break;
			parent = parent.parent;
		}
		
		this.PlayerCube( parent.GetComponent<RRCube>() );
	}
	
	
	private void PlayerCube( RRCube playerCube ){
		_player.Cube = playerCube;
		
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
	
		playerCube.gameObject.SetActiveRecursively(true);
		
		// Destroy other cubes
		foreach( GameObject gameObject in cubes ){
			RRCube cube = gameObject.GetComponent<RRCube>();
			if( gameObject != playerCube.gameObject && cube.WorldBound == false ){
				_world.Remove(cube.WorldPosition);
				GameObject.Destroy(gameObject);
			}
		}
	}
		
	
}