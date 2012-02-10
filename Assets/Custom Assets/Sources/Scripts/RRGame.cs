using UnityEngine;


public sealed class RRGame {
	private static readonly RRGame _instance = new RRGame();
	private static readonly string _cubePrefabPath = "Prefabs/Cube";
	private static GameObject _player;
	private bool _hasOpenDoors;
	
	
	public static RRGame SharedInstance {
		get { return _instance; }
	}
	
	
	private RRGame(){
		Messenger<RRDoor>.AddListener(RRDoor.StateNotification, DoorStateChanged);
		_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	private void DoorStateChanged(RRDoor door){
		_hasOpenDoors = door.isOpen;

		if( _hasOpenDoors == false ){
			
			RaycastHit hit;
			Physics.Raycast(_player.transform.position, Vector3.down, out hit);
						
			Transform parent = hit.collider.transform.parent;
			while( parent != null ) { 
				if ( parent.CompareTag("Cube") ) break;
				parent = parent.parent;
			}

			this.PlayerCube( parent.GetComponent<RRCube>() );
			
		}
	}
	
	
	public bool HasOpenDoors {
		get { return _hasOpenDoors; }
	}
	
	
	public void SpawnCubeInDirection( Vector3 direction, RRCube relativeToCube ){
		
		// Todo - check if there is no cube created
		GameObject newCube = (GameObject)Resources.Load(_cubePrefabPath);
		newCube = (GameObject)GameObject.Instantiate(newCube, relativeToCube.transform.position +direction *12, relativeToCube.transform.rotation);
		
		RRCube cube = newCube.GetComponent<RRCube>();
		cube.WorldPosition = relativeToCube.WorldPosition +direction;
		
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
	
	
	private void PlayerCube( RRCube playerCube ){
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
	
		playerCube.gameObject.SetActiveRecursively(true);
		
		// Destroy other cubes
		foreach( GameObject cube in cubes ){
			if( cube != playerCube.gameObject ){
				GameObject.Destroy(cube);
			}
		}

	}
		
	
}