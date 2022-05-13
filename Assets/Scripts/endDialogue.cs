using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endDialogue : MonoBehaviour
{
    public Bg clip;
    public bool music;
    // Start is called before the first frame update
    public string value;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void PlayEnding(){
        EventController.instance.StartDialogueEvent(value);
            if(music) SoundManager.instance.PlayBg(clip);
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            for(int j=0; j<5; j++)
            {
                Item item = player.FindItem("card"+j);
                if(item == null) {PlayEnding(); return;} 
            }
            EventController.instance.StartDialogueEvent("altEnding");
            if(music) SoundManager.instance.PlayBg(clip);
            Destroy(this.gameObject);
        }
    }
    }
