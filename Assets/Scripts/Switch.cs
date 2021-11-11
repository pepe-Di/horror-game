using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switched { get; set; }
    AudioSource audioSource;
    public List<GameObject> lamps = new List<GameObject>();
    private List<Light> lights = new List<Light>();
    private float volume = 0.3f;
    public Vector3 on_pos, off_pos;
    void Awake()
    {
        switched = true;
        audioSource = gameObject.AddComponent<AudioSource>();
        foreach (GameObject lamp in lamps)
        {
            lights.Add(lamp.GetComponentInChildren<Light>());
        }
        Transform t = gameObject.GetComponent<Transform>(); 
        on_pos = new Vector3(0,0,0);
        off_pos = new Vector3(0, 0, 0);
        on_pos = t.position;
        off_pos = t.position;
        off_pos+= new Vector3(0,-0.1f,0);
    }
    bool C_running = false;
    public void Switching()
    {
        if (!C_running) { StartCoroutine(Switch_()); }
    }
    IEnumerator Switch_()
    {
        C_running = true;
        switched = !switched;
        if (switched) 
        { 
            audioSource.PlayOneShot(Resources.Load("Sounds/click") as AudioClip, volume);
            while (gameObject.transform.position !=on_pos)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, on_pos, 10f*Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else 
        {
            audioSource.PlayOneShot(Resources.Load("Sounds/click2") as AudioClip, volume);
            while (gameObject.transform.position != off_pos)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, off_pos, 10f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(switched);
        }
        foreach (GameObject lamp in lamps)
        {
            lamp.gameObject.SetActive(switched);
        }
        yield return new WaitForSeconds(0.1f); 
        C_running = false;
    }
}
