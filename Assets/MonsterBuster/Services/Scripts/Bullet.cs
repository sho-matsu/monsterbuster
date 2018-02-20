using UnityEngine;

namespace MonsterBuster.Services
{
    public class Bullet : Photon.PunBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // Something Receive Data
            }
            else
            {
                // Something Send Data
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Bullet Hit");
            PhotonView myView = gameObject.GetComponent<PhotonView>();
            if (other.gameObject.name.Contains("Monster"))
            {
                Debug.Log("Bullet hit monster");
                PhotonView otherView = other.gameObject.GetComponent<PhotonView>();
                otherView.RPC("Damage", PhotonTargets.All, otherView.viewID);
            }
            if (myView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                photonView.RequestOwnership();
            }
        }

        public override void OnOwnershipRequest(object[] viewAndPlayer)
        {
            // 権限の要求があった場合、要求してきたプレイヤーに移譲する
            PhotonView view = viewAndPlayer[0] as PhotonView;
            PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

            Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");

            view.TransferOwnership(requestingPlayer.ID);
        }

        public override void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            // 権限が移譲されたらGameObjectを破棄する
            PhotonView view = viewAndPlayers[0] as PhotonView;
            PhotonPlayer requestingPlayer = viewAndPlayers[1] as PhotonPlayer;

            if (view.isMine)
            {
                PhotonNetwork.Destroy(view.gameObject);
            }
        }
    }
}
