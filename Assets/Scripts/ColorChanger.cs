using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
     public Image image;
 
     // Start is called before the first frame update
     void Start()
     {
         var objs =  this.gameObject.GetComponentsInChildren<Image>();
         foreach(Image i in objs)
         {
            image.color = Color.red;
         }
     }    
}
