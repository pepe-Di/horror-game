using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Transform content;
    public List<Quest> quests = new List<Quest>();
    public List<GameObject> qs = new List<GameObject>();
    public Text qName;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        quests.Add(new Quest(questType.ToPoint, "Подойти к столу WASD", false));
        quests.Add(new Quest(questType.Grab, "Взять фонарь с помощью ЛКМ", true));
        quests.Add(new Quest(questType.Use, "Использовать фонарь на клавишу F", true, "flashlight"));
    }
    private void Start()
    {
        EventController.instance.QEvent += ChangeQName;
        EventController.instance.endQEvent += DeleteQName;
        EventController.instance.updateQEvent += UpdateContent;
    }
    public void ChangeQName(int id)
    {
        qName.text = quests[id].name;
    }
    public void DeleteQName(int id)
    {
        qName.text = "";
    }
    public bool TryToInvoke(int id)
    {
        if (!quests[id].isConsistent)
        {
            return true;
        }
        else if(Player.instance.lastQIndex == id - 1)
        {
            return true;
        }
        return false;
    }
    public void UpdateContent()
    {
        foreach(GameObject o in qs)
        {
            Destroy(o);
        }
        qs.Clear();
        foreach(Quest q in Player.instance.quests)
        {
            GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/UI/quest"));
            gm.GetComponentInChildren<Text>().text = q.name; 
            gm.transform.SetParent(content.transform);
            qs.Add(gm);
        }
    }
}
