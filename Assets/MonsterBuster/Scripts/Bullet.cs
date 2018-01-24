using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        PhotonView myView = gameObject.GetComponent<PhotonView>();
        if (other.gameObject.tag == "Player")
        {
            PhotonView otherView = other.gameObject.GetComponent<PhotonView>();
            if (otherView.ownerId == myView.ownerId)
            {
                return;
            }
            if (myView.ownerId == PhotonNetwork.player.ID)
            {
                otherView.RPC("Destroy", PhotonPlayer.Find(otherView.ownerId));
            }
        }
        if (other.gameObject.tag == "Finish")
        {
        }
        if (myView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
