using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour {
    private float passedTime;
    private const float interval = 8f;

	// Use this for initialization
	void Start () {
        passedTime = -10f;
	}

    private void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            //Debug.Log("PhotonNetwork.inRoom:" + PhotonNetwork.inRoom);
            var now = Time.fixedTime;
            var diff = Mathf.Abs(now - passedTime);
            //Debug.Log("diff:" + diff);
            if (diff > interval)
            {
                if (PhotonNetwork.inRoom)
                {
                    passedTime = now;
                    // 生成位置をランダムな座標にする
                    float x = Random.Range(-10f, 10f);
                    float z = Random.Range(-10f, 10f);
                    Vector3 pos = new Vector3(x, 0f, z);

                    // 第1引数にResourcesフォルダの中にあるプレハブの名前(文字列)
                    // 第2引数にposition
                    // 第3引数にrotation
                    // 第4引数にView ID(指定しない場合は0)
                    GameObject target = GameObject.FindGameObjectWithTag("MainCamera");
                    Quaternion rotation = Quaternion.LookRotation(target.transform.position);
                    GameObject obj = PhotonNetwork.Instantiate("monster", pos, rotation, 0);
                }
            }
        }
    }
}
