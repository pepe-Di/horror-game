using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omamori : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject coll;
    public GameObject door_trigger;
    IEnumerator Start()
    {
        door_trigger.SetActive(false);
        yield return new WaitUntil(()=>Player.instance.loaded);
        Item i = GameManager.instance.player_.FindItem("omamori");
        if(i!=null){
            coll.SetActive(false);
            door_trigger.SetActive(true);
            Destroy(this.gameObject);
            }
    }
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            Item i = other.GetComponent<Player>().FindItem("notepad");
            if(i==null){
            stay=true;
            EventController.instance.StartDialogueEvent("warn");
            StartCoroutine(Curse());}
            else{
                coll.SetActive(false);
                door_trigger.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
    public bool stay=false;
    //void OnTriggerStay(Collider other){
   //     if(other.CompareTag("Player")){
    //    }
    //}
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
        stay=false;
        }
    }
    public IEnumerator Curse()
    {
        while(stay){
            yield return new WaitForSeconds(1f);
            EventController.instance.StartHPchange(-1,1);
        }
    }
}
