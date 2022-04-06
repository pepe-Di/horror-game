using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public List<DropDown> drops;
    public List<GameObject> panels;
    public AudioSource sfx; 
    private AudioSource audio_;
    public GameData gameData;
    public AudioMixer audioMixer;
    float currentVolume;
    public DropDown lg_drop;
    Resolution[] resolutions;
    public Dropdown resDrop;
    public Dropdown qaDrop;
    public Dropdown lgDrop;
    public Dropdown stDrop;
    public Dropdown frameDrop;
    public Toggle partToggle;
    public Toggle fullToggle;
    public Toggle frameToggle;
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
    public bool frame_mode = true, particles_on;
    Text[] txt;
    List<Image> imgs;
    List<Text> texts;
    Color bg, fg;
    public int vsync_count;
    public Particles prts;
    public static MenuManager instance;
    public Fungus.Localization localization;
    Frame frame;GameObject player; Fungus.Character character;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1f;
        instance = this;
    }
    void createDrops()
    {
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        drops[1].ClearOptions();
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
        drops[1].AddOptions(options);
        drops[1].Value = curRes;
        drops[1].RefreshShownValue();
    }
    void Start()
    {
        if(GameManager.instance!=null)
        {
            player = GameManager.instance.Player;
            character = player.GetComponent<Fungus.Character>();
        }
        frame = FindObjectOfType<Frame>();
        try
        {
            gameData.SetOptions(new GameData(SaveSystem.LoadOptions()));
        }
        catch{}

        vsync_count = QualitySettings.vSyncCount;
        if (gameData == null)
        {
            gameData = new GameData();
            gameData.q = QualitySettings.GetQualityLevel();
            Screen.fullScreen =gameData.fc;
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
        prts = GameObject.FindObjectOfType<Particles>();
        particles_on = gameData.particles;
        frame_mode = gameData.frame_mode;
        FrameRateDrop(gameData.frame_limit);
        StyleUIChanger();
        txt = GameObject.FindObjectsOfType<Text>();
        if (panels != null)
        {
            foreach (GameObject o in panels)
            {
                o.SetActive(false);
            }
        }
       // if(!frame_mode) EventController.instance.StartFrameEvent(frame_mode);
        audio_ = GetComponent<AudioSource>();
        createDrops();
        if (stDrop != null)
        {
            SetStyle(gameData.style);
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
           if(frameToggle!=null) frameToggle.isOn = gameData.frame_mode;
            DataManager.instance.curlg = gameData.lg;
        }
        catch { }
        if (drops[2] != null)
        {
            drops[2].Value = gameData.frame_limit;
        }
        if (lgButton != null)
        {
            lgButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/" + lg[curlg]);
        }
        if (lgDrop != null)
        {
           // lgDrop.value = gameData.lg;
        }
        drops[4].Value= gameData.lg;
        if (lg_drop != null)
        {
            //lg_drop.Value = gameData.lg;
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
        
            drops[3].Value = gameData.style;
            drops[3].RefreshShownValue();
        
        if(partToggle!= null){
            partToggle.isOn = gameData.particles;
        }
        LangChanger();
        drops[0].Value = gameData.q;
        fullToggle.isOn = gameData.fc;
        drops[1].value = gameData.res;
    }
    public void FrameRateDrop(int i)
    {
        QualitySettings.vSyncCount = 0;
        int value = 60;
        switch (i)
        {
            case 0: value = 30; break;
            case 1: value = 60; break;
            case 2: value = 144; break;
            case 3: value = 400; break;
            case 4: QualitySettings.vSyncCount = vsync_count; break;
        }
        Application.targetFrameRate = value;
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
        Debug.Log("setlg " + i);
        curlg = i;
    }
    public void ChangeSens(float value)
    {
        Player.instance.GetComponent<MouseLook>().SensChange(value);
    }
    public void ChangeUIColor(Color bg, Color fg)
    {Debug.Log("changeuicol");
        foreach (Image i in imgs)
        {
            try
            {
                UiElement el;
                if (i.gameObject.TryGetComponent<UiElement>(out el)) 
                {
                    if (el.type == UIColor.Bg) 
                    {
                        if (el.alpha) 
                            i.color = bg;
                        else i.color = new Color(bg.r, bg.g, bg.b, 0f);
                    }
                    else i.color = fg;
                }
                else
                {
                    i.gameObject.AddComponent<UiElement>();
                    UiElement e = i.gameObject.GetComponent<UiElement>();
                    Button b;
                    if (i.gameObject.TryGetComponent<Button>(out b))
                    {
                        i.color = new Color(bg.r, bg.g, bg.b, 0f);
                        e.alpha = false;
                    }
                    else
                    {
                        i.color = bg;
                    }
                    e.type = UIColor.Bg;
                }
            }
            catch
            {

            }
        }
                if(character!=null){
                    character.NameColor = fg;
                    Debug.Log("dsklgjkgldsfglkjdlfskgjkdfg");
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
        //if (stDrop != null)
      //  {
            gameData.style = drops[3].Value;

      //  }
      //  if (lgDrop != null)
      //  {
       //     gameData.lg = curlg;

      //  }
       // if (lg_drop != null)
      //  {
           // gameData.lg = drops[4].Value;

      //  }
        Font hfont;
        switch (gameData.lg)
        {
            case 0: 
                { 
                    LocalisationSystem.language = LocalisationSystem.Language.English; 
                    hfont = Resources.Load<Font>("Font/Ho8Bit");
                    if(localization!=null) localization.SetActiveLanguage("en");
                    break; 
                }
            case 1: 
                {
                    LocalisationSystem.language = LocalisationSystem.Language.Russian;
                    hfont = Resources.Load<Font>("Font/pixcyr");
                    if(localization!=null) localization.SetActiveLanguage("Standart");
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
           if(tt.name!="ignore") tt.text = LocalisationSystem.TryGetLocalisedValue(tt.name);
        }
        if (drops[4] != null)
        {
            drops[4].ClearOptions();
            List<string> opt = new List<string>();
        for (int i=0;i<2;i++)
        {
            opt.Add(LocalisationSystem.GetLocalisedValue("lg" + i));
        }
        drops[4].AddOptions(opt);
        drops[4].Value = gameData.lg;
        drops[4].RefreshShownValue();

        }
        drops[0].ClearOptions();
        List<string> options = new List<string>();
        for (int i=0;i<4;i++)
        {
            options.Add(LocalisationSystem.GetLocalisedValue("q" + i));
        }
        drops[0].AddOptions(options);
        drops[0].Value = gameData.q;
        drops[0].RefreshShownValue();
        drops[1].RefreshShownValue();
        drops[4].RefreshShownValue();
       // if (stDrop != null)
       // {
            drops[3].ClearOptions();
            List<string> o = new List<string>();
            for (int i = 0; i < DataManager.instance.palettes.Count; i++)
            {
                o.Add(LocalisationSystem.TryGetLocalisedValue("st" + i));
            }
            drops[3].AddOptions(o);
            drops[3].Value = gameData.style;
            drops[3].RefreshShownValue();
       // }
        if (drops[2] != null)
        {
            drops[2].ClearOptions();
            List<string> ss = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                ss.Add(LocalisationSystem.TryGetLocalisedValue("fps" + i));
            }
            drops[2].AddOptions(ss);
            drops[2].Value = gameData.frame_limit;
            drops[2].RefreshShownValue();
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
                   // Debug.Log(s);
                    slot.GetComponentInChildren<Text>().text = s;
                }
                else slot.GetComponentInChildren<Text>().text = DataManager.saveSlots[j].text;
                j++;
            }
        }
    }
    public void SetStyle(int i)
    {
        DataManager.instance.selectedPalette = i;
        Debug.Log(DataManager.instance.palettes[i].bg.Name);
        bg = DataManager.instance.palettes[i].bg.color;
        fg = DataManager.instance.palettes[i].fg.color;
        mat.SetColor("_BG", bg);
        mat.SetColor("_FG", fg);
        ChangeUIColor(bg,fg);
        if(prts!=null) {prts.enabled = false;
        var objs = GameObject.FindObjectsOfType<Particle>();
        foreach(Particle o in objs) 
        {
            Destroy(o.gameObject);
        }
        if(particles_on) prts.enabled = true;}
    }
    

    public void FrameModeToggle()
    {
        frame_mode = frameToggle.isOn;
        EventController.instance.StartFrameEvent(frame_mode);
        
        if(frame!=null)frame.gameObject.SetActive(frame_mode);
        Debug.Log("frame_toggle "+frame_mode);
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
    public void SetParticles(bool isOn)
    {
        particles_on = isOn;
        prts.enabled = isOn;
        if(isOn==false){
           var objs = GameObject.FindObjectsOfType<Particle>();
        foreach(Particle o in objs) 
        {
            Destroy(o.gameObject);
        }
        }
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
        gameData.q = drops[0].Value;
        gameData.fc = fullToggle.isOn;
        gameData.res = drops[1].Value;
        gameData.lg = drops[4].Value;
        gameData.style = drops[3].Value;
        if(partToggle!=null) gameData.particles = particles_on;
        if (drops[2] != null) gameData.frame_limit = drops[2].Value;
           if (txt_speed != null) gameData.txt_speed = txt_speed.value;
           if (sens != null)  gameData.sens = sens.value;
           if (frameToggle != null)  {gameData.frame_mode = frame_mode;}
        gameData.ForceSerialization();
        SaveSystem.SaveOptions(new Options(gameData));
        LangChanger();
            Debug.Log(" frame_mode  "+frame_mode);
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
   public GameObject cartridge;
    float Xdistance = -130f, Ydistance=0f;
    public void OnPointerEnterSlot(GameObject gm)
    {
        cartridge = Instantiate(Resources.Load<GameObject>("ui/gif/cartridge"),gm.transform);
        cartridge.transform.localPosition = new Vector2(Xdistance, Ydistance);
        Debug.Log(cartridge.name);
    }
    public void SelectSlot(GameObject gm)
    {
        selectedSlot = int.Parse(gm.name);
        gameData.cur_slot = selectedSlot;
        for (int i = 0; i < DataManager.saveSlots.Length; i++)
        {
            if (i == selectedSlot) DataManager.saveSlots[i].selected = true;
            else DataManager.saveSlots[i].selected = false;
        }
        int j = 0,k=0;
        Slot s;
        if (cartridge != null) Destroy(cartridge);
        Debug.Log(cartridge);
        foreach (GameObject slot in slots)
        {
           if (j == 4) j = 0;
           if (slot.TryGetComponent<Slot>(out s))
           {
                if (j == selectedSlot) 
                {
                    if (s.isEnabled)
                    {
                        s.selected = true; OnPointerEnterSlot(slot); }
                    else
                    {
                        s.selected = false; ExitUI(slot);
                    }
                }
                else
                {
                    s.selected = false;
                    //if (cartridge != null) Destroy(cartridge);
                    ExitUI(slot);
                }
           }
            j++;k++;
        }
        gm.GetComponent<Slot>().selected = true;
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
        gameData.Clear();
        UpdateData();
    }
    public void SelectedUI(Transform button)
    {
        //icon.transform.position =new Vector3(icon.transform.position.x, button.position.y,0);
        button.GetComponent<Image>().color = fg;
        button.GetComponentInChildren<Text>().color =bg;
        
    }
    public void ExitUiSlot(GameObject button)
    {
        Slot slot;
        if (button.TryGetComponent<Slot>(out slot))
        {
            if (slot.selected)
            {
                return;
            }
        }
        button.GetComponent<Image>().color = new Color(bg.r, bg.g, bg.b, 0f);
        button.GetComponentInChildren<Text>().color = fg;
    }
    public void ExitUI(GameObject button)
    {
         button.GetComponent<Image>().color = new Color(bg.r, bg.g, bg.b, 0f);
         button.GetComponentInChildren<Text>().color = fg;
    }
    public void SelectButton(GameObject gm)
    {
        gm.GetComponent<Image>().color = new Color(bg.r, bg.g, bg.b, 0f);
        gm.GetComponentInChildren<Text>().color = fg;
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
