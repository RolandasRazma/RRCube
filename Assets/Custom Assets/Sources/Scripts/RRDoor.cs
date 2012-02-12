using UnityEngine;
using System.Collections;


public class RRDoor : MonoBehaviour {
	private Vector3 _moveDirection = Vector3.zero;
	private Vector3 _defaultLocation;
	private RRCube  _cube;
	private bool 	_open;
	private bool	_locked;
	
	public AudioClip openSounds;
	public AudioClip closeSounds;
	public AudioClip lockedSounds;
	public static string StateNotification = "StateNotification";
	
	
	private void Start(){
		_defaultLocation = this.transform.localPosition;
	}

	
	private void Update(){
		if( !_moveDirection.Equals(Vector3.zero) ){
			this.transform.Translate(_moveDirection *3.1f *Time.deltaTime);

			if( _moveDirection.z > 0 && this.transform.localPosition.z >= 0.0f ){
				//Closed
				_open = false;
				_moveDirection = Vector3.zero;
				this.transform.localPosition = _defaultLocation;
				this.audio.Stop();
				
				Messenger<RRDoor>.Broadcast(RRDoor.StateNotification, this);
			}else if( _moveDirection.z < 0 && this.transform.localPosition.z <= -3.1f ){
				// Open
				_moveDirection = Vector3.zero;
				this.audio.Stop();

				StartCoroutine("CloseTimer");
			}
			
		}
	}
	
	
	public void Open() {
		_open = true;

		_moveDirection = Vector3.back;
		this.audio.PlayOneShot(this.openSounds);
		
		Messenger<RRDoor>.Broadcast(RRDoor.StateNotification, this);
	}
	
	
	private IEnumerator CloseTimer(){
		yield return new WaitForSeconds(2);
		this.Close();
	}
	
	
	private void Close(){
		if( _locked ){
			StartCoroutine("CloseTimer");
			return;
		}
			
		_moveDirection = Vector3.forward;
		
		this.audio.PlayOneShot(this.closeSounds);
	}
	
	
	public RRCube Cube {
		set { _cube = value; }
		get { return _cube;  }
	}
	
	
	public bool isOpen {
		get { return _open; }
	}
	
	
	public bool Locked {
		set { _locked = value; }
		get { return _locked;  }
	}
	
}
