using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cartridge : MonoBehaviour
{
    // Start is called before the first frame update
    string path = "ui/gif/";
    Image img;
    float timer = 0.05f;
    void Start()
    {
        img = this.gameObject.GetComponent<Image>();
        StartCoroutine(Spin());
        img.color = DataManager.instance.GetPalette().fg.color;
    }
    IEnumerator Spin()
    {
        while (true)
        {
            for (int i=0; i<20;i++)
            {
                img.sprite = Resources.Load<Sprite>(path+i);
                img.SetNativeSize();
                yield return new WaitForSeconds(timer);
            }
        }
    }
    private void OnDisable()
    {
        StopCoroutine(Spin());
        Destroy(this.gameObject);
    }
}
