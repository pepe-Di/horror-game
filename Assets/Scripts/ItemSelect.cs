using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    StarterAssetsInputs _input;
    Player player;
    public GameObject content;
    public InventoryUI inventory;
    public GameObject view;
    public int selectedID = 0;
    private int start_i=0,end_i=7;
    public int max_items;
    int max_cell = 7;
    public bool C_run = false;
    public List<GameObject> items = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _input = inventory.player.gameObject.GetComponent<StarterAssetsInputs>();
        view.SetActive(false);
        max_items = inventory.player.items.Count;
    }
    public void GetItems()
    {
        foreach (GameObject obj in items)
        {
            Destroy(obj);
        }
        items.Clear();
        max_items = inventory.player.items.Count;
        if (selectedID >= max_cell)
        {
            start_i = selectedID-max_cell+1;
        }
        else
        {
            start_i = 0;
        }
        if (max_cell > max_items)
        {
            end_i = max_items;
        }
        else end_i = max_cell;
        for (int i=start_i;i<end_i + start_i; i++)
        {
            GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/Item"));
            Image im = gm.GetComponentInChildren<Image>();
            im.sprite = Resources.Load<Sprite>("Prefs/Items/" + inventory.player.items[i].Name);
            im.SetNativeSize(); 
            Button b = gm.GetComponentInChildren<Button>();
            b.gameObject.name = inventory.player.items[i].Name;
            if (i == selectedID)
            {
                im.color = Color.red;
            }
            gm.transform.SetParent(content.transform);
            items.Add(gm);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_input.scroll.y>0)
        {
            view.SetActive(true);
            if (selectedID > 0) { selectedID--; }
            if(!C_run) StartCoroutine(GetItem());
        }
        else if(_input.scroll.y < 0)
        {
            view.SetActive(true);
            if (selectedID < max_items-1) { selectedID++; }
            if (!C_run) StartCoroutine(GetItem()); 
        }
        else 
        {
        }
    }
    IEnumerator Wait()
    {
        Player.instance.DeselectItem();
        if (items.Count!=0)
        {
            inventory.SetItem(Player.instance.items[selectedID]);
        }
        int sec = 3;
        while (sec != 0)
        {
            //Debug.Log(sec);
            yield return new WaitForSeconds(1);
            sec--;
        }
        view.SetActive(false);
    }
    public void SetWait()
    {
        StopAllCoroutines();
        StartCoroutine(Wait());
    }
    IEnumerator GetItem()
    {
        C_run = true;
        GetItems();
        yield return new WaitForSeconds(0.2f);
        C_run = false;
        SetWait();
    }
}
