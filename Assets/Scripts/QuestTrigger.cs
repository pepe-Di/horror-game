using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public int id;
    public GameObject finish;
    private void Start()
    {
        try
        {
            if (finish != null)
            {
                if (finish.CompareTag("Point"))
                {
                    finish.SetActive(false);
                    finish.GetComponent<FinishTrigger>().id = id;
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
                else if (finish.CompareTag("Puzzle"))
                {
                }
                    QuestManager.instance.quests[id].finish = finish;
            }
        }
        catch{}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("enter");
            if (QuestManager.instance.TryToInvoke(id))
            {
                EventController.instance.StartQEvent(id);
                EventController.instance.UpdateQEvent();
                if (finish != null) finish.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
