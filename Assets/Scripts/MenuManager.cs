using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class MenuManager : MonoBehaviour
{
    public AudioSource sfx;
    private AudioSource audio_;
    public GameData gameData;
    public AudioMixer audioMixer;
    float currentVolume;
    Resolution[] resolutions;
    public Dropdown resDrop;
    public Dropdown qaDrop;
    public Toggle fullToggle;
    public Slider master,music,effects;
    private GameObject menu;
    private GameObject _mainCamera;
    public GameObject icon;
    public List<GameObject> buttons;
    public List<GameObject> slots;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        audio_ = GetComponent<AudioSource>();
        qaDrop.value = QualitySettings.GetQualityLevel();
        fullToggle.isOn = Screen.fullScreen;
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();
        audioMixer.SetFloat("master", gameData.master_vol);
        audioMixer.SetFloat("music", gameData.music_vol);
        audioMixer.SetFloat("effects", gameData.effects_vol);
        master.value = gameData.vol; 
        music.value = gameData.vol1; 
        effects.value = gameData.vol2;
        List<string> options = new List<string>();
        int curRes = 0;
        for(int i=0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width&& resolutions[i].height == Screen.currentResolution.height)
            {
                curRes = i;
            }
        }
        resDrop.AddOptions(options);
        resDrop.value = curRes;
        resDrop.RefreshShownValue();
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
            slot.GetComponentInChildren<Text>().text = DataManager.saveSlots[j].text;
            j++;
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
