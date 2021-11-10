using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool opened { set; get; }
    void Awake()
    {
        opened = false;
    }
}
