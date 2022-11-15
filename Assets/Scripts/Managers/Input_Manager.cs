using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public static Input_Manager instance;

    [SerializeField] bool keyboardInputs = true;

    public event EventHandler OnUpInputReceived;
    public event EventHandler OnDownInputReceived;
    public event EventHandler OnLeftInputReceived;
    public event EventHandler OnRightInputReceived;

    private void Awake()
    {
        instance = this;
    }

    string m_DeviceType;
    private void Start()
    {
        dragDistance = Screen.height * 5 / 100;

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            m_DeviceType = "Desktop";
            keyboardInputs = true;
            Debug.Log("Device type : " + m_DeviceType);
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            m_DeviceType = "Handheld";
            keyboardInputs = false;
            Debug.Log("Device type : " + m_DeviceType);
        }
    }

    void Update()
    {
        if (!Game_Manager.instance.IslevelStarted || Game_Manager.instance.IslevelFinished) return;

        if (keyboardInputs) Listen_KeyboardInputs();
        else Listen_TouchInputs();
    }

    private void Listen_KeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.D)) Trigger_Event_RightInputReceived();
        else if (Input.GetKeyDown(KeyCode.A)) Trigger_Event_LeftInputReceived();
        else if (Input.GetKeyDown(KeyCode.W)) Trigger_Event_UpInputReceived();
        else if (Input.GetKeyDown(KeyCode.S)) Trigger_Event_DownInputReceived();
    }

    private Vector3 firstTouchPosition;
    private Vector3 lastTouchPosition;
    private float dragDistance;

    private void Listen_TouchInputs()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = touch.position;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastTouchPosition = touch.position;

                if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > dragDistance || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y))
                    {   
                        if ((lastTouchPosition.x > firstTouchPosition.x))
                        {
                            Trigger_Event_RightInputReceived();
                            //Debug.Log("Right Swipe");
                        }
                        else
                        {
                            Trigger_Event_LeftInputReceived();
                            //Debug.Log("Left Swipe");
                        }
                    }
                    else
                    {
                        if (lastTouchPosition.y > firstTouchPosition.y)
                        {
                            Trigger_Event_UpInputReceived();
                            //Debug.Log("Up Swipe");
                        }
                        else
                        {
                            Trigger_Event_DownInputReceived();
                            //Debug.Log("Down Swipe");
                        }
                    }
                }
            }
        }
    }

    private void Trigger_Event_UpInputReceived()
    {
        //Debug.Log("Up direction input Received");
        OnUpInputReceived?.Invoke(this, EventArgs.Empty);
    }

    private void Trigger_Event_DownInputReceived()
    {
        //Debug.Log("Down direction input Received");
        OnDownInputReceived?.Invoke(this, EventArgs.Empty);
    }

    private void Trigger_Event_LeftInputReceived()
    {
        //Debug.Log("Left direction input Received");
        OnLeftInputReceived?.Invoke(this, EventArgs.Empty);
    }

    private void Trigger_Event_RightInputReceived()
    {
        //Debug.Log("Right direction input Received");
        OnRightInputReceived?.Invoke(this, EventArgs.Empty);
    }
}
