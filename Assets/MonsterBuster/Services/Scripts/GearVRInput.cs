using System;
using UnityEngine;

public class GearVRInput : MonoBehaviour {

    public event Action OnTriggerDown;
    public event Action OnTriggerUp;
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;
    public event Action OnTouchPadTouchDown;
    public event Action OnTouchPadTouchUp;
    public event Action OnTouchPadButtonDown;
    public event Action OnTouchPadButtonUp;

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        // トリガー
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OnTriggerDown != null)
                OnTriggerDown();
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OnTriggerUp != null)
                OnTriggerUp();
        }

        // スワイプでButton.Up, Down, Left, Rightが反応するっぽい
        if (OVRInput.GetDown(OVRInput.Button.Up))
        {
            if (OnSwipeUp != null)
                OnSwipeUp();
        }

        if (OVRInput.GetDown(OVRInput.Button.Down))
        {
            if (OnSwipeDown != null)
                OnSwipeDown();
        }

        if (OVRInput.GetDown(OVRInput.Button.Left))
        {
            if (OnSwipeLeft != null)
                OnSwipeLeft();
        }

        if (OVRInput.GetDown(OVRInput.Button.Right))
        {
            if (OnSwipeRight != null)
                OnSwipeRight();
        }

        // タッチパッドのタッチ
        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
        {
            if (OnTouchPadTouchDown != null)
                OnTouchPadTouchDown();
        }

        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            if (OnTouchPadTouchUp != null)
                OnTouchPadTouchUp();
        }

        // タッチパッドのクリック
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchPadButtonDown != null)
                OnTouchPadButtonDown();
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchPadButtonUp != null)
                OnTouchPadButtonUp();
        }

    }

    private void OnDestroy()
    {
        OnTriggerDown = null;
        OnTriggerUp = null;
        OnSwipeUp = null;
        OnSwipeDown = null;
        OnSwipeLeft = null;
        OnSwipeRight = null;
        OnTouchPadTouchDown = null;
        OnTouchPadTouchUp = null;
        OnTouchPadButtonDown = null;
        OnTouchPadButtonUp = null;
    }
}
