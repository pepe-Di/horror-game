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
    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        menu = GameObject.Find("GameMenu");
        //menu.SetActive(false);
       // SceneManager.GetActiveScene().name
    }
    public void SelectedUI(Transform button)
    {
        //icon.transform.position =new Vector3(icon.transform.position.x, button.position.y,0);
        button.GetComponent<Image>().color = Color.white;
        button.GetComponentInChildren<Text>().color = Color.black;
    }
    public void ExitUI(GameObject button)
    {
        button.GetComponent<Image>().color = Color.black;
        button.GetComponentInChildren<Text>().color = Color.white;
    }
    public void OnStart()
    {
        //SceneManager.LoadScene("1", LoadSceneMode.Single);
        Transition.LoadScene("3");
        // _mainCamera.SetActive(false);
      //  menu.SetActive(false);
    }
    public void PressButton(GameObject gm)
    {
        StartCoroutine(Skip(gm));
    }
    public IEnumerator Skip(GameObject gm)
    {
        gm.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(0.3f);
        gm.SetActive(false);
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
