using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    public event Action<State> StateEvent;
    public event Action<bool> FrameEvent;
    public event Action<float> FlashEvent;
    public event Action<int> ItemEvent;
    public event Action<int> DoorEvent;
    public event Action<int> QEvent;
    public event Action<int> endQEvent;
    public event Action updateQEvent;
    public event Action<string> SayEvent;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void StartDialogueEvent(string blockName)
    {
        Debug.Log("StartDialogueEvent");
        SayEvent?.Invoke(blockName);
    }
    public void ChangeStateEvent(State state)
    {
        Debug.Log("ChangeStateEvent");
        StateEvent?.Invoke(state);
    }
    public void StartFrameEvent(bool frame_mode)
    {
        FrameEvent?.Invoke(frame_mode);
    }
    public void StartFlashEvent(float value)
    {
        FlashEvent?.Invoke(value);
    }
    public void StartItemEvent(int id)
    {
        ItemEvent?.Invoke(id);
    }
    public void StartDoorEvent(int id)
    {
        DoorEvent?.Invoke(id);
    }
    public void StartQEvent(int id)
    {
        Debug.Log("StartQEvent");
        QEvent?.Invoke(id);
    }
    public void EndQEvent(int id)
    {
        Debug.Log("EndQEvent");
        endQEvent?.Invoke(id);
        //updateQEvent?.Invoke();
    }
    public void UpdateQEvent()
    {
        Debug.Log("UpdateQEvent");
        updateQEvent?.Invoke();
    }
}
