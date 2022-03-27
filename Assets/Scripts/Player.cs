using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lastQIndex=-1;
    public static Player instance;
    public string name_;
    public float hp, stamina, max_stamina=1000f,max_hp=10000f, energy, full_energy=5000f;
    public State state;
    public List<Item> items=new List<Item>();
    public List<Quest> quests = new List<Quest>();
    public GameObject selectedItem;
    private int selectedID;
    public float speed_modifier=0;
    public event OnHpChange onHpChange;
    public delegate void OnHpChange(float value);
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        EventController.instance.FlashEvent += EnergyChange;
        EventController.instance.QEvent += AddQ;
        EventController.instance.endQEvent += EndQuest;
    }
    public void AddQ(int id)
    {
        Quest q = QuestManager.instance.quests[id];
        if (q.Type==questType.Use)
        {
            Item i = FindItem(q.finish.name);
            i.questId = id;
            Debug.Log(i.questId);
        }
        quests.Add(q);
    }
    public Item FindItem(string name)
    {
        foreach (Item i in items)
        {
            if (i.Name == name) return i;
        }
        return null;
    }
    public void EndQuest(int id)
    {
        try
        {
            lastQIndex = id;
            quests.Remove(QuestManager.instance.quests[id]);
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
            catch { Debug.Log("ÿ"); }
        }
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
            default: break;
        }
        Destroy(selectedItem);
        items.RemoveAt(selectedID);
        GameManager.instance.inv.UpdateData(); 
        selectedID = -1;
    }
    public void LoadData(PlayerData data)
    {
        name_ = data.name;
        Fungus.Character character = GetComponent<Fungus.Character>();
        character.nameText = data.name;
        Debug.Log(data.name);
        hp = data.hp;
        stamina = data.stamina;
        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        transform.position = position;
        for (int i=0; i<data.items.Length;i++)
        {
            items.Add(new Item(data.items[i]));
        }
        Debug.Log("player loaded");
    }
    public bool GetItem(string name)
    {
        if (items.Count< GameManager.instance.inv.inv_size)
        {
            items.Add(new Item(name));
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
    IEnumerator ChangeEnergy(float value)
    {
        while (true)
        {
            if (energy <= 0) break;
            energy -= value;
            yield return new WaitForSeconds(1);
            Debug.Log("energy "+energy);
        }
    }
}
