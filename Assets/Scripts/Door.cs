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
}
