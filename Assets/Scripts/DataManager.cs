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
        {{"pressButtonPanel","нажмите любую клавишу" },{"new_game","выберите игровой слот"},{"back_button","назад"},
            {"confirm_button","подтвердить" },{"A slot","A - пустой слот"},{"B slot","B - пустой слот"},{"C slot","C - пустой слот"},{"D slot","D - пустой слот"},
            {"load_h","игровые слоты" }, {"load_button","загрузить"},{"clear_button","отчистить"},
            {"settings_h","настройки" },{"slider0","общая громкость" },{"slider1","музыка" },{"slider2","эффекты" },
            {"drop0","разрешение"},{"drop1","качество"},{"fc","на весь экран" },{"cancel_button","отмена"},{"apply_button","применить"},
            { "menu_font", "pixeldigivolvecyrillic" }, { "menu_size", "41" },{ "menu0", "Продолжить" }, { "menu1", "Новая игра" }, { "menu2", "Загрузить" }, { "menu3", "Настройки" }, { "menu4", "Выход" }, });
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
