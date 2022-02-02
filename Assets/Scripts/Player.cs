using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public List<Item> ii;
    public string name_;
    public float hp, stamina, max_stamina=1000f,max_hp=10000f;
    public State state;
    public List<Item> items=new List<Item>();
    public GameObject selectedItem;
    private int selectedID;
    public float speed_modifier=0;
    // Start is called before the first frame update
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
        }
    }
    public void SetItem(GameObject g, int i)
    {
        selectedItem = g;
        selectedID = i;
    }
    public void DeselectItem()
    {
        if(selectedItem!=null) Destroy(selectedItem);
        selectedID = -1;
    }
    public void UseItem()
    {
        switch (items[selectedID].type)
        {
            case itemType.Food: RegenerateHP(items[selectedID].value, items[selectedID].speed); break;
            case itemType.Drink: ChangeSpeed(items[selectedID].value, items[selectedID].speed); break;
            case itemType.Battery: break;
            case itemType.Drug: RegenerateHP(items[selectedID].value, 1); break;
            case itemType.Torch: return; 
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
        if (items.Count<15)
        {
            items.Add(new Item(name));
            GameManager.instance.inv.UpdateData();
            return true;
        }
        return false;
    }
    public void RegenerateHP(float value, float speed)
    {
        if(hp<max_hp) StartCoroutine(Regeneration(value, speed));
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
}
