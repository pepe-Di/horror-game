using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public int id;
    public GameObject finish;
    bool loaded=false;
    public bool timer=false;
    public float secs=2f;

    IEnumerator Start()
    {
        yield return new WaitUntil(()=>GameManager.instance.loaded); 
        loaded = true;
        Debug.Log(id+" qtrigger loaded");
        try
        {
            Debug.Log("hui");
            if (finish != null)
            {
                if (finish.CompareTag("Point"))
                {
                    finish.GetComponent<FinishTrigger>().id = id;
                    finish.SetActive(false);
                }
                else if (finish.CompareTag("Item"))
                {
                    if (QuestManager.instance.quests[id].Type == questType.Grab)
                    {
                        finish.AddComponent<QuestItem>();
                        finish.GetComponent<QuestItem>().id = id;
                    }
                    else //use
                    {
                        Player.instance.FindItem(finish.name).questId = id;
                    }
                }
              /*  else if (finish.CompareTag("Puzzle"))
                {
                }*/
                else {//item_pos
                    ItemPos ip = finish.GetComponent<ItemPos>();
                    Debug.Log("it pos");
                    GameObject item = finish.gameObject.GetComponentInChildren<Rigidbody>().gameObject;
                    if (QuestManager.instance.quests[id].Type == questType.Grab)
                    {
                        item.AddComponent<QuestItem>();
                        item.GetComponent<QuestItem>().id = id;
                    }
                    else //use
                    {
                        Player.instance.FindItem(item.name).questId = id;
                    }
                }
                    QuestManager.instance.quests[id].finish = finish;
            }
        }
        catch{}
    }
    bool c=false;
    IEnumerator Timer()
    {
        Debug.Log("start timer");
        c=true;
        timer=false;
        yield return new WaitForSeconds(secs);
        Debug.Log(id + " startq");
        EventController.instance.StartQEvent(id);
    //  EventController.instance.UpdateQEvent();
        if (finish != null) finish.SetActive(true);
        Destroy(this.gameObject);
        c=false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!loaded) return;
            if(timer) {
                StartCoroutine(Timer());
                return;
            }
            if(c)return;
            if (QuestManager.instance.TryToInvoke(id))
            {
                Debug.Log(id + " startq");
                EventController.instance.StartQEvent(id);
              //  EventController.instance.UpdateQEvent();
                if (finish != null) finish.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player"))
        {

        }
    }
}
