using UnityEngine;

public class ScoreManager : Photon.PunBehaviour {
    private static int score;
    private static int norma = 50;

	// Use this for initialization
	void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (int)stream.ReceiveNext();
        }
    }

    public static void ScoreUp() {
        score++;
        if (score >= norma)
        {
            MonsterCreater.enableCreate = false;
            NetworkMonster.enableAttack = false;
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(20, 20, 240, 60), "50体倒せ！");
        GUI.Label(new Rect(20, 100, 240, 60), "撃破数:" + score);
        if (score >= norma)
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(Screen.width / 2 - 105, Screen.height / 2 - 30, 100, 60), "クリア！", GUI.skin.customStyles[0]);
        }
    }
}
