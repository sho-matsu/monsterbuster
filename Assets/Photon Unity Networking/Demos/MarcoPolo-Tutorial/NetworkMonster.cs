using UnityEngine;
using UnityEngine.EventSystems;

public class NetworkMonster : Photon.PunBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    private int hitCounter;
    private int limit = 5;
    private GameObject target;

    void Awake()
    {
        //if (photonView.isMine)
        //{
        //    GetComponent<ThirdPersonCamera>().enabled = true;
        //    GetComponent<MonsterFire>().enabled = true;
        //}
        target = GameObject.FindGameObjectWithTag("MainCamera");
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
        } else {
            var trans = transform.forward * 10 * Time.deltaTime;
            transform.Translate(new Vector3(trans.x, 0f, trans.z));
        }
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
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            //check if target object was hit
            if (hit.transform.gameObject == gameObject)
            {
                Debug.Log("monster hit");
                hitCounter++;
                if (hitCounter > limit)
                {
                    if (photonView.isMine)
                    {
                        gameObject.SetActive(false);
                        PhotonNetwork.Destroy(gameObject);
                    } else {
                        photonView.RequestOwnership();
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnOwnershipRequest(object[] viewAndPlayer)
    {
        PhotonView view = viewAndPlayer[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

        Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");

        view.TransferOwnership(requestingPlayer.ID);
    }

    public override void OnOwnershipTransfered(object[] viewAndPlayers)
    {
        PhotonView view = viewAndPlayers[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayers[1] as PhotonPlayer;

        if (view.isMine) {
            view.gameObject.SetActive(false);
            PhotonNetwork.Destroy(view.gameObject);
        }
    }
}