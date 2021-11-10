using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switched { get; set; }
    public List<GameObject> lamps = new List<GameObject>();
    private List<Light> lights = new List<Light>();
    void Awake()
    {
        switched = true; 
        foreach (GameObject lamp in lamps)
        {
            lights.Add(lamp.GetComponentInChildren<Light>());
        }
    }
    public void Switching()
    {
        switched = !switched;
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(switched);
        }
        foreach (GameObject lamp in lamps)
        {
            lamp.gameObject.SetActive(switched);
        }
    }
}
