using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using MonsterBuster.Services;

public class NetworkMonster : Photon.PunBehaviour, IPointerDownHandler, IPointerUpHandler
{
    int hitCounter;
    [SerializeField]
    int damageLimit;
    GameObject target;
    public static bool enableAttack;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (damageLimit == 0)
        {
            damageLimit = 5;
        }
        enableAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject)
        {
            Debug.Log("monster hit");
            hitCounter++;
            if (hitCounter > damageLimit)
            {
                ScoreManager.ScoreUp();
                if (photonView.isMine)
                {
                    // 自身が生成したインスタンスの場合、権限があるのでGameObjectを破棄
                    Dead(photonView.gameObject);
                }
                else
                {
                    // 自身のではない場合、権限を要求
                    photonView.RequestOwnership();
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

        if (view.isMine)
        {
            Dead(view.gameObject);
        }
    }

    [PunRPC]
    public void Attack()
    {
        // 攻撃アニメーションを取得し、GameObjectに設定
        // 攻撃し、パーティクルでエフェクトを表示したあと、GameObjectを破棄
        StartCoroutine(RunAttack());
    }

    private IEnumerator RunAttack()
    {
        if (enableAttack)
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
            // ステートの反映に1フレームいる
            yield return null;

            if (photonView.isMine)
            {
                // 攻撃時のエフェクト
                GameObject fireBall = PhotonNetwork.Instantiate("FireBall", transform.position + new Vector3(0.0f, 2.0f, 0.0f) + (transform.forward * 0.5f), transform.rotation, 0);
                fireBall.transform.LookAt(target.transform);
                var ballRb = fireBall.GetComponent<Rigidbody>();
                ballRb.velocity = fireBall.transform.forward * 15;
            }

            // アニメーションが終わるまで待つ
            yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        }
        // モンスター破棄
        if (photonView.isMine)
        {
            Dead(photonView.gameObject);
        }
    }

    private void Dead(GameObject monster)
    {
        monster.SetActive(false);
        PhotonNetwork.Destroy(monster);
    }
}