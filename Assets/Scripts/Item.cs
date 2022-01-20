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

    public Item(string name)
    {
        this.name = name;
    }
}
