using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<PhotonView>();
        if (target != null) {
            target.RPC("Attack", PhotonTargets.All);
        }
    }
}
