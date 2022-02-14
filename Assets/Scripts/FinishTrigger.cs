using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public int id;
    public bool isEnabled = false;
    private void Awake()
    {
        if (!isEnabled)
        {
            Destroy(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventController.instance.EndQEvent(id);
            EventController.instance.UpdateQEvent();
            Destroy(this.gameObject);
        }
    }
}
