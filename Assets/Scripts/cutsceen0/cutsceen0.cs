using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutsceen0 : MonoBehaviour
{
    private GameObject panel;
    private InputField input;
    private LevelLoader lv;
    Typer typer;
    void Start()
    {
        lv = FindObjectOfType<LevelLoader>();
        typer = FindObjectOfType<Typer>();
        input = FindObjectOfType<InputField>();
        typer.Type();
    }
    private void Update()
    {
        if (Input.anyKeyDown && typer.state != TxtState.Typing)
        {
            if (input.text == "")
            {
                input.Select();
                input.ActivateInputField();
            }
            else if(!input.isFocused)
            {
                if (input.enabled) input.enabled = false;
                typer.Type();
            }
        }
        if (typer.state == TxtState.End)
        {
            typer.state = TxtState.Typing;
            PlayerPrefs.SetString("name", input.text);
            lv.LoadLevel(2);
        }
    }
    //IEnumerator LoadScene()
    //{
        
    //}
    public void OnClick()
    {
        if (input.text != ""&&typer.state!=TxtState.Typing)
        {
            typer.Type();
        }
    }
}
