using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject menu;
    private GameObject _mainCamera;
    public GameObject icon;
    public List<GameObject> buttons;
    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        menu = GameObject.Find("GameMenu");
       // foreach
        //menu.SetActive(false);
       // SceneManager.GetActiveScene().name
    }
    public void LoadGame()
    {
        try { GameManager.instance.LoadData(); }
        catch { Debug.LogError(""); }
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
    //    button.GetComponent<Image>().color = Color.black;
    //    button.GetComponentInChildren<Text>().color = Color.white;
    }
    public void OnStart()
    {
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
    // Update is called once per frame
    void Update()
    {
    }
}
