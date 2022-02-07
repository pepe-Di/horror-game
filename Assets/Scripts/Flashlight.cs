using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public GameObject ui;
    public GameObject energybar;
    public List<Image> imgs=new List<Image>();
    private float value,speed=200f;
    Color color, transponent;
    // Start is called before the first frame update
    void Start()
    {
        ui.SetActive(true);
        imgs.AddRange(energybar.GetComponentsInChildren<Image>());
        imgs.Reverse();
        color = imgs[0].color;
        transponent = imgs[0].color;
        transponent.a = 0;
        value = Player.instance.full_energy / (imgs.Count-1);
        Debug.Log(value);
        EventController.instance.FlashEvent += EnergyBarChange;
        EventController.instance.StartFlashEvent(speed);
    }
    public void EnergyBarChange(float value)
    {
        StartCoroutine(EnergyChange());
    }
    IEnumerator EnergyChange()
    {
        while (true)
        {
            float f = Player.instance.energy / value;
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
        ui.SetActive(false);
        //EventController.instance.FlashEvent -= EnergyBarChange;
    }
    private void OnEnable()
    {
        ui.SetActive(true);
        //EventController.instance.FlashEvent += EnergyBarChange;
        EventController.instance.StartFlashEvent(speed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
