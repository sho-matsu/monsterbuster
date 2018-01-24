using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour {
    private float passedTime;
    private const float interval = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var now = Time.deltaTime;
        if (Mathf.Abs(now - passedTime) > interval)
        {
            if (PhotonNetwork.inRoom)
            {
                passedTime = now;
                // 生成位置をランダムな座標にする
                float x = Random.Range(-10f, 10f);
                float z = Random.Range(-10f, 10f);
                Vector3 pos = new Vector3(x, 0F, z);

                // 第1引数にResourcesフォルダの中にあるプレハブの名前(文字列)
                // 第2引数にposition
                // 第3引数にrotation
                // 第4引数にView ID(指定しない場合は0)
                GameObject obj = PhotonNetwork.Instantiate("monster", pos, Quaternion.identity, 0);
            }
        }
	}
}
