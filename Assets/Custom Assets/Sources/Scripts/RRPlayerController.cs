using UnityEngine;
using System.Collections;


public class RRPlayerController : MonoBehaviour {
	private Rect _targetRect;
	private bool _onLadder;
	private CharacterMotor _motorMovement;
	private CharacterController _characterController;
	
	public Texture2D targetTexture;
	
	
	private void Start() {
		Screen.showCursor = false;
		
		_motorMovement = this.GetComponent<CharacterMotor>();
		_characterController = this.GetComponent<CharacterController>();
	}
	
	
	private void OnGUI(){
		
		_targetRect = new Rect(Screen.width /2 -1, Screen.height /2 -1, 2, 2);
		
		if( this.targetTexture ){
			GUI.DrawTexture(_targetRect, this.targetTexture, ScaleMode.StretchToFill, true);
		}else{
			GUI.Box(_targetRect, "");
		}
		
	}
	
	
	private void Update() {
		
		if( Input.GetButtonDown("Use") ){
			RaycastHit hit;
			Ray ray = Camera.mainCamera.ScreenPointToRay(new Vector2(_targetRect.x, _targetRect.y));
			if ( Physics.Raycast(ray, out hit, 8) ){
				
				// Did we targeted door?
				RRDoor door = hit.transform.gameObject.GetComponent<RRDoor>();
				if( door ){
					if( door.Cube ){
						door.Cube.OpenDoor(door);
					}
				}else if( !RRGame.SharedInstance.HasOpenDoors ){
					//GameObject
					GameObject gameObject = (GameObject)Resources.Load("Prefabs/Decal");
					
					RRDecals.PlaceDecal(gameObject, hit);
					
					// To allow placing decals with open doors this should be changed
					RRGame.SharedInstance.Player.Cube.WorldBound = true;
				}
				
				// Debug
				// Debug.DrawRay(ray.origin, ray.direction *hit.distance, Color.red); 
				// hit.transform.renderer.material.color = Color.red;
			}
		}
		
	}
	
	
	private void FixedUpdate(){

		if( _onLadder ){
			Vector3 lateralMove = this.transform.InverseTransformDirection(_motorMovement.inputMoveDirection);
			lateralMove *= _motorMovement.MaxSpeedInDirection(lateralMove);
			
			// If goinf forward and hitting wall
			if( (_characterController.collisionFlags & CollisionFlags.Sides) != 0 ){
				lateralMove.y = _motorMovement.movement.maxForwardSpeed *Input.GetAxis("Vertical");
			}
			
			// If going backward
			if( lateralMove.z < 0 && !_characterController.isGrounded ){
				lateralMove.z = 0;
				lateralMove.y = _motorMovement.movement.maxForwardSpeed *Input.GetAxis("Vertical");
			}
			
			// If jumping - slide down
			if( Input.GetButton("Jump") ){
				lateralMove.y = -_motorMovement.movement.gravity /3;
			}
			
			lateralMove = this.transform.TransformDirection(lateralMove);
			
			this.Move(lateralMove *Time.deltaTime);
		}

	}
	
	
	private void OnTriggerEnter(Collider collider) {
		if( collider.gameObject.name == "Ladder" || collider.gameObject.CompareTag("Ladder") ){
			_onLadder = true;
			_motorMovement.enabled = false;
		}
	}
	
	
	private void OnTriggerExit(Collider collider) {
		if( collider.gameObject.name == "Ladder" || collider.gameObject.CompareTag("Ladder") ){
			_onLadder = false;
			_motorMovement.enabled = true;
		}
	}
	
	
	private CollisionFlags Move( Vector3 motion ){
		Vector3 lastPosition = this.transform.position;
		CollisionFlags collisionFlags = _characterController.Move(motion);
		_motorMovement.movement.velocity = (this.transform.position -lastPosition) /Time.deltaTime;
		
		return collisionFlags;
	}
	

}
