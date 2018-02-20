using UnityEngine;

namespace MonsterBuster.Services
{
    public class NetworkManager : Photon.PunBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // モンスター同士の衝突を無効化する
            int layer = LayerMask.NameToLayer("Monster");
            Physics.IgnoreLayerCollision(layer, layer);

            PhotonNetwork.ConnectUsingSettings("0.1");
        }

        void OnDestroy()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            // todo エラーダイアログ
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            PhotonNetwork.Instantiate("BusterPlayer", new Vector3(0, 2, 15F), Quaternion.identity, 0);
        }

        public void OnPhotonRandomJoinFailed()
        {
            Debug.Log("OnPhotonRandomJoinFailed");
            PhotonNetwork.CreateRoom(null);
        }
    }
}
