using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class MenuManager : MonoBehaviour
{
    public List<GameObject> panels;
    public AudioSource sfx;
    private AudioSource audio_;
    public GameData gameData;
    public AudioMixer audioMixer;
    float currentVolume;
    Resolution[] resolutions;
    public Dropdown resDrop;
    public Dropdown qaDrop;
    public Dropdown lgDrop;
    public Dropdown stDrop;
    public Toggle fullToggle;
    public Slider master, music, effects, txt_speed, sens;
    private GameObject menu;
    private GameObject _mainCamera;
    //public GameObject icon;
    public List<GameObject> buttons;
    public List<GameObject> slots;
    public List<string> lg = new List<string> { "en", "ru"  };
    private int curlg=0;
    public Material mat;
    public Button lgButton;
    Text[] txt;
    List<Image> imgs;
    List<Text> texts;
    Color bg, fg;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1f;
    }
    void createDrops()
    {
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();
        List<string> options = new List<string>();
        int curRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                curRes = i;
                if(gameData.res==-1) gameData.res = i;
            }
        }
        if (gameData.res != -1)
        {
            curRes = gameData.res;
        }
        resDrop.AddOptions(options);
        resDrop.value = curRes;
        resDrop.RefreshShownValue();
    }
    void Start()
    {
        if (gameData == null)
        {
            gameData = new GameData();
            gameData.q = QualitySettings.GetQualityLevel();
            gameData.fc = Screen.fullScreen;
        }
        else
        {
        }
        if (panels != null)
        {
            foreach (GameObject o in panels)
            {
                o.SetActive(true);
            }
        }
        StyleUIChanger();
        txt = GameObject.FindObjectsOfType<Text>();
        if (panels != null)
        {
            foreach (GameObject o in panels)
            {
                o.SetActive(false);
            }
        }
        audio_ = GetComponent<AudioSource>();
        createDrops();
        if (stDrop != null)
        {
            //stDrop.ClearOptions();
            //try
            //{
            //    List<string> opts = new List<string>();
            //    foreach (ColorPalette cp in DataManager.instance.palettes)
            //    {
            //        opts.Add(cp.Name);
            //    }
            //    stDrop.AddOptions(opts);
            //}
            //catch { }
        }
        UpdateData();
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
        bg = DataManager.instance.palettes[gameData.style].bg.color;
        fg = DataManager.instance.palettes[gameData.style].fg.color;
    }
    public void UpdateData()
    {
        audioMixer.SetFloat("master", gameData.master_vol);
        audioMixer.SetFloat("music", gameData.music_vol);
        audioMixer.SetFloat("effects", gameData.effects_vol);
        master.value = gameData.vol;
        music.value = gameData.vol1;
        effects.value = gameData.vol2;
        curlg = gameData.lg;
        try
        {
            DataManager.instance.curlg = gameData.lg;
        }
        catch { }
        if (lgButton != null)
        {
            lgButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/" + lg[curlg]);
        }
        if (lgDrop != null)
        {
            lgDrop.value = gameData.lg;
        }
        if (txt_speed != null)
        {
            txt_speed.value = gameData.txt_speed;
            SetTxtSpeed(gameData.txt_speed);
        }
        if (sens != null)
        {
            sens.value = gameData.sens;
            ChangeSens(gameData.sens);
        }
        if (stDrop != null)
        {
            stDrop.value = gameData.style;
            stDrop.RefreshShownValue();
        }
        LangChanger();
        qaDrop.value = gameData.q;
        fullToggle.isOn = gameData.fc;
        resDrop.value = gameData.res;
    }
    public void ContinueButton()
    {
        if (gameData.last_slot != -1)
        {

        }
    }
    public void lgChange(Button b)
    {
        curlg++;
        if (curlg >= lg.Count) curlg = 0; 
        b.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/" + lg[curlg]); 
        LangChanger();
        gameData.lg = curlg;
    }
    public void SetLang(int i)
    {
        curlg = i;
    }
    public void ChangeSens(float value)
    {
        Player.instance.GetComponent<MouseLook>().SensChange(value);
    }
    public void ChangeUIColor(Color bg, Color fg)
    {
        foreach (Image i in imgs)
        {
            try
            {
                UiElement el;
                if (i.gameObject.TryGetComponent<UiElement>(out el)) 
                {
                    if (el.type == UIColor.Bg) 
                    {
                       // if (el.alpha) 
                            i.color = bg;
                       // else i.color = new Color(bg.r, bg.g, bg.b, 0f);
                    }
                    else i.color = fg;
                }
                else
                {
                    i.gameObject.AddComponent<UiElement>();
                    UiElement e = i.gameObject.GetComponent<UiElement>();
                    //Button b;
                    //if(i.gameObject.TryGetComponent<Button>(out b))
                    //{
                    //    i.color = new Color(bg.r, bg.g, bg.b, 0f);
                    //    e.alpha = false;
                    //}
                    //else
                    //{
                        i.color = bg; 
                    //}
                    e.type = UIColor.Bg;
                }
            }
            catch
            {

            }
        }
        foreach (Text t in texts)
        {
            try
            {
                UiElement e;
                if (t.gameObject.TryGetComponent<UiElement>(out e))
                {
                    if (e.type == UIColor.Bg) t.color = bg;
                    else t.color = fg;
                }
                else
                {
                    t.gameObject.AddComponent<UiElement>();
                    t.gameObject.GetComponent<UiElement>().type = UIColor.Fg;
                    t.color = fg;
                }
            }
            catch { }
        }
    }
    public void StyleUIChanger()
    {
        imgs = new List<Image>();
        texts = new List<Text>();
        var objs = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in objs)
        {
            var img = canvas.gameObject.GetComponentsInChildren<Image>();
            var txt = canvas.gameObject.GetComponentsInChildren<Text>();
            foreach (Image i in img)
            {
                imgs.Add(i);
            }
            foreach (Text t in txt)
            {
                texts.Add(t);
            }
        }
    }
    public void LangChanger()
    {
        if (stDrop != null)
        {
            gameData.style = stDrop.value;

        }
        if (lgDrop != null)
        {
            gameData.lg = curlg;

        }
        Font hfont;
        switch (curlg)
        {
            case 0: 
                { 
                    LocalisationSystem.language = LocalisationSystem.Language.English; 
                    hfont = Resources.Load<Font>("Font/Ho8Bit");
                    break; 
                }
            case 1: 
                {
                    LocalisationSystem.language = LocalisationSystem.Language.Russian;
                    hfont = Resources.Load<Font>("Font/pixcyr");
                    break;
                }
            default:
                hfont = Resources.Load<Font>("Font/pixcyr");
                break;
        }
        foreach(Text tt in txt)
        {
            if (tt.gameObject.tag == "header")
            {
                tt.font = hfont;
            }
            tt.text = LocalisationSystem.TryGetLocalisedValue(tt.name);
        }
        qaDrop.ClearOptions();
        List<string> options = new List<string>();
        for (int i=0;i<4;i++)
        {
            options.Add(LocalisationSystem.GetLocalisedValue("q" + i));
        }
        qaDrop.AddOptions(options);
        qaDrop.value = gameData.q;
        qaDrop.RefreshShownValue();
        resDrop.RefreshShownValue();
        if (lgDrop != null)
        {
            lgDrop.RefreshShownValue();
        }
        if (stDrop != null)
        {
            stDrop.ClearOptions();
            List<string> o = new List<string>();
            for (int i = 0; i < DataManager.instance.palettes.Count; i++)
            {
                o.Add(LocalisationSystem.GetLocalisedValue("st" + i));
            }
            stDrop.AddOptions(o);
            stDrop.value = gameData.style;
            stDrop.RefreshShownValue();
        }
        if (slots != null)
        {
            int j = 0;
            foreach (GameObject slot in slots)
            {
                if (j == 4) j = 0;
                if (DataManager.saveSlots[j].isEmpty)
                {
                    string s = LocalisationSystem.TryGetLocalisedValue(j + "slot");
                    Debug.Log(s);
                    slot.GetComponentInChildren<Text>().text = s;
                }
                else slot.GetComponentInChildren<Text>().text = DataManager.saveSlots[j].text;
                j++;
            }
        }
    }
    public void SetStyle(int i)
    {
        bg = DataManager.instance.palettes[i].bg.color;
        fg = DataManager.instance.palettes[i].fg.color;
        mat.SetColor("_BG", bg);
        mat.SetColor("_FG", fg);
        ChangeUIColor(bg,fg);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("master", Mathf.Log10(volume)*20);
        currentVolume = volume;
    }
    public void PlaySfxSound()
    {
        var gm = GameObject.Find("options");
        if (gm == null) return;
        sfx.gameObject.SetActive(true);
        sfx.Play();
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        currentVolume = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effects", Mathf.Log10(volume) * 20);
        currentVolume = volume;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetTxtSpeed(float value)
    {
        Player.instance.GetComponent<Fungus.Character>().SetSayDialog.GetComponent<Fungus.Writer>().WritingSpeed = value*10f;
    }
    public void ApplySettings()
    {
        gameData.vol = master.value;
        gameData.vol1 = music.value;
        gameData.vol2 = effects.value;
        audioMixer.GetFloat("master", out float vol);
        gameData.master_vol = vol;
        audioMixer.GetFloat("music", out float vol2);
        gameData.music_vol = vol2;
        audioMixer.GetFloat("effects", out float vol3);
        gameData.effects_vol = vol3;
        gameData.lg = curlg;
        gameData.q = qaDrop.value;
        gameData.fc = fullToggle.isOn;
        gameData.res = resDrop.value;
        try
        {
            gameData.txt_speed = txt_speed.value;
            gameData.sens = sens.value;
            gameData.lg = lgDrop.value;
        }
        catch
        {

        }
        gameData.ForceSerialization();
        LangChanger();
    }
    public void ClearSlot()
    {
        if (selectedSlot != -1)
        {
            SaveSystem.ClearSlot(selectedSlot);
            DataManager.saveSlots[selectedSlot].Clear();
            // DataManager.saveSlots[selectedSlot].text = DataManager.instance.text[selectedSlot]; 
            slots[selectedSlot].GetComponentInChildren<Text>().text = LocalisationSystem.GetLocalisedValue(selectedSlot + "slot");
            //DataManager.instance.text[selectedSlot];
            slots[selectedSlot+4].GetComponentInChildren<Text>().text = LocalisationSystem.GetLocalisedValue(selectedSlot + "slot");
            //DataManager.instance.text[selectedSlot];
        }
    }
    public void LoadGame()
    {
            if (selectedSlot != -1)
            {
                DataManager.instance.selectedSlot = selectedSlot;
                gameData.cur_slot = selectedSlot;
                DataManager.instance.LoadGame();
            }
    }
    public int selectedSlot = -1;
    public void SelectSlot(GameObject gm)
    {
        selectedSlot = int.Parse(gm.name);
        gameData.cur_slot = selectedSlot;
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
    public void ResetSettings()
    {
        gameData = new GameData();
        UpdateData();
    }
    public void SelectedUI(Transform button)
    {
        //icon.transform.position =new Vector3(icon.transform.position.x, button.position.y,0);
        button.GetComponent<Image>().color = fg;
        button.GetComponentInChildren<Text>().color =bg;
        
    }
    public void ExitUI(GameObject button)
    {
        button.GetComponent<Image>().color = bg;
        button.GetComponentInChildren<Text>().color = fg;
    }
    public void SelectButton(GameObject gm)
    {
        gm.GetComponent<Image>().color = Color.white;
        gm.GetComponentInChildren<Text>().color = Color.black;
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
