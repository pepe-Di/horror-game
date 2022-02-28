using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static Dictionary<string,Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
    public static Dictionary<string, string> lang = new Dictionary<string, string>();
    public static SaveSlot[] saveSlots = new SaveSlot[4];
    public string[] text => new string[] { "A - empty slot", "B - empty slot", "C - empty slot", "D - empty slot" };
    public static DataManager instance;
    public bool loaded = false;
    public int selectedSlot = -1;
    void Awake()
    {
        dictionary.Add("en", new Dictionary<string, string>() 
        {
            {"pressButtonPanel","press any button" },{"new_game","choose a game slot"},{"back_button","back"},
            {"confirm_button","confirm" },{"A slot","A - empty slot"},{"B slot","B - empty slot"},{"C slot","C - empty slot"},{"D slot","D - empty slot"},
            {"load_h","save slots" }, {"load_button","load"},{"clear_button","clear"},
            {"settings_h","settings" },{"slider0","master value" },{"slider1","music" },{"slider2","effects" },
            {"drop0","resolution"},{"drop1","quality"},{"fc","fullscreen" },{"cancel_button","cancel"},{"apply_button","apply"},
            {"menu_font","Ho8Bit" },{"menu_size","31" },{ "menu0", "Continue" }, { "menu1", "New Game" }, { "menu2", "Load" }, { "menu3", "Settings" }, { "menu4", "Exit" }, 
        });
        dictionary.Add("ru", new Dictionary<string, string>() 
        {{"pressButtonPanel","������� ����� �������" },{"new_game","�������� ������� ����"},{"back_button","�����"},
            {"confirm_button","�����������" },{"A slot","A - ������ ����"},{"B slot","B - ������ ����"},{"C slot","C - ������ ����"},{"D slot","D - ������ ����"},
            {"load_h","������� �����" }, {"load_button","���������"},{"clear_button","���������"},
            {"settings_h","���������" },{"slider0","����� ���������" },{"slider1","������" },{"slider2","�������" },
            {"drop0","����������"},{"drop1","��������"},{"fc","�� ���� �����" },{"cancel_button","������"},{"apply_button","���������"},
            { "menu_font", "pixeldigivolvecyrillic" }, { "menu_size", "41" },{ "menu0", "����������" }, { "menu1", "����� ����" }, { "menu2", "���������" }, { "menu3", "���������" }, { "menu4", "�����" }, });
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
