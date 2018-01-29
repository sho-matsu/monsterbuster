using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom() {
        Debug.Log("OnJoinedRoom");
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom(null);
    }
}
