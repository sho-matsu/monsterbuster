using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TriggerAttack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<PhotonView>();
        if (target != null)
        {
            target.RPC("Attack", PhotonTargets.All);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // do nothing
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // モンスターが閾値に到達したことをフックするGameObjectなのでタッチイベントは処理しない

        var objectsHit = new List<RaycastResult>();
        // タッチした座標にいるすべてのGameObjectを取得
        EventSystem.current.RaycastAll(eventData, objectsHit);
        foreach (var objectHit in objectsHit)
        {
            // タッチイベントを処理できるGameObjectかどうか判定。ただし自身の場合は処理しない
            if (ExecuteEvents.CanHandleEvent<IPointerDownHandler>(objectHit.gameObject)
                && objectHit.gameObject != gameObject)
            {
                // 伝搬用の新しいイベントを生成
                PointerEventData newEventData = new PointerEventData(EventSystem.current)
                {
                    pointerCurrentRaycast = objectHit
                };
                // イベントのポスト
                ExecuteEvents.Execute<IPointerDownHandler>(objectHit.gameObject, newEventData, (handler, data) => handler.OnPointerDown((PointerEventData)data));
                break;
            }
        }
    }
}
