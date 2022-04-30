using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool right_side;
    [SerializeField] public bool opened;
    [SerializeField] public bool locked=false;
    [SerializeField] public int index;
    [SerializeField] public int Qindex;
   void Awake(){
       opened = false;
   }
   void OnTriggerEnter(Collider other){
       if(other.tag=="Enemy"){
           if(!timer) StartCoroutine(Wait());
       }
   }
   IEnumerator Wait(){
       yield return new WaitForSeconds(0.5f);
       timer = true;
   }
   bool timer = false;
   void OnTriggerStay(Collider other){
       if(other.tag=="Enemy"&&timer){
           if(!locked){
               //open
           } 
           else{
               other.GetComponent<EnemyController>().route=false;
           }
            timer = false;
       }
   }
}
