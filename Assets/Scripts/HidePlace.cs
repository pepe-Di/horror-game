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
        if(other.tag=="Player") Debug.Log("Player enters");
    }
    void OnTriggerStay(Collider other){
        if(other.tag=="Player") {}
    }
    void OnTriggerExit(Collider other){
        if(other.tag=="Player") {}
    }
}
