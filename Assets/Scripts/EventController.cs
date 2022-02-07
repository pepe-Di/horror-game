using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    public event Action<float> FlashEvent;
    public event Action<int> ItemEvent;
    public event Action<int> DoorEvent;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
}
