using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Text Message;
    public int inv_size = 15;
    public GameObject descWindow;
    public GameObject inventory;
    public List<GameObject> items = new List<GameObject>();
    public Player player;
    public Transform itemPos;
    public ItemSelect isel;
    bool C = false;
    // Update is called once per frame
    private void Start()=>Player.instance.onHpChange += onPlayerHpChange;
    public void onPlayerHpChange(float value)
    {
        Debug.Log("onPlayerHpChange "+value);
    }
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
                try
                {
                    if (!C)
                    {
                        if (descWindow.name == item.Name)
                        {
                            descWindow.SetActive(false);
                            GameManager.instance.ContinueButton();
                            SetItem(item);
                        }
                        else
                        {
                            descWindow.SetActive(true);
                            descWindow.name = item.Name;
                            Image img = descWindow.GetComponentInChildren<Image>();
                            img.sprite = i.sprite;
                            img.SetNativeSize(); StartCoroutine(Desc(item));
                            return;
                        }
                    }
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
    IEnumerator Desc(Item item)
    {
        C = true;
        var text = descWindow.GetComponentsInChildren<Text>();
        text[0].text = item._name;
        text[1].text = item.GetDesc();
        yield return new WaitForSeconds(0.1f);
        C = false;
    }
    public void SetItem(Item item) 
    {
        try
        {
            if (item!=null)
            {
                switch (item.type)
                {
                    case itemType.Flashlight:
                        {
                            player.SetItem(player.items.IndexOf(item));
                            return; 
                        }
                    default: { 
                            break; 
                        }
                }
                GameObject o = Instantiate(Resources.Load<GameObject>("Prefs/Items/" + item.Name));
                o.transform.SetParent(itemPos);
                o.transform.localPosition = Vector3.zero;
                o.AddComponent<InteractItem>();
                Destroy(o.GetComponent<Outline>());
                player.SetItem(o, player.items.IndexOf(item));
                GetMessage("Нажмите на E чтобы использовать");
            }
        }
        catch
        {

        }
    }
    public void GetMessage(string message)
    {
        Message.text = message;
        Message.gameObject.SetActive(true);
        StartCoroutine(End());
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(2);
        Message.gameObject.SetActive(false);
    }
}
