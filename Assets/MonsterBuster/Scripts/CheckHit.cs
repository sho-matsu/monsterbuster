using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckHit : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private int hitCounter;
    private int limit = 5;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            //check if target object was hit
            if (hit.transform.gameObject == gameObject)
            {
                Debug.Log("monster hit");
                hitCounter++;
                if (hitCounter > limit) {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }
}
