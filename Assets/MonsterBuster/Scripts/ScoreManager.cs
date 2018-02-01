using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private static int score;
    private static int norma = 50;

	// Use this for initialization
	void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
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
        GUI.Label(new Rect(10, 10, 100, 20), "50体倒せ！");
        GUI.Label(new Rect(10, 30, 100, 20), "撃破数:" + score);
        if (score >= norma)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 10, 100, 20), "クリア！", GUI.skin.customStyles[0]);
        }
    }
}
