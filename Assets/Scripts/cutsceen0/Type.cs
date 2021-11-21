using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Type : MonoBehaviour
{
    public Text text;
    public char[] lts;
    Typer typer;
    // Start is called before the first frame update
    void Awake()
    {
        typer = FindObjectOfType<Typer>();
        text = GetComponent<Text>();
        string txt = text.text.ToString();
        lts = txt.ToCharArray();
        text.text = "";
    }
}
