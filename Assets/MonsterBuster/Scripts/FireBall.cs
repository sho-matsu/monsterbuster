using UnityEngine;

public class FireBall : Photon.PunBehaviour {
    Vector3 correctPos = Vector3.zero;
    Quaternion correctRot = Quaternion.identity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctRot, Time.deltaTime * 5);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctPos = (Vector3)stream.ReceiveNext();
            correctRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
