using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static SaveSlot[] saveSlots = new SaveSlot[4];
    public string[] text => new string[] { "A - empty slot", "B - empty slot", "C - empty slot", "D - empty slot" };
    public static DataManager instance;
    public bool loaded = false;
    public int selectedSlot = -1;
    void Awake()
    {
        Debug.Log(text[0]);
        SaveSystem.OnAwake(saveSlots,text);
    }
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
    }
    public void Cutsceen() 
    {

    }
    public void NewGame()
    {
        FindObjectOfType<LevelLoader>().LoadLevel(2);
       // GameObject player = (GameObject)Instantiate(Resources.Load("Prefs/Player Variant"));
    }
    public void LoadGame()
    {
        try
        {
            loaded = true;
            SaveSlot data = SaveSystem.LoadPlayer();
            FindObjectOfType<LevelLoader>().LoadLevel(data.playerData.sceneIndex);
           
            Debug.Log("LoadGame()");
        }
        catch { Debug.LogError("erorr"); }
    }
    public void SaveData()
    {
        SaveSystem.SavePlayer(FindObjectOfType<Player>()); Debug.Log("SaveData()");
    }
    void Update()
    {
        
    }
}
