using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventory;
    public List<GameObject> items = new List<GameObject>();
    public Player player;
    public Transform itemPos;
    public ItemSelect isel;
    // Update is called once per frame
    public void UpdateData()
    {
        foreach(GameObject obj in items)
        {
            Destroy(obj);
        }
        items.Clear();
        foreach (Item item in player.items)
        {
            Debug.Log("Prefs/Items/" + item.Name);
            GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/Item"));
            Image i = gm.GetComponentInChildren<Image>();
            i.sprite = Resources.Load<Sprite>("Prefs/Items/" + item.Name);
            i.SetNativeSize();
            Button b = gm.GetComponentInChildren<Button>();
            b.gameObject.name = item.Name;
            b.onClick.AddListener(()=> 
            {
                GameManager.instance.ContinueButton();
                try
                {
                    SetItem(item);
                }
                catch
                {

                }
                Debug.Log(b.gameObject.name); 
            });
            gm.transform.SetParent(inventory.transform);
            items.Add(gm);
        }
        isel.GetItems();
    }
    public void SetItem(Item item) 
    {
        try
        {
            if (item!=null)
            {
                GameObject o = Instantiate(Resources.Load<GameObject>("Prefs/Items/" + item.Name + " Item"));
                o.transform.SetParent(itemPos);
                o.transform.localPosition = Vector3.zero;
                player.SetItem(o, player.items.IndexOf(item));
            }
        }
        catch
        {

        }
    }
}
