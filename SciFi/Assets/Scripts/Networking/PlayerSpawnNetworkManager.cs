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
		SpawnLocalPlayer(spawnTransform.position, spawnTransform.rotation, id);
		view.RPC("SpawnPlayerProxy", PhotonTargets.OthersBuffered, spawnTransform.position, spawnTransform.rotation, id);
	}
	
	void SpawnLocalPlayer(Vector3 spawnPosition, Quaternion spawnRotation, int id){
		Debug.Log ("spawning self "+PhotonNetwork.player+" at: "+spawnPosition);
		GameObject HUD = (GameObject) Instantiate(HUDObj);
		HUD.GetComponent<PhotonView>().viewID = PhotonNetwork.AllocateViewID();

		GameObject newPlayer = (GameObject) Instantiate(playerObj, spawnPosition, spawnRotation);
		newPlayer.GetComponent<PhotonView>().viewID = id;

		HUDManager playerHUDManager = newPlayer.GetComponentInChildren<HUDManager>();
		playerHUDManager.ammoCounterObj = HUD.GetComponentInChildren<AmmoCountManager>().gameObject;
		playerHUDManager.SMGCrosshairsObj = HUD.GetComponentInChildren<SMGCrosshairs>().gameObject;
		playerHUDManager.rifleCrosshairsObj = HUD.GetComponentInChildren<RifleCrosshairs>().gameObject;

		newPlayer.GetComponent<PlayerNetworkManager>().SetUpLocalPlayer();
	}
	
	[RPC]
	void SpawnPlayerProxy(Vector3 spawnPosition, Quaternion spawnRotation, int id){
		Debug.Log ("spawning proxy at: "+spawnPosition);
		GameObject newPlayer = (GameObject) Instantiate(playerObj, spawnPosition, spawnRotation);
		newPlayer.GetComponent<PhotonView>().viewID = id;

		newPlayer.GetComponent<PlayerNetworkManager>().SetUpProxyCompanion();
	}
}
