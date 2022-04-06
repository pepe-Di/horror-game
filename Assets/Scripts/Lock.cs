using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public bool right_side;
    [SerializeField] public bool opened;
    [SerializeField] public bool locked=false;
    [SerializeField] public int index;
   void Awake(){
       opened = false;
   }
}
