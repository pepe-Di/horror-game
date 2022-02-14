using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int questId;
    public string _name;
    private string name;
    public string Name 
    { 
        get 
        {
            return name;
        }  
        set 
        {
            name = value;
        } 
    }
    public itemType type;
    public float value, speed;
    public int click = 0;
    public string GetDesc()
    {
        switch (type)
        {
            case itemType.Food: return _name+ " ��������������� <color=red>" + value+ "</color> �������� �� <color=cyan>"+speed+ "</color> ������.";
            case itemType.Drink: return _name + " �������� �������� ��������� �� <color=green>+" + value + "</color>. ������ ������ <color=cyan>"+speed+ "</color> ������.";
            case itemType.Battery: return _name + " �������� <color=yellow>" + value+"</color> ������ �������.";
            case itemType.Drug: return _name + " ��������� ��������������� <color=red>" + value+ "</color> ������ ��������.";
            case itemType.Flashlight: return _name + " ������ ���������. � ������� �������� ��������. ����� ������� ������� F.";
            default: break;
        }
        return "??? ���������� ???";
    }
    public Item(string name)
    {
        this.name = name;
        questId = -1;
        switch (name)
        {
            case "Cola Can": _name = "����"; value = 1f; speed = 30f; type = itemType.Drink; break;
            case "Carrot": _name = "�������"; value = 50f; speed = 20f; type = itemType.Food; break;
            case "Coffee": _name = "�������� �������"; value = 3f; speed = 50f; type = itemType.Drink; break;
            case "Coffee2": _name = "�������� �������"; value = 2f; speed = 30f; type = itemType.Drink; break;
            case "flashlight": _name = "������"; type = itemType.Flashlight; break;
            case "battery": _name = "������� 1�"; value = 1000f; type = itemType.Battery; break;
            case "big battery": _name = "������� 2�"; value = 2000f; type = itemType.Battery; break;
            case "Chips": _name = "�����"; value = -10f; speed = 10f; type = itemType.Food; break;
            case "kit": _name = "�������"; value = 1000f; type = itemType.Drug; break;
            case "beans": _name = "��������"; value = 20f; speed = 120f; type = itemType.Food; break;
            case "Bottle": _name = "����"; value = 0.01f; speed = 70f; type = itemType.Drink; break;
            case "Cheese": _name = "���"; value = 80f; speed = 60f; type = itemType.Food; break;
            case "Mozzarella": _name = "���������"; value = 80f; speed = 60f; type = itemType.Food; break;
            case "Meat": _name = "����"; value = -40f; speed = 10f; type = itemType.Food; break;
            case "Milk": _name = "������"; value = -4f; speed = 70f; type = itemType.Drink; break;
            case "pills": _name = "���������"; value = 500f; type = itemType.Drug; break;
            case "Yogurt": _name = "������"; value = 1500f; speed = 120f; type = itemType.Food; break;
            case "Sandwich": _name = "���������"; value = 150f; speed = 90f; type = itemType.Food; break;
            case "Tomato": _name = "�����"; value = 20f; speed = 20f; type = itemType.Food; break;

            default: break;
        }
    }
    public void Use()
    {
        if(questId>=0)
        {
            EventController.instance.EndQEvent(questId);
            Debug.Log(questId);
            EventController.instance.UpdateQEvent();
            questId = -1;
        }
        switch (type)
        {
            case itemType.Flashlight: break;
            default: break;
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
    Drug
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
    Puzzle
}