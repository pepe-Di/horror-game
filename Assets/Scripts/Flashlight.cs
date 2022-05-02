using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    Coroutine c = null;
    public GameObject ui;
    public GameObject energybar;
    public List<Image> imgs=new List<Image>();
    private float value,speed=200f;
    Color color, transponent;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!started){
 ui.SetActive(true);
        imgs.AddRange(energybar.GetComponentsInChildren<Image>());
        imgs.Reverse();
        color = imgs[0].color;
        transponent = imgs[0].color;
        transponent.a = 0;
        value = Player.instance.full_energy / (imgs.Count-1);
        Debug.Log(value);
        EventController.instance.FlashEvent += EnergyBarChange;
        EventController.instance.StartFlashEvent(speed);started=true;
        }
       
    }
    public void EnergyBarChange(float value)
    {
        if(GameManager.instance.player_!=null)  {
            //GameManager.instance.player_.EnergyChange(value);
            c = StartCoroutine(GameManager.instance.player_.ChangeEnergy(value));
            }
        StartCoroutine(EnergyChange());
    }
    IEnumerator EnergyChange()
    {
        while (true)
        {
            float f = GameManager.instance.player_.energy / value;
            int t = (int)f + 1;
            for (int i = 0; i < t; i++)
            {
                imgs[i].color = color;
            }
            if (t < imgs.Count)
            {
                imgs[t].color = transponent;
                Debug.Log(t);
            }
            if (f == 0) { imgs[0].color = transponent;
                this.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            Debug.Log("bam");
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if(c!=null) StopCoroutine(c);
        ui.SetActive(false);
        EventController.instance.FlashEvent -= EnergyBarChange;
    }
    private void OnEnable()
    {
        if(started){
ui.SetActive(true);
        EventController.instance.FlashEvent += EnergyBarChange;
        EventController.instance.StartFlashEvent(speed);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
