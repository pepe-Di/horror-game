using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventory;
    public List<GameObject> items = new List<GameObject>();
    public Player player;
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
            GameObject gm = new GameObject(item.Name + "Item");
            gm.AddComponent<Image>();
            gm.GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefs/Items/" + item.Name);
            gm.transform.SetParent(inventory.transform);
            items.Add(gm);
        }
    }
}
