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
    bool C_running = false;
   void Awake(){
       opened = false;
   }
   
    public IEnumerator Open() 
    {
        C_running = true;
        Quaternion newRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        int val = 1;
        //Door d = door.GetComponent<Door>();
        if (right_side) val = -1;
        if (opened) { 
            SoundManager.instance.PlaySe(Se.Close);
            //audioSource.PlayOneShot(Resources.Load("Sounds/door2") as AudioClip, volume); 
            newRotation *= Quaternion.Euler(0, -90*val, 0); 
            }
        else { 
            SoundManager.instance.PlaySe(Se.Open);
            //audioSource.PlayOneShot(Resources.Load("Sounds/door1") as AudioClip, volume); 
            newRotation *= Quaternion.Euler(0, 90*val, 0); 
        }
        opened = !opened;
        Debug.Log(opened);
        while (transform.rotation!=newRotation) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 20 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        C_running = false;
        //click = 0;
    }
}
