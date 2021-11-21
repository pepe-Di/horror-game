using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
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
    Walk
}