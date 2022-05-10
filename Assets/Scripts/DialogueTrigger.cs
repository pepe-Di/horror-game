using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Bg clip;
    public bool music;
    // Start is called before the first frame update
    public string value;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventController.instance.StartDialogueEvent(value);
            if(music) SoundManager.instance.PlayBg(clip);
            Destroy(this.gameObject);
        }
    }
}
