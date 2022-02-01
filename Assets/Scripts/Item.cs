using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
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
    public Item(string name)
    {
        this.name = name;
        switch (name)
        {
            case "Cola Can": value = 2f; speed = 30f; type = itemType.Drink; break;
            case "Carrot": value = 50f; speed = 1f; type = itemType.Food; break;
            default: break;
        }
    }
    public void Use()
    {
        
    }
    ~Item () { }
}
public enum itemType
{
    Food,
    Drink,
    Battery,
    Torch,
    Drug
}
