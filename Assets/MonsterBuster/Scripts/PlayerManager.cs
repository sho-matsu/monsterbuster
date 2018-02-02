using UnityEngine;

public class PlayerManager : Photon.PunBehaviour
{
    public int lifeCount = 10;
    int damageCount;
    GameObject target;

    // Use this for initialization
    void Start()
    {
        damageCount = 0;
        target = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        var collisionObject = collision.gameObject;
        if (collisionObject.name.Contains("FireBall"))
        {
            damageCount++;
            Debug.Log("damage:" + damageCount);
            if (damageCount >= lifeCount)
            {
                // todo game over
            }

            var view = collisionObject.GetComponent<PhotonView>();
            if (view.isMine)
            {
                PhotonNetwork.Destroy(collisionObject);
            }
            var targetPos = target.transform.position;
            var pos = new Vector3(targetPos.x, 1.5f, targetPos.z - 1);
            GameObject fire = PhotonNetwork.Instantiate("Fire", pos, Quaternion.identity, 0);
            fire.SetActive(true);
        }
    }

    //[PunRPC]
    //public void ReceiveDamage()
    //{
    //    damageCount++;
    //    if (damageCount >= lifeCount)
    //    {
    //        // todo game over
    //    }
    //}

    void OnGUI()
    {
        // todo プレイヤーごとにLPを表示させる
        //GUI.Label(new Rect(Screen.width - 170, 20, 150, 60), "LP:" + (lifeCount - damageCount));
    }
}
