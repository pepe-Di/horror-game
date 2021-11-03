using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private ThirdPersonController controller;
    private StarterAssetsInputs _input;
    private void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chair")
        {
            Debug.Log("Press E to sit");
            if (_input.interact) { }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Chair")
        {
            if (_input.interact)
            {
                Debug.Log("pressing e");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Chair")
        {
            Debug.Log("exit area");
        }
    }
}
