using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GameObject menu;
    private GameObject _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        menu = GameObject.Find("GameMenu");
        //menu.SetActive(false);
    }

    public void OnStart()
    {
        SceneManager.LoadScene("1", LoadSceneMode.Single);
       // _mainCamera.SetActive(false);
      //  menu.SetActive(false);
    }
    public void OnExit()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
