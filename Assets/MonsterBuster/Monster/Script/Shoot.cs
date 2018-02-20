using UnityEngine;
using VRStandardAssets.Utils;

namespace MonsterBuster.Monster
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] VRInput vrInput;
        [SerializeField] GearVRInput gearVRInput;
        [SerializeField] Transform generateFrom;
        [SerializeField] Transform destination;

        void OnEnable()
        {
            gearVRInput.OnTriggerDown += ShootMonster;
            vrInput.OnClick += ShootMonster;
        }

        void ShootMonster()
        {
            if (PhotonNetwork.inRoom)
            {
                GameObject obj = PhotonNetwork.Instantiate("Bullet", generateFrom.position, Quaternion.identity, 0);
                var target = destination;
#if UNITY_EDITOR_OSX
                var pos = Input.mousePosition;
                pos.z = 10f;
                target.transform.position = Camera.main.ScreenToWorldPoint(pos);
#endif
                obj.transform.LookAt(target);
                obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * 15;
            }
        }
    }
}