using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particles : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform parent;
    string path,pref_path="ui/particles/particle";
    [SerializeField] public float min_duration=0.02f, max_duration = 0.2f;
    ColorPalette cp;
    Color color;
    bool ended = false;GameData gameData;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.instance.loaded);
        gameData = new GameData(SaveSystem.LoadOptions());
        cp=DataManager.instance.palettes[gameData.style];
        path = "ui/particles/" + cp.Name+"/";
        color = cp.fg.color;
        ended = true;
    }
    IEnumerator SpawnParticles()
    {
        while (true)
        {
            GameObject o = Instantiate(Resources.Load<GameObject>(pref_path), parent);
            Image im = o.GetComponent<Image>();
            im.sprite = Resources.Load<Sprite>(path+Random.Range(0,15));
            im.SetNativeSize();
            im.color = color;
            switch (Random.Range(0,8))
            {
                case 0:
                    o.transform.eulerAngles = new Vector3(0, 0, 90); break;
                case 1: o.transform.eulerAngles = new Vector3(0, 0, -90); break;
                case 2: o.transform.eulerAngles = new Vector3(0, 0, 180); break;
                default: break;
            }
            float r = Random.Range(-2, 10);
            RectTransform rect = o.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x+r, rect.sizeDelta.y + r);
            yield return new WaitForSeconds(Random.Range(min_duration, max_duration));
        }
    }
    // Update is called once per frame
    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        StopAllCoroutines();
    }
    void OnEnable()
    {Debug.Log("PrintOnEnable: script was enabled");
         if(ended){
             gameData = new GameData(SaveSystem.LoadOptions());
            cp=DataManager.instance.GetPalette();
        path = "ui/particles/" + cp.Name+"/";
        color = cp.fg.color;
            StartCoroutine(SpawnParticles());
            }
        else{
        ended = true;StartCoroutine(SpawnParticles());
    } 
    //StartCoroutine(SpawnParticles());

    }
}
