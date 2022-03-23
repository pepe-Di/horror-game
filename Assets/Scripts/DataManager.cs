using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public List<string> lg = new List<string> { "en", "ru" };
    public static Dictionary<string,Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
    public static Dictionary<string, string> lang = new Dictionary<string, string>();
    public static SaveSlot[] saveSlots = new SaveSlot[4];
    public string[] text;
    public int curlg = 0;
    public static DataManager instance;
    public bool loaded = false;
    public int selectedSlot = -1;
    void Awake()
    {
        if (dictionary.ContainsKey(lg[curlg]))
        {
            return;
        }
        dictionary.Add("en", new Dictionary<string, string>() 
        {
            {"st0","classic" },{"st1","aquamarine" },{"st2","autumn" },{"inv_btn","inventory" },{"quest_btn","quests" },{"cards_btn","cards" },{"pause_h","pause" },{"reset_button","reset" },{"q0","Low" },{"q1","Medium"},{"q2","High"},{"q3","Ultra"},
            {"pressButtonPanel","press any button" },{"new_game","choose a game slot"},{"back_button","back"},
            {"confirm_button","confirm" },{"A slot","A - empty slot"},{"B slot","B - empty slot"},{"C slot","C - empty slot"},{"D slot","D - empty slot"},
            {"load_h","save slots" }, {"load_button","load"},{"clear_button","clear"},
            {"settings_h","settings" },{"slider0","master value" },{"slider1","music" },{"slider2","effects" },{"slider3","text speed" },{"slider4","sensitivity" },{"lgdrop","language" },{"stdrop","style" },
            {"drop0","resolution"},{"drop1","quality"},{"fc","fullscreen" },{"cancel_button","cancel"},{"apply_button","apply"},
            {"menu_font","Ho8Bit" },{"menu_size","31" },{ "menu0", "Continue" }, { "menu1", "New Game" }, { "menu2", "Load" }, { "menu3", "Settings" }, { "menu4", "Exit" }, 
        });
        dictionary.Add("ru", new Dictionary<string, string>() 
        {{"st0","классический" },{"st1","аквамарин" },{"st2","осень" },{"inv_btn","инвентарь" },{"quest_btn","задания" },{"cards_btn","карты" },{"pause_h","пауза" },{"reset_button","сбросить" },{"q0","Низкое" },{"q1","Среднее"},{"q2","Высокое"},{"q3","Ультра"},
            {"pressButtonPanel","нажмите любую клавишу" },{"new_game","выберите игровой слот"},{"back_button","назад"},
            {"confirm_button","подтвердить" },{"A slot","A - пустой слот"},{"B slot","B - пустой слот"},{"C slot","C - пустой слот"},{"D slot","D - пустой слот"},
            {"load_h","игровые слоты" }, {"load_button","загрузить"},{"clear_button","отчистить"},
            {"settings_h","настройки" },{"slider0","общая громкость" },{"slider1","музыка" },{"slider2","эффекты" },{"slider3","скорость текста" },{"slider4","чувствительность" },{"lgdrop","язык" },{"stdrop","стиль" },
            {"drop0","разрешение"},{"drop1","качество"},{"fc","на весь экран" },{"cancel_button","отмена"},{"apply_button","применить"},
            { "menu_font", "pixeldigivolvecyrillic" }, { "menu_size", "41" },{ "menu0", "Продолжить" }, { "menu1", "Новая игра" }, { "menu2", "Загрузить" }, { "menu3", "Настройки" }, { "menu4", "Выход" }, });
        try
        {
            GameData gd = Resources.Load<GameData>("Scriptable/new GameData");
            curlg = gd.lg;
        }
        catch { }
        Dictionary<string, string> d = DataManager.dictionary[lg[curlg]];
        text = new string[] { d["A slot"], d["B slot"], d["C slot"], d["D slot"] };
        SaveSystem.OnAwake(saveSlots, text);

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
