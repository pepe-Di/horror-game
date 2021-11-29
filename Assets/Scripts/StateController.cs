using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable/StateController", fileName ="New StateController")]
public class StateController : ScriptableObject
{
    public State state;
    public void ChangeState(State state)
    {
        if (this.state != state)
        {
            this.state = state;
        }
    }
}
public enum State
{
    Idle,
    Pause,
    Talk,
    Walk,
    Sprint,
    Crouch,
    Jump,
    Sit
}