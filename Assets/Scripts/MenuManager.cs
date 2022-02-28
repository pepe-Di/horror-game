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
    public Slider master, music, effects, txt_speed;
    private GameObject menu;
    private GameObject _mainCamera;
    //public GameObject icon;
    public List<GameObject> buttons;
    public List<GameObject> slots;
    public List<string> lg = new List<string> { "en", "ru"  };
    private int curlg=0;
    public Material mat;
    public Button lgButton;
    // Start is called before the first frame update
    private void Awake()
    {
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
        audio_ = GetComponent<AudioSource>();
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();
        List<string> options = new List<string>();
        int curRes = 0;
        for(int i=0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width&& resolutions[i].height == Screen.currentResolution.height)
            {
                curRes = i;
                gameData.res = i;
            }
        }
        resDrop.AddOptions(options);
        resDrop.value = curRes;
        resDrop.RefreshShownValue();
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
        int j = 0;
        if (slots != null){
            foreach (GameObject slot in slots)
            {
                if (j == 4) j = 0;
                slot.name = j.ToString();
                slot.GetComponentInChildren<Text>().text = DataManager.saveSlots[j].text;
                j++;
            }
        }
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
        if (lgButton != null)
        {
            lgButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/" + lg[curlg]);
        }
        if (stDrop != null)
        {
            stDrop.value = gameData.style;
        }
        if (lgDrop != null)
        {
            lgDrop.value = gameData.lg;
        }
        if (txt_speed != null)
        {
            txt_speed.value = gameData.sens;
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
    }
    public void LangChanger()
    {
        gameData.lg = curlg;
        Dictionary<string, string> d = DataManager.dictionary[lg[curlg]]; 
        Font f = Resources.Load<Font>("Font/" + d["menu_font"]);
        int size = System.Int32.Parse(d["menu_size"]);
        //if (buttons != null)
        //{
        //    switch (curlg)
        //    {
        //        case 0:
        //            {
        //                break;
        //            }
        //        case 1:
        //            {
                        
        //                break;
        //            }
        //    }
        //    //int i = 0;
            
        //    foreach (GameObject b in buttons)
        //    {
        //        Text t = b.GetComponentInChildren<Text>();
        //        t.font = f;
        //        t.fontSize = size;
        //        //t.text = d["menu" + i];
        //       // i++;
        //    }
        //}
        if (panels != null)
        {
            foreach(GameObject o in panels)
            {
                o.SetActive(true);
            }
        }
        var txt = GameObject.FindObjectsOfType<Text>();
        foreach(Text tt in txt)
        {
            if (d.ContainsKey(tt.name))
            {
                tt.text = d[tt.name];
                if (tt.gameObject.tag == "header")
                {
                    tt.font = f;
                    tt.fontSize = size;
                }
            }
        }
        if (panels != null)
        {
            foreach (GameObject o in panels)
            {
                o.SetActive(false);
            }
        }
    }
    public void SetStyle(int i)
    {
        //Color[] c = new Color[2];
        //c[0] = Color.black;
        //c[1] = Color.red;
        //mat.SetColorArray("",c);
        switch (i) 
        {
            case 0:
                {
                    mat.SetColor("_BG", Color.black);
                    mat.SetColor("_FG", Color.white);break;
                }
            case 1:
                {
                    mat.SetColor("_BG", Color.blue);
                    mat.SetColor("_FG", Color.white);
                    break; }
            case 2:
                {
                    mat.SetColor("_BG", Color.red);
                    mat.SetColor("_FG", Color.yellow);
                    break;
                }
        }
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
        gameData.ForceSerialization();
    }
    public void ClearSlot()
    {
        if (selectedSlot != -1)
        {
            SaveSystem.ClearSlot(selectedSlot);
            DataManager.saveSlots[selectedSlot].Clear();
            DataManager.saveSlots[selectedSlot].text = DataManager.instance.text[selectedSlot];
            slots[selectedSlot].GetComponentInChildren<Text>().text = DataManager.instance.text[selectedSlot];
            slots[selectedSlot+4].GetComponentInChildren<Text>().text = DataManager.instance.text[selectedSlot];
        }
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
    public void ResetSettings()
    {
        gameData = new GameData();
        UpdateData();
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
