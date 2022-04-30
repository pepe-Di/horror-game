using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public EnemyController enemy;
    public LevelLoader lv;
    public GameObject options;
    public static GameManager instance;
    public StarterAssetsInputs _input;
    public GameObject Player;
    public Player player_;
    public GameObject menu;
    public GameObject _mainCamera;
    public InventoryUI inv;
    public Fungus.Flowchart flowchart;
    public List<Transform> itemsPos;
    public List<QuestTrigger> qtriggers;
    public bool loaded=false;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EventController.instance.SayEvent+=StartDialogue;
        //flowchart.ExecuteBlock("0");
      //  DontDestroyOnLoad(instance.gameObject);
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        player_ = Player.GetComponent<Player>();
        //  _input = FindObjectOfType<StarterAssetsInputs>();
        // lv = FindObjectOfType<LevelLoader>();
        // menu = GameObject.Find("ui");
        // Player = GameObject.Find("Player");
        //  Player.SetActive(true); 
        
        menu.SetActive(false); 
           //SpawnAllItems();
         if(PlayerPrefs.HasKey("name"))
        {
            SpawnAllItems();
            //StartDialogue("0");
        loaded=true;
        }
        else 
        {
                LoadData(); 
                var o = FindObjectsOfType<QuestTrigger>();
                foreach(QuestTrigger t in o){
                qtriggers.Add(t);
                }
                StartCoroutine(DelQTriggers());
            }
        try {
           // LoadData();
            //if (DataManager.instance.loaded) 
            //{ 
            //    LoadData(); 
            //    DataManager.instance.loaded = false; 
            //} 
        }
        catch { Debug.Log("catch"); }
        
    }
    IEnumerator DelQTriggers(){
        yield return new WaitUntil(()=>player_.loaded);
        foreach(QuestTrigger trigger in qtriggers)
        {
             var q = player_.quests.Where(c=>c.id==trigger.id).FirstOrDefault();
             var fq = player_.finished_quests.Where(c=>c.id==trigger.id).FirstOrDefault();
             if(q!=null||fq!=null) Destroy(trigger.gameObject);
        }
        loaded=true;
    }
   public void StartDialogue(string blockName){
       flowchart.ExecuteBlock(blockName);
   }
    // Update is called once per frame
    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void SaveData()
    {
        DataManager.instance.SaveData(); 
    }
    public void LoadData()
    {
        SaveSlot data = SaveSystem.LoadPlayer();
        player_.LoadData(data.playerData);
        enemy.LoadEnemy(data.aiData);
        SpawnItems();
       // inv.UpdateData(this);
    }
    public void SpawnAllItems(){
        foreach(Transform pos in itemsPos)
        {
            ItemPos ip = pos.GetComponent<ItemPos>();
            GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/"+ip.Name),pos);
            gm.name = ip.Name;
            if(ip.look)gm.tag="Look";
            gm.transform.localPosition = new Vector3(0,0,0);
            gm.transform.localRotation = Quaternion.Euler(0,0,0);
        }
    }
    public void SpawnItems(){
        foreach(Transform pos in itemsPos)
        {
            ItemPos ip = pos.GetComponent<ItemPos>();

            var item = player_.items.Where(c=>c.GetGmName()==ip.Name&&c.Id==ip.index).FirstOrDefault();
            var it = player_.used_items.Where(c=>c.GetGmName()==ip.Name&&c.Id==ip.index).FirstOrDefault();
            if(it!=null||item!=null) 
            {
                Debug.Log("item found");
            }
            else{
                GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/"+ip.Name),pos);
                gm.name = ip.Name;
                gm.transform.localPosition = new Vector3(0,0,0);
                gm.transform.localRotation = Quaternion.Euler(0,0,0);
            }
        }
    }
    public void Test()
    {
        Debug.Log("a");
    }
    public void ContinueButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _input.locked_input = false;
        _input.cursorInputForLook = true;
        //Player.GetComponent<CharacterController>().enabled = true;
       // Player.GetComponent<MouseLook>().enabled = true;
       MenuManager.instance.UpdateData();
        menu.SetActive(false); 
        gamePaused = false;
        Time.timeScale = 1f;
        EventController.instance.ChangeStateEvent(State.Idle);
    }
    public void BackToMenu() //and save
    {
        //Time.timeScale = 1f;
        Debug.Log("cur_slot " + DataManager.instance.gameData.cur_slot);
        SaveSystem.SavePlayer(instance.Player.GetComponent<Player>(),instance.enemy);
        lv.LoadLevel(0);

    }
    bool gamePaused = false;
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        if (_input.esc)
        {
    //        Debug.Log(Time.timeScale);
    //        Debug.Log(C_running);
            if (!C_running)
            StartCoroutine(OpenMenu());
            //Transition.LoadScene("0");
            //if (menu.activeSelf)
            //{
            //    Time.timeScale = 0;
            //}
            //else Time.timeScale = 1;
        }
    }
    public IEnumerator Esc()
    {
        C_running = true;
        yield return new WaitForSecondsRealtime(0.2f); C_running = false;
    }
    bool C_running = false;
    public IEnumerator OpenMenu()
    {
        Debug.Log("q");
        C_running = true;
        if (!menu.activeSelf)
        {
            if (player_.selectedItem != null)
            {
                player_.DeselectItem();
            }
        EventController.instance.ChangeStateEvent(State.Pause);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.cursorInputForLook = false;
            _input.locked_input = true;
            //Player.GetComponent<CharacterController>().enabled = false;
            //Player.GetComponent<MouseLook>().enabled = false;
            menu.SetActive(true); gamePaused = true;
        }
        else
        {
            ContinueButton(); gamePaused = false;
        }
        yield return new WaitForSecondsRealtime(0.2f); 
        if (!gamePaused)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
          C_running = false;
    }
}
