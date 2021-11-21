using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelLoader lv;
    public static GameManager instance;
    public StarterAssetsInputs _input;
    public GameObject Player;
    public GameObject menu; 
    private GameObject _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        _input = FindObjectOfType<StarterAssetsInputs>();
        lv = FindObjectOfType<LevelLoader>();
        // menu = GameObject.Find("ui");
        Player = GameObject.Find("Player");
      //  Player.SetActive(true); 
        menu.SetActive(false);
    }
    // Update is called once per frame
    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void SaveData()
    {
        SaveSystem.SavePlayer(Player.GetComponent<Player>());
    }
    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        lv.LoadLevel(data.sceneIndex);
        Player.GetComponent<Player>().LoadData(data);
    }
    public void ContinueButton()
    {
        Debug.Log("a");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _input.cursorInputForLook = true; 
        Player.GetComponent<CharacterController>().enabled = true;
        // Player.SetActive(true);
        menu.SetActive(false);
    }
    public void BackToMenu() //and save
    {
        SaveSystem.SavePlayer(instance.Player.GetComponent<Player>());
        lv.LoadLevel(0);

    }
    void Update()
    {
        if (_input != null)
        {
             if (_input.esc&&!C_running)
             {
                StartCoroutine(OpenMenu());
                //Transition.LoadScene("0");
             }
            // else if (Input.GetKeyDown(KeyCode.I))
            //{
               
            //}
        }
    }
    bool C_running = false;
    IEnumerator OpenMenu()
    {
        C_running = true;
        if (!menu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.cursorInputForLook = false;
            Player.GetComponent<CharacterController>().enabled = false;
            //Player.SetActive(false);
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
