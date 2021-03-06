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
    public Text qName, bgName;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        quests.Add(new Quest(questType.ToPoint, "move to the desk", false));
        quests.Add(new Quest(questType.Grab, "grab the flashlight", true, "flashlight"));
        quests.Add(new Quest(questType.Use, "use the flashlight", true, "flashlight"));
        quests.Add(new Quest(questType.ToPoint, "exit the room", false));
        quests.Add(new Quest(questType.Grab, "find a key", false, "key0"));
        quests.Add(new Quest(questType.ToPoint, "exit the school", false));
        quests.Add(new Quest(questType.Hide, "Hide somewhere", false));
        quests.Add(new Quest(questType.ToPoint, "Find security room", false));
        int i=0;
        foreach(Quest q in quests){
            q.id = i;
            i++;
        }
    }
    private void Start()
    {   
        qName.text = "";
        bgName.text="";
        EventController.instance.QEvent += ChangeQName;
        EventController.instance.endQEvent += DeleteQName;
        EventController.instance.updateQEvent += UpdateContent;
        EventController.instance.ShowLast+= ShowLast;
    }
    public void ChangeColor()
    {
        
    }
    public void ChangeQName(int id)
    {
        //bgImg.SetActive(true);
        string s = LocalisationSystem.TryGetLocalisedValue("quest"+id);
        qName.text = s;
        bgName.text = s;
       // qName.text = quests[id].name;
    }
    public void DeleteQName(int id)
    {
        qName.text = "";
        bgName.text = "";
        //bgImg.SetActive(false);
    }
    public bool TryToInvoke(int id)
    {
        //if(id==-1) return false;
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
        int id=0;
        foreach(Quest q in Player.instance.quests)
        {
            GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/UI/quest"));
            gm.GetComponentInChildren<Text>().text = LocalisationSystem.TryGetLocalisedValue("quest"+q.id); 
            Debug.Log("Q!"+LocalisationSystem.TryGetLocalisedValue("quest"+q.id));
            gm.transform.SetParent(content.transform);
            qs.Add(gm);
            id++;
        }
    }
    public void ShowLast(int id){
       ChangeQName(id);
    }
    void OnEnable(){
      //  Debug.Log("OnEnable!!!!!!!!!!!!!");

    }
    void OnDisable(){
        try{
        EventController.instance.ShowLast-= ShowLast;
        EventController.instance.QEvent-= ChangeQName;
        EventController.instance.endQEvent -= DeleteQName;
        EventController.instance.updateQEvent -= UpdateContent;

        }
        catch{

        }
    }
}
