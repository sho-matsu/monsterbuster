using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionAttack : MonoBehaviour {
    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
	}
	
	// Update is called once per frame
	void Update () {
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            // todo モンスターへの攻撃
            device.TriggerHapticPulse(1000);
        }
	}
}
