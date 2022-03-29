using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool selected = false, isEnabled=false;

    private void OnEnable()
    {
        isEnabled = true;
    }
    private void OnDisable()
    {
        isEnabled = false;
    }
}