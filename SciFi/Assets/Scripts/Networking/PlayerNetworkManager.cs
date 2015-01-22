using UnityEngine;
using System.Collections;

public class PlayerNetworkManager : Photon.MonoBehaviour {

	private PhotonView view;
	private CompanionAnimationManager companionAnimManager;
	private MovementManager movementManager;
	private WeaponAndAbilityManager weaponsManager;
	
	private bool isProxy;
	//for movement
	private Vector3 goalPosition;
	private Quaternion goalRotation;
	private GameObject movementObj;
	//for animation
	private Vector3 curVelScaled;
	private bool isSprinting;

	// Use this for initialization
	void Start () {
		view = GetComponent<PhotonView>();
		companionAnimManager = GetComponentInChildren<CompanionAnimationManager>();
		movementManager = GetComponentInChildren<MovementManager>();
		weaponsManager = GetComponentInChildren<WeaponAndAbilityManager>();
	}

	public void SetUpLocalPlayer(){
		isProxy = false;
		movementObj = GetComponentInChildren<MovementManager>().gameObject;
		GetComponentInChildren<CompanionAnimationManager>().gameObject.SetActive(false);
	}

	public void SetUpProxyCompanion(){
		isProxy = true;
		movementObj = GetComponentInChildren<CompanionAnimationManager>().gameObject;
		GetComponentInChildren<MovementManager>().gameObject.SetActive(false);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//ours
			stream.SendNext(movementObj.transform.position);
			stream.SendNext(movementObj.transform.rotation);
			stream.SendNext(curVelScaled);
			stream.SendNext(isSprinting);
		} else {
			//proxy
			goalPosition = (Vector3) stream.ReceiveNext();
			goalRotation = (Quaternion) stream.ReceiveNext();
			curVelScaled = (Vector3) stream.ReceiveNext();
			isSprinting = (bool) stream.ReceiveNext();
		}
	}

	void FixedUpdate(){
		if (!isProxy){
			//ours
			curVelScaled = (movementManager.CurVel.normalized) / (movementManager.movementSpeed+.00000001f);
			isSprinting = movementManager.IsSprinting;
		} else {
			//proxy
			movementObj.transform.position = Vector3.Lerp(movementObj.transform.position, goalPosition + Vector3.up*0.145152f, .3f);	//adjust for the small difference between positions of the companion avatar and player hands
			movementObj.transform.rotation = Quaternion.Lerp(movementObj.transform.rotation, goalRotation, .3f);
			companionAnimManager.SetCurSpeedScaledFloat(curVelScaled.magnitude);
			companionAnimManager.SetCurVelScaledXFloat(curVelScaled.x);
			companionAnimManager.SetCurVelScaledZFloat(curVelScaled.z);
			companionAnimManager.SetSprintingBool(isSprinting);
		}
	}


	public void TriggerMeleeAnimation(){
		Debug.Log(this+"\t"+view);
		view.RPC("ProxyTriggerMelee",PhotonTargets.Others);
	}
	[RPC]
	void ProxyTriggerMelee(){
		companionAnimManager.TriggerMelee();
	}

	public void TriggerReloadAnimation(){
		view.RPC("ProxyTriggerReload",PhotonTargets.Others);
	}
	[RPC]
	void ProxyTriggerReload(){
		companionAnimManager.TriggerReload();
	}
}
