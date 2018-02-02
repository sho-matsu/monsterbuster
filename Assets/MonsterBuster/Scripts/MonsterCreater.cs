using UnityEngine;

public class MonsterCreater : MonoBehaviour {
    private float passedTime;
    public float interval;
    private int emergenceCount;
    public static bool enableCreate;

	// Use this for initialization
	void Start () {
        passedTime = -10f;
        if (Equals(interval, 0))
        {
            interval = 5;
        }
        emergenceCount = 0;
        enableCreate = true;
	}

    private void FixedUpdate()
    {
        if (!enableCreate) return;

        if (PhotonNetwork.isMasterClient)
        {
            if (emergenceCount > 20)
            {
                interval = interval / 2;
            }
            var now = Time.fixedTime;
            var diff = Mathf.Abs(now - passedTime);
            if (diff > interval)
            {
                if (PhotonNetwork.inRoom)
                {
                    passedTime = now;
                    // 生成位置をランダムな座標にする
                    float z = Random.Range(0f, 7f);
                    float xRange = 14f - z;
                    float x = Random.Range(-xRange, xRange);
                    Vector3 pos = new Vector3(x, 1f, z);

                    // 第1引数にResourcesフォルダの中にあるプレハブの名前(文字列)
                    // 第2引数にposition
                    // 第3引数にrotation
                    // 第4引数にView ID(指定しない場合は0)
                    GameObject target = GameObject.FindGameObjectWithTag("MainCamera");
                    // モンスターのインスタンス生成
                    GameObject obj = PhotonNetwork.Instantiate("Monster", pos, Quaternion.identity, 0);
                    // ターゲットに向かってくるよう調整
                    obj.transform.LookAt(target.transform);
                    obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * 4;
                    // 移動アニメーション
                    var anim = obj.GetComponent<Animator>();
                    anim.Play("Run", 0, 0.0f);
                    emergenceCount++;
                }
            }
        }
    }
}
