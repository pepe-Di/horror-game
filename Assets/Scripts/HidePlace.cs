using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

void OnTriggerEnter(Collider other){
        if(other.tag=="Player") 
        {
            Debug.Log("Player enters");
            int id = GameManager.instance.player_.HideQ();
            if (id>=0)
            {
                EventController.instance.EndQEvent(id);
            }
        }
    }
    void OnTriggerStay(Collider other){
        if(other.tag=="Player") {}
    }
    void OnTriggerExit(Collider other){
        if(other.tag=="Player") {}
    }
}
