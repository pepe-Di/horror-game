using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerPuzzle : MonoBehaviour
{
    public List<string> sequence = new List<string>{"dark","dark","blue","blue","green","green","green","blue","blue","green"};
    public List<string> player_sequence = new List<string>();
    public Door locker_door; 
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>Player.instance.loaded);
        Item i = GameManager.instance.player_.FindItem("key3");
        if(i!=null){
            locker_door.locked = false;
            }
            
    }
    public void AddToList(string value){
        if(player_sequence.Count>10) return;
        player_sequence.Add(value);
        if(player_sequence.Count==10){
            if(CheckTheAnswer()){
                locker_door.locked = false;
                EventController.instance.StartDialogueEvent("!");
                SoundManager.instance.PlaySe(Se.Click);
            }
            else
            {
                EventController.instance.StartDialogueEvent("idk");
                player_sequence.Clear();
            }
        }
    }
    bool CheckTheAnswer(){
        int i=0;
        foreach(string s in sequence){
            if(player_sequence[i]!=s) return false;
            i++;
        }
        return true;
    }
}
