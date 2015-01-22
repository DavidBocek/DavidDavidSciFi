using UnityEngine;
using System.Collections;
using Photon;

public class PlayerSpawnNetworkManager : Photon.MonoBehaviour {

	public GameObject playerObj;
	public GameObject HUDObj;

	public Transform[] spawnTransforms;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnNewPlayer(PhotonView view){
		Transform spawnTransform = spawnTransforms[Random.Range(0,spawnTransforms.Length)];

		int id = PhotonNetwork.AllocateViewID();
		SpawnLocalPlayer(spawnTransform.position, spawnTransform.rotation);
		view.RPC("SpawnPlayerProxy", PhotonTargets.OthersBuffered, PhotonNetwork.player, spawnTransform.position, spawnTransform.rotation, id);
	}
	
	void SpawnLocalPlayer(Vector3 spawnPosition, Quaternion spawnRotation){
		Debug.Log ("spawning self "+PhotonNetwork.player+" at: "+spawnPosition);
		GameObject HUD = (GameObject) Instantiate(HUDObj);
		foreach (PhotonView view in HUD.GetComponentsInChildren<PhotonView>()){
			view.viewID = PhotonNetwork.AllocateViewID();
		}

		GameObject newPlayer = (GameObject) Instantiate(playerObj, spawnPosition, spawnRotation);
		foreach (PhotonView view in newPlayer.GetComponentsInChildren<PhotonView>()){
			view.viewID = PhotonNetwork.AllocateViewID();
		}

		HUDManager playerHUDManager = newPlayer.GetComponentInChildren<HUDManager>();
		playerHUDManager.ammoCounterObj = HUD.GetComponentInChildren<AmmoCountManager>().gameObject;
		playerHUDManager.SMGCrosshairsObj = HUD.GetComponentInChildren<SMGCrosshairs>().gameObject;
		playerHUDManager.rifleCrosshairsObj = HUD.GetComponentInChildren<RifleCrosshairs>().gameObject;

		newPlayer.GetComponent<PlayerNetworkManager>().SetUpLocalPlayer();
	}
	
	[RPC]
	void SpawnPlayerProxy(PhotonPlayer player, Vector3 spawnPosition, Quaternion spawnRotation, int id){
		Debug.Log ("spawning proxy for " +player+" at: "+spawnPosition);
		GameObject newPlayer = (GameObject) Instantiate(playerObj, spawnPosition, spawnRotation);
		foreach (PhotonView view in newPlayer.GetComponentsInChildren<PhotonView>()){
			view.viewID = id;
		}
		newPlayer.GetComponent<PlayerNetworkManager>().SetUpProxyCompanion();
	}
}
