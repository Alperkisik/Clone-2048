using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
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

    private void Listen_TouchInputs()
    {

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
