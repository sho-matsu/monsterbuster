using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown (KeyCode.H))
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(0.0f, 1.0f, 0.0f) + (transform.forward * 0.5f), transform.rotation, 0);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15.0f;
        }
	}
}
