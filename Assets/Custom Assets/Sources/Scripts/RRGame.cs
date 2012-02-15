using UnityEngine;
using System.Collections.Generic;


public sealed class RRGame {
	private static readonly RRGame _instance = new RRGame();
	private static readonly string _cubePrefabPath = "Prefabs/Cube";
	private static RRPlayer _player;
	private bool _hasOpenDoors;
	private Dictionary<Vector3, RRCube> _world = new Dictionary<Vector3, RRCube>();
	
	
	public static RRGame SharedInstance {
		get { return _instance; }
	}
	
	
	private RRGame(){
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<RRPlayer>();

		this.CalculatePlayerCube();
		
		_world.Add(_player.Cube.WorldPosition, _player.Cube);

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
		RRCube cube;
		if( !(cube = this.CubeAtWorldPosition( worldPosition )) ){
			// Create new Cube
			GameObject newCube;
		
			newCube = (GameObject)Resources.Load(_cubePrefabPath);
			newCube = (GameObject)GameObject.Instantiate(newCube, relativeToCube.transform.position +direction *12, relativeToCube.transform.rotation);
		
			cube = newCube.GetComponent<RRCube>();
			cube.WorldPosition = worldPosition;
			
			// Add to generated cubes list
			_world.Add(worldPosition, cube);
		}

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

		RRCube activeCube = hit.collider.FindParentWithTag("Cube").GetComponent<RRCube>();
		
		// Activate doors
		activeCube.gameObject.SetActiveRecursively(true);
		
		// Assign to player
		_player.Cube = activeCube;
		
		// Destroy nonbound cubes
		List<RRCube> cubes = new List<RRCube>(_world.Values);
		foreach( RRCube cube in cubes ){
			if( cube != activeCube && cube.WorldBound == false ){
				_world.Remove(cube.WorldPosition);
				GameObject.Destroy(cube.gameObject);
			}
		}
	}
	
	
	public RRCube CubeAtWorldPosition( Vector3 worldPosition ){
		return (_world.ContainsKey( worldPosition )?_world[worldPosition]:null);
	}
	
	
}