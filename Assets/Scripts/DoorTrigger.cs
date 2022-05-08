using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Door door;
    // Start is called before the first frame update
    void Awake()
    {
        door = GetComponentInParent<Door>();
    }
     void OnTriggerEnter(Collider other){
       if(other.tag=="Enemy"){
           if(!timer) StartCoroutine(Wait());
       }
   }
    // Update is called once per frame
     IEnumerator Wait(){
       yield return new WaitForSeconds(0.5f);
       timer = true;
   }
   bool timer = false;
   void OnTriggerStay(Collider other){
       if(other.tag=="Enemy"&&timer){
           if(!door.locked&&!door.opened){
               //open
                StartCoroutine(door.Open());
           } 
           else{
               other.GetComponent<EnemyController>().route=false;
           }
            timer = false;
       }
   }
}
