using UnityEngine;
using System.Collections;

namespace MonsterBuster.Fire
{
    public class BurnFire : Photon.PunBehaviour
    {
        Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
        Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this

        void Update()
        {
            if (!photonView.isMine)
            {
                transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
            }
        }

        void OnEnable()
        {
            if (photonView.isMine)
            {
                StartCoroutine(DoAction());
            }
        }

        IEnumerator DoAction()
        {
            var particle = GetComponent<ParticleSystem>();
            particle.Play();
            yield return new WaitWhile(() => particle.IsAlive(true));
            PhotonNetwork.Destroy(gameObject);
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                // Network player, receive data
                correctPlayerPos = (Vector3)stream.ReceiveNext();
                correctPlayerRot = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
