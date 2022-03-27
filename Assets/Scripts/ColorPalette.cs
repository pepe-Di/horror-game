using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ColorPalette
{
    string name; 
    public string Name { get { return name; } }
    public Color_ bg, fg;
    public ColorPalette(string name, Color_ bg, Color_ fg)
    {
        this.name = name;
        this.bg = bg;
        this.fg = fg;
    }
    public ColorPalette(string name, float r, float g, float b, float rx, float gx, float bx)
    {
        this.name = name;
        bg = new Color_(r, g, b);
        fg = new Color_(rx, gx, bx);
    }
}
[System.Serializable]
public class Color_
{
    string name;
    public string Name { get { return name; } }
    public Color color;
    float a = 255f;
    public Color_(string name, float r, float g, float b, float a) 
    {
        this.name = name;
        color = new Color(r, g, b, a);
    }
    public Color_(float r, float g, float b, float a)
    {
        color = new Color(r, g, b, a);
    }
    public Color_(float r, float g, float b)
    {
        color = new Color(r, g, b, a);
    }
    public Color_(string name, float r, float g, float b)
    {
        this.name = name;
        color = new Color(r, g, b, a);
    }
}