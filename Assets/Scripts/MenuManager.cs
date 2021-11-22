using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject menu;
    private GameObject _mainCamera;
    public GameObject icon;
    public List<GameObject> buttons;
    public List<GameObject> slots;
    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<DataManager>();
        if (obj.Length > 1)
        {
            int i = 0;
            for (i=1; i<obj.Length; i++)
            {
                Destroy(obj[i].gameObject);
            }
        }
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        menu = GameObject.Find("GameMenu");
        int j = 0;
       foreach(GameObject slot in slots)
        {
            if (j == 4) j = 0;
            slot.name = j.ToString();
                //DataManager.saveSlots[j].name;
            slot.GetComponentInChildren<Text>().text = DataManager.saveSlots[j].text;
            j++;
        }
    }
    public void ClearSlot()
    {

    }
    public void LoadGame()
    {
        try 
        {
            if (selectedSlot != -1)
            {
                DataManager.instance.selectedSlot = selectedSlot;
                DataManager.instance.LoadGame();
            }
        }
        catch { Debug.LogError("erorr"); }
    }
    public int selectedSlot = -1;
    public void SelectSlot(GameObject gm)
    {
        selectedSlot = int.Parse(gm.name);
        Debug.Log(selectedSlot);
    }
    public void DeselectSlot()
    {
      //  selectedSlot = -1;
      //  Debug.Log(selectedSlot);
    }
    public void OnStart()
    {
        if (selectedSlot != -1) 
        { 
            DataManager.instance.selectedSlot = selectedSlot;
            FindObjectOfType<LevelLoader>().LoadLevel(1);
        }
    }
    public void SelectedUI(Transform button)
    {
        //icon.transform.position =new Vector3(icon.transform.position.x, button.position.y,0);
        button.GetComponent<Image>().color = Color.white;
        button.GetComponentInChildren<Text>().color = Color.black;
        foreach (GameObject gm in buttons)
        {
            if (gm.name != button.name)
            {
                gm.GetComponent<Image>().color = Color.black;
                gm.GetComponentInChildren<Text>().color = Color.white;
            }
        }
    }
    public void ExitUI(GameObject button)
    {
        button.GetComponent<Image>().color = Color.black;
        button.GetComponentInChildren<Text>().color = Color.white;
    }
    public void SelectButton(GameObject gm)
    {
        gm.GetComponent<Image>().color = Color.white;
        gm.GetComponentInChildren<Text>().color = Color.black;
        foreach (GameObject button in buttons) 
        {
            if (button.name != gm.name)
            {
                button.GetComponent<Image>().color = Color.black;
                button.GetComponentInChildren<Text>().color = Color.white;
            }
        }
    }
    public void PressButton(GameObject gm)
    {
        StartCoroutine(Skip(gm));
    }
    public IEnumerator Skip(GameObject gm)
    {
        if (!gm.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start"))
        {
            gm.GetComponent<Animator>().SetTrigger("End");
            yield return new WaitForSeconds(0.3f);
            gm.SetActive(false);
        }
    }
    public void OnExit()
    {
        Application.Quit();
    }
    void Update()
    {
    }
}
