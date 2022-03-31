using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    public event Action<bool> FrameEvent;
    public event Action<float> FlashEvent;
    public event Action<int> ItemEvent;
    public event Action<int> DoorEvent;
    public event Action<int> QEvent;
    public event Action<int> endQEvent;
    public event Action updateQEvent;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
        QEvent?.Invoke(id);
    }
    public void EndQEvent(int id)
    {
        endQEvent?.Invoke(id);
        //updateQEvent?.Invoke();
    }
    public void UpdateQEvent()
    {
        updateQEvent?.Invoke();
    }
}
