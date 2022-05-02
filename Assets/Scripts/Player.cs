using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int seed;
    public bool loaded=false;
    public int lastQIndex=-1;
    public static Player instance;
    public string name_;
    public float hp, stamina, max_stamina=5000f,max_hp=10000f, energy, full_energy=15000f;
    public State state; 
    public List<Item> items=new List<Item>();
    public List<Item> used_items=new List<Item>();
    public List<Quest> quests = new List<Quest>();
    public List<Quest> finished_quests = new List<Quest>();
    public GameObject selectedItem;
    private int selectedID;
    public float speed_modifier=0;
    public event OnHpChange onHpChange;
    public delegate void OnHpChange(float value);
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //EventController.instance.FlashEvent += EnergyChange;
        EventController.instance.QEvent += AddQ;
        EventController.instance.endQEvent += EndQuest;
    }
    public int HideQ()
    {
        foreach(Quest q in quests)
        {
            if(q.Type == questType.Hide) return q.id;
        }
        return -1;
    }
    public void AddQ(int id)
    {
        Quest q = QuestManager.instance.quests[id];
        if (q.Type==questType.Use)
        {
            Item i = FindItem(q.finish.name);
            i.questId = id;
            Debug.Log("i.questId "+i.questId);
        }
        quests.Add(q);
    }
    public Item FindItem(string name)
    {
        foreach (Item i in items)
        {
            if (i.GetGmName() == name) return i;
        }
        return null;
    }
    public Item FindUsedItem(string name)
    {
        foreach (Item i in used_items)
        {
            if (i.GetGmName() == name) return i;
        }
        return null;
    }
    public void EndQuest(int id)
    {
        try
        {
            lastQIndex = id;
            quests.Remove(QuestManager.instance.quests[id]);
            finished_quests.Add(QuestManager.instance.quests[id]);
            if (QuestManager.instance.quests[id].isConsistent)
            {
                StartCoroutine(StartQ(id));
            }
        }
        catch { }
    }
    IEnumerator StartQ(int id)
    {
        yield return new WaitForEndOfFrame();
        EventController.instance.StartQEvent(id + 1);
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            Fungus.Character character = GetComponent<Fungus.Character>();
            name_ = PlayerPrefs.GetString("name");
            character.nameText = PlayerPrefs.GetString("name");
            PlayerPrefs.DeleteKey("name");
            hp = max_hp;
            stamina = max_stamina;
            energy = full_energy;
        }
        else
        {
            try 
            {
                Fungus.Character character = GetComponent<Fungus.Character>();
               // name_ = DataManager.DataManager.instance.gameData.cur_slot
            }
            catch { Debug.Log("�"); }
        }
    }
    public Item GetSelectedItem()
    {
        return items[selectedID];
    }
    public void EnergyChange(float value)
    {
        StartCoroutine(ChangeEnergy(value));
    }
    public void SetItem(GameObject g, int i)
    {
        selectedItem = g;
        selectedID = i;
        StopAllCoroutines();//?
    }
    public void SetItem(int i)
    {
        selectedID = i;
        StopAllCoroutines();//?
        switch(items[i].type){
            case itemType.Flashlight: break;
            case itemType.Key: break; 
            case itemType.Card: break; 
            default: InventoryUI.instance.GetMessage(LocalisationSystem.TryGetLocalisedValue("message0")); break;
        }
    }
    public void DeselectItem()
    {
        if(selectedItem!=null) Destroy(selectedItem);
        selectedID = -1;
        StopAllCoroutines();//?
    }
    public void UseItem()
    {
        items[selectedID].Use();
        switch (items[selectedID].type)
        {
            case itemType.Food: RegenerateHP(items[selectedID].value, items[selectedID].speed); break;
            case itemType.Drink: ChangeSpeed(items[selectedID].value, items[selectedID].speed); break;
            case itemType.Battery: { energy += items[selectedID].value; if (energy > full_energy) energy = full_energy; break; }
            case itemType.Drug: RegenerateHP(items[selectedID].value, 1); break;
            case itemType.Flashlight: return; 
            case itemType.Key: if(FindUsedItem("key0")==null){used_items.Add(items[selectedID]); }return; 
            case itemType.Card: return; 
            default: return;
        }
        Destroy(selectedItem);
        used_items.Add(items[selectedID]);
        items.RemoveAt(selectedID);
        GameManager.instance.inv.UpdateData(); 
        selectedID = -1;
    }
    public bool FindQ(int value){
        foreach(Quest q in quests){
            if (q.id==value) return false;
        }
        return true;
    }
    public void LoadData(PlayerData data)
    {
        if(loaded) return;
        seed = data.seed;
        name_ = data.name;
        Fungus.Character character = GetComponent<Fungus.Character>();
        character.nameText = data.name;
        Debug.Log(data.name);
        
        hp = data.hp;
        items.Clear();
        stamina = data.stamina;
        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        transform.position = position;
        for (int i=0; i<data.items.Length;i++)
        {
           items.Add(new Item(data.items[i]));
        }
        //items.Clear();
        InventoryUI.instance.UpdateData();
        used_items.Clear();
        for (int i=0; i<data.used_items.Length;i++)
        {
           used_items.Add(new Item(data.used_items[i]));
        }
        quests.Clear();
        for (int i=0; i<data.quests.Length;i++)
        {
           quests.Add(QuestManager.instance.quests[data.quests[i]]);
           if(i==data.quests.Length-1) { 
               lastQIndex = data.quests[i];
               //StartCoroutine(StartQ(data.quests[i]-1));
               }
        }
        finished_quests.Clear();
        for (int i=0; i<data.finished_quests.Length;i++)
        {
           finished_quests.Add(QuestManager.instance.quests[data.finished_quests[i]]);
        }
        EventController.instance.UpdateQEvent();
        loaded = true;
        Debug.Log("player loaded");
    }
    public bool GetItem(GameObject gm)
    {
        if (items.Count< GameManager.instance.inv.inv_size)
        {
            Item i = new Item(gm.name);
            i.Id = gm.GetComponentInParent<ItemPos>().index;
            items.Add(i);
            GameManager.instance.inv.UpdateData();
            return true;
        }
        return false;
    }
    public void RegenerateHP(float value, float speed)
    {
        if (hp < max_hp) 
        {
            StartCoroutine(Regeneration(value, speed));
            onHpChange.Invoke(value);
        }
    }
    public void ChangeStamina(float value)
    {
        hp -= value;
    }
    public void ChangeSpeed(float value, float time) 
    {
        StartCoroutine(SpeedModifier(value, time));
    }
    IEnumerator SpeedModifier(float value, float time)
    {
        speed_modifier = value;
        while (time!=0)
        {
            time--;
            Debug.Log("time");
            yield return new WaitForSeconds(1);
        }
        speed_modifier = 0;
    }
    IEnumerator Regeneration(float value, float time)
    {
        float hp_ = value/time;
        while (time != 0)
        {
            hp += hp_;
            if (hp>max_hp) { hp = max_hp; break; }
            yield return new WaitForSeconds(1);
            time--;
        }
    }
    public IEnumerator ChangeEnergy(float value)
    {
        while (true)
        {
            if (energy <= 0) break;
            energy -= value;
            yield return new WaitForSeconds(1);
            Debug.Log("energy "+energy);
        }
    }
    void OnDisable(){
        
        EventController.instance.QEvent -= AddQ;
        EventController.instance.endQEvent -= EndQuest;
    }
}
