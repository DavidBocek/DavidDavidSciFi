using UnityEngine;
using System.Collections;

public class PlayerNetworkManager : Photon.MonoBehaviour {

	private PhotonView view;
	private CompanionAnimationManager companionAnimManager;

	private bool isProxy;
	//for movement
	private Vector3 curPosition;
	private Quaternion curRotation;
	//for animation
	private Vector3 curVelScaled;
	private bool isSprinting;

	// Use this for initialization
	void Start () {
		view = PhotonView.Get(this);
		companionAnimManager = GetComponentInChildren<CompanionAnimationManager>();
	}

	public void SetUpLocalPlayer(){
		isProxy = false;
		GetComponentInChildren<CompanionAnimationManager>().gameObject.SetActive(false);
	}

	public void SetUpProxyCompanion(){
		isProxy = true;
		GetComponentInChildren<MovementManager>().gameObject.SetActive(false);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//ours
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(curVelScaled);
			stream.SendNext(isSprinting);
		} else {
			//proxy
			curPosition = (Vector3) stream.ReceiveNext();
			curRotation = (Quaternion) stream.ReceiveNext();
			curVelScaled = (Vector3) stream.ReceiveNext();
			isSprinting = (bool) stream.ReceiveNext();
		}
	}

	void FixedUpdate(){
		if (!isProxy){
			//ours
			//nothing for now? we update on our screen through movement manager
		} else {
			//proxy
			transform.position = curPosition;
			transform.rotation = curRotation;
			companionAnimManager.SetCurSpeedScaledFloat(curVelScaled.magnitude);
			companionAnimManager.SetCurVelScaledXFloat(curVelScaled.x);
			companionAnimManager.SetCurVelScaledZFloat(curVelScaled.z);
			companionAnimManager.SetSprintingBool(isSprinting);
		}
	}

	public void UpdateVelocityValues(Vector3 curVelScaled){
		this.curVelScaled = curVelScaled;
	}

	public void UpdateSprinting(bool isSprinting){
		this.isSprinting = isSprinting;
	}


}
