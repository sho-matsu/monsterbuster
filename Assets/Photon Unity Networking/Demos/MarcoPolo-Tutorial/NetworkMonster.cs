using UnityEngine;
using UnityEngine.EventSystems;

public class NetworkMonster : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    private int hitCounter;
    private int limit = 5;

    void Awake()
    {
        if (photonView.isMine)
        {
            GetComponent<ThirdPersonCamera>().enabled = true;
            GetComponent<MonsterFire>().enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
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
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}