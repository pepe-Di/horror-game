using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    public int id;
    ColorPalette colorPalette;
    IEnumerator Start()
    {
        //EventController.instance.QEvent += ChangeQName;
        yield return new WaitUntil(() => DataManager.instance.loaded);
        ChangeColor();
    }
    public void ChangeColor()
    {
        GameData data = new GameData(SaveSystem.LoadOptions());
        colorPalette = DataManager.instance.palettes[data.style];
        var img = GetComponentsInChildren<Image>();
        var txt = this.gameObject.GetComponentsInChildren<Text>();
        foreach (Text t in txt)
        {
            t.color = colorPalette.fg.color;
        }
        
        foreach(Image i in img)
        {
            if(i.GetComponent<UiElement>().type==UIColor.Fg)
            {
                i.color = colorPalette.fg.color;
            }
            else
            {
                i.color = colorPalette.bg.color;
            }
        }
    }
}
