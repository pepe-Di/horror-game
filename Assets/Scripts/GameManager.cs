using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    CharacterController cc;
    public StateController state;
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
    public int seed;
    public GameObject inv_panel, opt_pan, q_pan,menu_pan;
    public Animator blackout_animator;
    public Animator gameover_animator;
    public Animator start_animator;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance!=null) Destroy(this);
        instance = this;
    }
    public Vector3 gameover_pos,enemy_pos;
    void Start()
    {
      //  Debug.Log("START");
        EventController.instance.BlackOut+=BlackOut;
        EventController.instance.SayEvent+=StartDialogue;
        EventController.instance.GameOver+=GameOver;
        start_animator.SetBool("start",true);
       // Debug.Log("START1");
        //flowchart.ExecuteBlock("0");
      //  DontDestroyOnLoad(instance.gameObject);
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
       // Debug.Log("START2");
        player_ = Player.GetComponent<Player>();
        //player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=false;
        //StartCoroutine(Wait());
        cc= Player.GetComponent<CharacterController>();
        //  _input = FindObjectOfType<StarterAssetsInputs>();
        // lv = FindObjectOfType<LevelLoader>();
        // menu = GameObject.Find("ui");
        // Player = GameObject.Find("Player");
        //  Player.SetActive(true); 
        var iposes = FindObjectsOfType<ItemPos>();
       // Debug.Log("START3");
        itemsPos.Clear();
        foreach(ItemPos itp in iposes){
            itemsPos.Add(itp.transform);
        }
     //   Debug.Log("START4");
        menu.SetActive(false); 
      //  Debug.Log("START5");
           //SpawnAllItems();
        if(PlayerPrefs.HasKey("name"))
        //&&DataManager.instance.newGame)
        {
            Debug.Log("yas");
            gameover_pos = player_.transform.position;
            enemy_pos = enemy.transform.position;
            SpawnAllItems();
            seed = (int)System.DateTime.Now.Ticks;
            Debug.Log("seed: "+seed);
            Random.InitState(seed);
            player_.seed = seed;
            StartCoroutine(StartQ());
            //PlayerPrefs.DeleteKey("name");
            //StartDialogue("0");
           // qtriggers.Where(c=>c.id==0).FirstOrDefault().gameObject.SetActive(true);
        
        loaded=true;
        StartDialogue("Start");
        } 
        else if(!DataManager.instance.newGame)
        {
            Debug.Log("load");
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
    IEnumerator Wait(){
        yield return new WaitForSeconds(1f);
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=true;
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
       if(state.state!=State.Freeze)EventController.instance.ChangeStateEvent(State.Talk);
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
    AIdata ai_data;
    public void LoadData()
    {
        SaveSlot data = SaveSystem.LoadPlayer();
        seed = data.playerData.seed;Debug.Log("seed: "+seed);
        player_.LoadData(data.playerData);
        enemy.LoadEnemy(data.aiData);
       // ai_data = data.aiData;
        SpawnItems();
       // inv.UpdateData(this);
    }
    public AIdata GetEnemyData(){
        return ai_data;
    }
    IEnumerator StartQ(){
        yield return new WaitForSeconds(4f);
        EventController.instance.StartQEvent(0);
    }
    public void GameOver()
    {   
        Debug.Log("GameOver");
        if(player_.Hp<=0)
        {
            StartCoroutine(GameOverScreen());
        }
        else StartCoroutine(BlackOutGameOver());
    }
    IEnumerator GameOverScreen()
    {
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=false;
        SoundManager.instance.PlayBg(Bg.gameover);
        SoundManager.instance.playLock=true;
        state.state = State.Freeze; 
       // gameover_animator.gameObject.SetActive(true);
        blackout_animator.SetBool("start",true);
        yield return new WaitForSeconds(1f);
        cc.enabled = false;
        Player.transform.position = new Vector3(gameover_pos.x,gameover_pos.y,gameover_pos.z);
        enemy.transform.position = new Vector3(enemy_pos.x,enemy_pos.y,enemy_pos.z);
        cc.enabled=true;
        gameover_animator.SetBool("start",true);
        yield return new WaitForSeconds(7f);
        gmanim_end=true;ToMenu();
    }
    bool gmanim_end=false;
    IEnumerator BlackOutGameOver()
    {
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=false;
        SoundManager.instance.ResumeLastBg();
        SoundManager.instance.playLock=true;
        state.state = State.Freeze; 
        blackout_animator.SetBool("start",true);
        yield return new WaitForSeconds(1f);
        blackout_animator.SetBool("start",false);
        blackout_animator.SetBool("end",true);
        yield return new WaitForSeconds(0.5f);
        cc.enabled = false;
        Player.transform.position = new Vector3(gameover_pos.x,gameover_pos.y,gameover_pos.z);
        enemy.transform.position = new Vector3(enemy_pos.x,enemy_pos.y,enemy_pos.z);
        cc.enabled=true;
        yield return new WaitForSeconds(0.5f);
        blackout_animator.SetBool("end",false);
        state.state = State.Idle;
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=true;
        SoundManager.instance.playLock=false;
    }
    public void SpawnAllItems(){
        foreach(Transform pos in itemsPos)
        {
            ItemPos ip = pos.GetComponent<ItemPos>();
            try{
              //  Debug.Log(ip.Name + ip.index+" loaded.");
                 GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/"+ip.Name),pos);
                gm.name = ip.Name;
                if(ip.look)gm.tag="Look";
                gm.transform.localPosition = new Vector3(0,0,0);
                gm.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            catch{
                Debug.Log(ip.Name+" not found! Index: "+ip.index);
            }
           
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
                
                try{
                GameObject gm = Instantiate(Resources.Load<GameObject>("Prefs/Items/"+ip.Name),pos);
              //  Debug.Log(ip.Name + ip.index+" loaded.");
                gm.name = ip.Name;
                gm.transform.localPosition = new Vector3(0,0,0);
                gm.transform.localRotation = Quaternion.Euler(0,0,0);
                }
                catch{
                    Debug.Log(ip.Name+" not found! Index: "+ip.index);
                }
            }
        }
    }
    public void Test()
    {
        Debug.Log("a");
    }
    public void BlackOut(){
        StartCoroutine(StartBlackOut());
    }
    IEnumerator StartBlackOut(){
        state.state = State.Freeze;
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=false;
        blackout_animator.SetTrigger("start 0");
        yield return new WaitForSeconds(2f);
        blackout_animator.SetTrigger("end 0");
        state.state = State.Idle;
        player_.GetComponent<Footsteps.CharacterFootsteps>().enabled=true;
    }
    public void ContinueButton()
    {
        SoundManager.instance.ResumeLastBg();
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
    public void ToMenu(){
       if(gmanim_end) {
           CursorLock(false);
           lv.LoadLevel(0);}
    }
    bool gamePaused = false;
    private void FixedUpdate()
    {
        
    }
    Transform t;
    bool b= false;
    void Update()
    {
        if(_input.esc&&state.state == State.Freeze&&!b){
            Debug.Log("State.Freeze");
            b=true;
            //player_.GetComponent<MouseLook>().enabled = true;
           // t.position = new Vector3(0.001f,1.44f,0.011f);
           
            EventController.instance.StartCameraEvent(true);
            EventController.instance.ChangeStateEvent(State.Idle);
            EventController.instance.OffComputerUIEvent();
            EventController.instance.OffPuzzleUIEvent();
            StartCoroutine(Waiter());
        }
        else if (_input.esc&&!b)
        {
            tab=false;
           // if(state.state == State.Freeze){
               //b=true;
           // }
    //        Debug.Log(Time.timeScale);
    //        Debug.Log(C_running);
            if (!C_running){
            StartCoroutine(OpenMenu());
            }
            //Transition.LoadScene("0");
            //if (menu.activeSelf)
            //{
            //    Time.timeScale = 0;
            //}
            //else Time.timeScale = 1;
        }
        if(_input.tab&&!b){
            if (!C_running){
                tab=true;
            StartCoroutine(OpenMenu());
            }
        }
    }
    public IEnumerator Waiter(){
        Debug.Log("wait()00");
        b=true;
        yield return new WaitForSeconds(0.5f);
            CursorLock(true);
        b=false;
    }
    public IEnumerator Esc()
    {
        C_running = true;
        yield return new WaitForSecondsRealtime(0.2f); C_running = false;
    }
    public void CursorLock(bool b){
        if(b){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _input.locked_input = false;
            _input.cursorInputForLook = true;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.cursorInputForLook = false;
            _input.locked_input = true;
        }
    }
    bool C_running = false;
    bool tab=false;
    public IEnumerator OpenMenu()
    {
        C_running = true;
        SoundManager.instance.PlaySe(Se.Click2);
        SoundManager.instance.PlayAltBg(Bg.noise);
        Debug.Log("openmenu()");
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
            menu.SetActive(true);
            
            if(tab){ 
                inv_panel.SetActive(true);
                opt_pan.SetActive(false);
                q_pan.SetActive(false);
                menu_pan.SetActive(false);
            tab=false;}
             gamePaused = true;
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
