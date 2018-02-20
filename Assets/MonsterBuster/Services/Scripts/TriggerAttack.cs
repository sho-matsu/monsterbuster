using UnityEngine;

namespace MonsterBuster.Services
{
    public class TriggerAttack : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            var target = other.gameObject.GetComponent<PhotonView>();
            if (target != null && target.gameObject.name.Contains("Monster"))
            {
                target.RPC("Attack", PhotonTargets.All);
            }
        }
    }
}
