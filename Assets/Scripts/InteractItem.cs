using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("E to eat");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance._input.interact)
        {
            Player.instance.UseItem();
        }
    }
}
