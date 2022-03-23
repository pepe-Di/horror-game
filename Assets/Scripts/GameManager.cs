using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelLoader lv;
    public static GameManager instance;
    public StarterAssetsInputs _input;
    public GameObject Player;
    public Player player_;
    public GameObject menu;
    public GameObject _mainCamera;
    public InventoryUI inv;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
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
        try { 
            if (DataManager.instance.loaded) 
            { 
                LoadData(); 
                DataManager.instance.loaded = false; 
            } 
        }
        catch { }
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
       // inv.UpdateData(this);
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
        Player.GetComponent<MouseLook>().enabled = true;
        menu.SetActive(false);
    }
    public void BackToMenu() //and save
    {
        SaveSystem.SavePlayer(instance.Player.GetComponent<Player>());
        lv.LoadLevel(0);

    }
    void Update()
    {
         if (_input.esc&&!C_running)
         {
             StartCoroutine(OpenMenu());
                //Transition.LoadScene("0");
                //if (menu.activeSelf)
                //{
                //    Time.timeScale = 0;
                //}
                //else Time.timeScale = 1;
         }
            // else if (Input.GetKeyDown(KeyCode.I))
            //{
               
            //}
    }
    bool C_running = false;
    public IEnumerator OpenMenu()
    {
        C_running = true;
        if (!menu.activeSelf)
        {
            if (player_.selectedItem != null)
            {
                player_.DeselectItem();
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.cursorInputForLook = false;
            _input.locked_input = true;
            //Player.GetComponent<CharacterController>().enabled = false;
            Player.GetComponent<MouseLook>().enabled = false;
            menu.SetActive(true);
        }
        else
        {
            ContinueButton();
        }
        yield return new WaitForSeconds(0.2f);
        C_running = false;
    }
}
