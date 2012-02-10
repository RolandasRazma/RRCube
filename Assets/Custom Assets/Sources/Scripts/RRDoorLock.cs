using UnityEngine;
using System.Collections;


public class RRDoorLock : MonoBehaviour {
	public RRDoor door;
	
	
	private void OnTriggerEnter(Collider collider) {
		if( collider.gameObject.CompareTag("Player") ){
			this.door.Locked = true;
		}
	}
	
	
	private void OnTriggerExit(Collider collider) {
		if( collider.gameObject.CompareTag("Player") ){
			this.door.Locked = false;
		}
	}
	

}
