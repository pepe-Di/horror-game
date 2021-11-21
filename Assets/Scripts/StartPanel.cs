using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    GameObject g;
    MenuManager mm;
    void Start()
    {
        g = this.gameObject;
        mm = FindObjectOfType<MenuManager>();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            mm.PressButton(g);
        }
    }
}
