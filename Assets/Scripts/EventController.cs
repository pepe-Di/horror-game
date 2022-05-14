using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public event Action<AIdata> LoadEnemy;
    public static EventController instance;
    public event Action<State> StateEvent;
    public event Action<bool> FrameEvent;
    public event Action<float> FlashEvent;
    public event Action<float, float> HPChange;
    public event Action<int> ItemEvent;
    public event Action<int> DoorEvent;
    public event Action<int> QEvent;
    public event Action<int> endQEvent;
    public event Action updateQEvent;
    public event Action<string> SayEvent;
    public event Action<bool> CameraEvent;
    public event Action<bool> FreezeCamera;
    public event Action OffComputerUI;
    public event Action OffPuzzleUI;
    public event Action GameOver;
    public event Action BlackOut;
    public event Action <int> ShowLast;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

       // DontDestroyOnLoad(gameObject);
    }
    public void StartBlackOut(){
        Debug.Log("StartBlackOut");
        BlackOut?.Invoke();
    }
    public void GameOverEvent(){
        Debug.Log("GameOverEvent");
        GameOver?.Invoke();
    }
    public void ShowLastEvent(int id){
        Debug.Log("ShowLastEvent");
        ShowLast?.Invoke(id);
    }
    public void StartEnemyLoad(AIdata data)
    {
        Debug.Log("StartFreezeCamera");
        LoadEnemy?.Invoke(data);
    }
    public void StartFreezeCamera(bool b)
    {
        Debug.Log("StartFreezeCamera");
        FreezeCamera?.Invoke(b);
    }
    public void StartHPchange(float value,float speed)
    {
        Debug.Log("StartHPchange");
        HPChange?.Invoke(value,speed);
    }
    public void OffComputerUIEvent(){
        Debug.Log("OffComputerUIEvent");
        OffComputerUI?.Invoke();
    }
    public void OffPuzzleUIEvent(){
        Debug.Log("OffPuzzleUIEvent");
        OffPuzzleUI?.Invoke();
    }
    public void StartCameraEvent(bool b)
    {
        Debug.Log("StartCameraEvent");
        CameraEvent?.Invoke(b);
    }
    public void StartDialogueEvent(string blockName)
    {
        Debug.Log("StartDialogueEvent");
        SayEvent?.Invoke(blockName);
    }
    public void ChangeStateEvent(State state)
    {
       // Debug.Log("ChangeStateEvent");
        StateEvent?.Invoke(state);
    }
    public void StartFrameEvent(bool frame_mode)
    {
        FrameEvent?.Invoke(frame_mode);
    }
    public void StartFlashEvent(float value)
    {
       if(GameManager.instance.player_!=null) FlashEvent?.Invoke(value);
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
        if(id==-1)return;
        Debug.Log("StartQEvent "+id);
        QEvent?.Invoke(id);
        updateQEvent?.Invoke();
    }
    public void EndQEvent(int id)
    {
        Debug.Log("EndQEvent "+id);
        endQEvent?.Invoke(id);
        updateQEvent?.Invoke();
    }
    public void UpdateQEvent()
    {
        Debug.Log("UpdateQEvent");
        updateQEvent?.Invoke();
    }
}
