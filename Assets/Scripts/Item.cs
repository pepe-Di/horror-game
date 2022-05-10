using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    int index;
    int id;
    public int questId;
    public string _name;
    private string name;
    private bool look=false;
    private bool grab=false;
    public string Name 
    { 
        get 
        {
            return LocalisationSystem.TryGetLocalisedValue("n"+index);
        }  
        set 
        {
            name = value;
        } 
    }
    public string GetGmName(){
        return name;
    }
    public int Index { get => index; set => index = value; }
    public bool Look { get => look; set => look = value; }
    public bool Grab { get => grab; set => grab = value; }
    public int Id { get => id; set => id = value; }

    public itemType type;
    public float value, speed;
    public int click = 0;
    public string GetDesc_()
    {
        switch (type)
        {
            case itemType.Food: return _name+ " восстанавливает <color=red>" + value+ "</color> здоровья за <color=cyan>"+speed+ "</color> секунд.";
            case itemType.Drink: return _name + " повышает скорость персонажа на <color=green>+" + value + "</color>. Эффект длится <color=cyan>"+speed+ "</color> секунд.";
            case itemType.Battery: return _name + " заряжает <color=yellow>" + value+"</color> единиц энергии.";
            case itemType.Drug: return _name + " мгновенно восстанавливает <color=red>" + value+ "</color> единиц здоровья.";
            case itemType.Flashlight: return _name + " тратит батарейки. В темноте теряется рассудок. Чтобы выбрать нажмите F.";
            default: break;
        }
        return "??? неизвестно ???";
    }
    public string GetLookAt(){
        switch(name){
            case "Cola Can": return "look0";
            case "card0": return "look1";
            case "laptop": return "look0";
            case "image0": return "look1";
            case "omamori": return "look2";
            default: return "look0";
        }
    }
    public string GetDesc()
    {
        switch (type)
        {
            case itemType.Food: return LocalisationSystem.TryGetLocalisedValue("desc0") + value+" "+ LocalisationSystem.TryGetLocalisedValue("desc2")+speed+" "+LocalisationSystem.TryGetLocalisedValue("desc3");
            case itemType.Drink: return LocalisationSystem.TryGetLocalisedValue("desc1") + value +" "+ LocalisationSystem.TryGetLocalisedValue("desc5")+speed+" "+ LocalisationSystem.TryGetLocalisedValue("desc3");
            case itemType.Battery: return LocalisationSystem.TryGetLocalisedValue("desc4") + value;
            case itemType.Drug: return LocalisationSystem.TryGetLocalisedValue("desc0") + value;
            case itemType.Flashlight: return LocalisationSystem.TryGetLocalisedValue("desc6");
            case itemType.Key: return LocalisationSystem.TryGetLocalisedValue("desc7");
            case itemType.Card: return LocalisationSystem.TryGetLocalisedValue("desc8");
            default: break;
        }
        return "???";
    }

    public Item(string name)
    {
        this.name = name;
        questId = -1;
        //Debug.Log(name);
        switch (name)
        {
            case "Cola Can" : index=0;_name = Name; value = 1f; speed = 30f; type = itemType.Drink; look=true; break;
            case "Carrot": _name = "морковь"; value = 5f; speed = 20f; type = itemType.Food; break;
            case "Coffee": _name = "кофейный напиток"; value = 3f; speed = 50f; type = itemType.Drink; break;
            case "Coffee2": _name = "кофейный напиток"; value = 2f; speed = 30f; type = itemType.Drink; break;
            case "flashlight": index=1;_name = Name; type = itemType.Flashlight; break;
            case "battery": index=2;_name = Name; value = 1000f; type = itemType.Battery; break;
            case "big battery": index=3;_name = Name; value = 2000f; type = itemType.Battery; break;
            case "Chips": _name = "чипсы"; value = -2f; speed = 10f; type = itemType.Food; break;
            case "kit": index=4;_name = Name; value = 4f; type = itemType.Drug; break;
            case "beans": _name = "консерва"; value = 3f; speed = 120f; type = itemType.Food; break;
            case "Bottle": _name = "вода"; value = 0.01f; speed = 70f; type = itemType.Drink; break;
            case "Cheese": _name = "сыр"; value = 4f; speed = 60f; type = itemType.Food; break;
            case "Mozzarella": _name = "моцарелла"; value = 15f; speed = 60f; type = itemType.Food; break;
            case "Meat": _name = "мясо"; value = -4f; speed = 10f; type = itemType.Food; break;
            case "Milk": _name = "молоко"; value = -2f; speed = 60f; type = itemType.Drink; break;
            case "pills": index=5;_name = Name; value = 2f; type = itemType.Drug; break;
            case "Yogurt": _name = "Йогурт"; value = 5f; speed = 120f; type = itemType.Food; break;
            case "Sandwich": _name = "Бутерброд"; value = 1f; speed = 90f; type = itemType.Food; break;
            case "Tomato": _name = "Томат"; value = 2f; speed = 20f; type = itemType.Food; break;
            case "key0": index=6;_name = Name;  type = itemType.Key; break;
            case "card0": index=7;_name = Name;  type = itemType.Card; look=true; break;
            case "key1": index=8;_name = Name;  type = itemType.Key; break;
            case "laptop": index=-1;grab = false; look=true; break;
            case "notepad": index=9; _name = Name; type=itemType.Card; look=true; grab=true; break;
            case "omamori": index=10; _name = Name; type=itemType.Card; look=true; grab=true; break;

            default: index=-1; break;
        }
        switch (type)
        {
            case itemType.Food: grab = true; break;
            case itemType.Drink: grab = true; break;
            case itemType.Battery: grab = true; break;
            case itemType.Drug: grab = true; break;
            case itemType.Flashlight: grab = true; break;
            case itemType.Key: grab = true; break;
            case itemType.Card: grab = true; break;
            default: grab = false; break;
        }
        if(index==-1) grab = false;
      //  Debug.Log(grab);
       
    }
    public void Use()
    {
        switch (type)
        {
            case itemType.Flashlight: return;
            default: break;
        }
        if(questId>=0)
        {
            EventController.instance.EndQEvent(questId);
            Debug.Log(questId);
         //   EventController.instance.UpdateQEvent();
            questId = -1;
        }
    }
    ~Item () { }
}
public enum itemType
{
    Food,
    Drink,
    Battery,
    Flashlight,
    Drug,
    Key,
    Card
}
public class Quest
{
    public int id;
    private questType type;
    public questType Type { get { return type; } set { type = value; } }
    public string name;
    public bool isConsistent;
    public GameObject finish;
    public Quest(questType type, string name,bool b)
    {
        Type = type;
        this.name = name;
        isConsistent = b;
        switch (type) 
        {
            case questType.ToPoint: break;
            case questType.Grab: break;
            case questType.Use: break;
            case questType.Puzzle: break;
            default: break;
        }

    }
    public Quest(questType type, string name, bool b, string s)
    {
        Type = type;
        this.name = name;
        isConsistent = b;
        switch (type)
        {
            case questType.ToPoint: break;
            case questType.Grab: break;
            case questType.Use: break;
            case questType.Puzzle: break;
            default: break;
        }
        finish = new GameObject(s);
    }
}
public enum questType
{
    ToPoint,
    Use,
    Grab,
    Puzzle,
    Hide
}