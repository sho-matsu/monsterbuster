using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

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
                    if (photonView.isMine)
                    {
                        // 自身が生成したインスタンスの場合、GameObjectを破棄
                        Dead(photonView.gameObject);
                    } else {
                        // 自身のではない場合、権限を要求
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

        if (view.isMine) {
            Dead(view.gameObject);
        }
    }

    [PunRPC]
    public void Attack()
    {
        // 攻撃アニメーションを取得し、GameObjectに設定
        // 攻撃し、パーティクルでエフェクトを表示したあと、GameObjectを破棄
        Debug.Log("RPC Attack");
        if (photonView.isMine)
        {
            StartCoroutine(RunAttack());
        }
    }

    private IEnumerator RunAttack()
    {
        var monster = photonView.gameObject;
        var rb = monster.GetComponent<Rigidbody>();
        // モンスターにかかっている物理力をリセット
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor();

        // 攻撃アニメーション
        var anim = monster.GetComponent<Animator>();
        anim.Play("Attack");

        // todo
        //yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitForSeconds(2);

        // モンスター破棄
        Dead(photonView.gameObject);
    }

    private void Dead(GameObject monster) {
        monster.SetActive(false);
        PhotonNetwork.Destroy(monster);
    }
}