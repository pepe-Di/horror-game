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
    public bool newGame=false;
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
            colors.Add(new Color_("Dark Red", 106f / 255f, 94f / 255f, 74f / 255f));
            colors.Add(new Color_("Lemon", 250f / 255f, 222f / 255f, 145f / 255f));
            colors.Add(new Color_("Light Yellow", 200f / 255f, 200f / 255f, 168f / 255f));
            colors.Add(new Color_("Grass", 70f / 255f, 100f / 255f, 100f / 255f));
            colors.Add(new Color_("Pink", 253f / 255f, 73f / 255f, 146f / 255f));
            colors.Add(new Color_("Black", 35f / 255f, 16f / 255f, 16f / 255f));
            colors.Add(new Color_("Bright Pink", 255f / 255f, 164f / 255f, 194f / 255f));
            colors.Add(new Color_("Light Blue",107f / 255f, 113f / 255f, 154f / 255f));
            colors.Add(new Color_("White",235f / 255f, 225f / 255f, 253f / 255f));
            colors.Add(new Color_("Gray",106f / 255f, 111f / 255f, 133f / 255f));
        }
        if (palettes.Count == 0)
        {
            palettes = new List<ColorPalette>();
            palettes.Add(new ColorPalette("Black & White", colors.Where(c=>c.Name == "Gray").FirstOrDefault(), colors.Where(c => c.Name == "White").FirstOrDefault()));
            palettes.Add(new ColorPalette("Lemon", colors.Where(c => c.Name == "Dark Red").FirstOrDefault(), colors.Where(c => c.Name == "Lemon").FirstOrDefault()));
            palettes.Add(new ColorPalette("Dandelion", colors.Where(c => c.Name == "Grass").FirstOrDefault(), colors.Where(c => c.Name == "Light Yellow").FirstOrDefault()));
            palettes.Add(new ColorPalette("Sakura", colors.Where(c => c.Name == "Light Blue").FirstOrDefault(), colors.Where(c => c.Name == "Bright Pink").FirstOrDefault()));
            palettes.Add(new ColorPalette("Magic", colors.Where(c=>c.Name == "Black").FirstOrDefault(), colors.Where(c => c.Name == "Pink").FirstOrDefault()));
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
        newGame=false;
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
