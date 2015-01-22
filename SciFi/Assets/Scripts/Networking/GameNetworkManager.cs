using UnityEngine;
using System.Collections;

public class GameNetworkManager : Photon.MonoBehaviour {

	//scene view
	private PhotonView photonView;
	public PhotonView ScenePhotonView {get{return photonView;}}

	// Use this for initialization
	void Start () {
		photonView = PhotonView.Get(this);

		PhotonNetwork.ConnectUsingSettings("v0.1");
	}

	void Update(){
		//debugging network shortcuts and stuff
		if (Input.GetKeyDown(KeyCode.L)){
			PhotonNetwork.Disconnect();
		}
		if (Input.GetKeyDown(KeyCode.P)){
			PhotonNetwork.ConnectUsingSettings("v0.1");
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	void OnJoinedLobby(){
		PhotonNetwork.JoinOrCreateRoom("room1", new RoomOptions(), new TypedLobby());
	}

	void OnJoinedRoom(){
		GetComponent<PlayerSpawnNetworkManager>().SpawnNewPlayer(photonView);
	}

	/*void OnGUI(){
		GUI.TextArea(new Rect(10,500,500,100),"players connected: "+PhotonNetwork.playerList.Length);
		for (int i=0; i<PhotonNetwork.playerList.Length; i++){
			GUI.TextArea(new Rect(20,500+25*(i+1),480,20), PhotonNetwork.playerList[i].ToString());
		}

		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}*/
}
