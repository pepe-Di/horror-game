using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Typer : MonoBehaviour
{
    public List<Type> txt = new List<Type>();
    public float speed = 0.1f;
    public char[] lts;
    public int currentIndex = 0;
    public TxtState state; 
    public void ChangeState(TxtState state)
    {
        if (this.state != state)
        {
            this.state = state;
        }
    }
    void Awake()
    {
        state = TxtState.Idle;
        var obj = GameObject.FindGameObjectsWithTag("Type").OrderByDescending(go => go.name).ToArray();
        int i = 1;
        foreach(GameObject o in obj)
        {
            Type type = o.GetComponent<Type>();
            txt.Add(type);
        }
        txt.Reverse();
    }
    public void Type()
    {
        if (currentIndex >= txt.Count) state = TxtState.End;
        else StartCoroutine(Sentence());
    }
    IEnumerator Sentence()
    {
        state = TxtState.Typing;
        lts = txt[currentIndex].lts;
        foreach (char c in txt[currentIndex].lts)
        {
            txt[currentIndex].text.text += c;
            yield return new WaitForSeconds(speed);
        }
        state = TxtState.Idle;
        currentIndex++;
    }
}
public enum TxtState
{
    Typing,
    Idle,
    End
}