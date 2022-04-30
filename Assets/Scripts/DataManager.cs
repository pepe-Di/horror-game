using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameData gameData;
    public List<string> lg = new List<string> { "en", "ru" };
    public static Dictionary<string,Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
    public static Dictionary<string, string> lang = new Dictionary<string, string>();
    public static SaveSlot[] saveSlots = new SaveSlot[4];
    public string[] text;
    public int curlg = 0;
    public static DataManager instance;
    public bool loaded = false;
    public int selectedSlot = -1, selectedPalette=-1;
    public List<ColorPalette> palettes;
    public List<Color_> colors;
    public ColorPalette GetPalette()
    {
        return palettes[selectedPalette];
    }
    void Awake()
    {
        if (colors.Count == 0)
        {
            colors = new List<Color_>();
            colors.Add(new Color_("Dark Blue", 60f / 255f, 53f / 255f, 130f / 255f));
            colors.Add(new Color_("White Blue", 124f / 255f, 212f / 255f, 217f / 255f));
            colors.Add(new Color_("Dark Red", 94f / 255f, 34f / 255f, 25f / 255f));
            colors.Add(new Color_("Lemon", 233f / 255f, 233f / 255f, 131f / 255f));
            colors.Add(new Color_("Light Yellow", 200f / 255f, 200f / 255f, 168f / 255f));
            colors.Add(new Color_("Grass", 70f / 255f, 100f / 255f, 100f / 255f));
        }
        if (palettes.Count == 0)
        {
            palettes = new List<ColorPalette>();
            palettes.Add(new ColorPalette("Marine", colors.Where(c=>c.Name == "Dark Blue").FirstOrDefault(), colors.Where(c => c.Name == "White Blue").FirstOrDefault()));
            palettes.Add(new ColorPalette("Daylight", colors.Where(c => c.Name == "Dark Red").FirstOrDefault(), colors.Where(c => c.Name == "Lemon").FirstOrDefault()));
            palettes.Add(new ColorPalette("Dandelion", colors.Where(c => c.Name == "Grass").FirstOrDefault(), colors.Where(c => c.Name == "Light Yellow").FirstOrDefault()));
        }
        text = new string[] { LocalisationSystem.TryGetLocalisedValue("A slot"), LocalisationSystem.TryGetLocalisedValue("B slot"), LocalisationSystem.TryGetLocalisedValue("C slot"), LocalisationSystem.TryGetLocalisedValue("D slot") };
        SaveSystem.OnAwake(saveSlots, text);
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
        loaded = true;
        try{
            GameData data = new GameData(SaveSystem.LoadOptions());
            selectedPalette = data.style;
        }
        catch{}
    }
    void Start()
    {
    }
    public void Cutsceen() 
    {

    }
    public void NewGame()
    {
        FindObjectOfType<LevelLoader>().LoadLevel(1);
       // GameObject player = (GameObject)Instantiate(Resources.Load("Prefs/Player Variant"));
    }
    public void LoadGame()
    {
        
            loaded = true;
           // SaveSlot data = SaveSystem.LoadPlayer();
            FindObjectOfType<LevelLoader>().LoadLevel(1);
           
            Debug.Log("LoadGame()");
        
    }
    public void SaveData()
    {
       // SaveSystem.SavePlayer(FindObjectOfType<Player>()); Debug.Log("SaveData()");
    }
    void Update()
    {
        
    }
}
public enum UIColor
{
    Bg,Fg
}
