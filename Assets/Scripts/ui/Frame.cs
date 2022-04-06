using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameData data = new GameData(SaveSystem.LoadOptions());
        if(!data.frame_mode) this.gameObject.GetComponent<Image>().color = new Color(1,1,1,0);
       // EventController.instance.FrameEvent += ChangeAlpha;
    }
    void Init(){
        
    }
    public void ChangeAlpha(bool frame_mode)
    {
        Color c = DataManager.instance.GetPalette().bg.color;
        if(frame_mode){Debug.Log("ChangeAlpha if");
            this.gameObject.GetComponent<Image>().color = c;}
        else{
            this.gameObject.GetComponent<Image>().color = new Color(c.r,c.g,c.b,0f);
            Debug.Log("ChangeAlpha else");
            }
    }
}
