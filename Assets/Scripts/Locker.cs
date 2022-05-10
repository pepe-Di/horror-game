using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public LockerPuzzle puzzle;
   void OnMouseDown(){
       if(!c){
        Debug.Log(this.gameObject.name);
        //EventController.instance.StartDialogueEvent("look2");
        puzzle.AddToList(this.gameObject.name);
        StartCoroutine(Wait());
        SoundManager.instance.PlaySe(Se.Item);
       }
   }
   bool c=false;
   IEnumerator Wait(){
       c=true;
       yield return new WaitForSeconds(0.2f);
       c=false;
   }
}
