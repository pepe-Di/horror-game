using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordInput : MonoBehaviour
{
    public GameObject screen, noise_screen;
    public Camera camera_;
    bool isOn=false;
    // Start is called before the first frame update
    void Start()
    {
        camera_.transform.gameObject.SetActive(false);
        screen.SetActive(false);
        noise_screen.SetActive(false);
    }
    public void On()
    {
        if(isOn) {Off(); return;}
        camera_.transform.gameObject.SetActive(true);
        screen.SetActive(true);
        noise_screen.SetActive(true);
        EventController.instance.StartCameraEvent(false);
        EventController.instance.ChangeStateEvent(State.Freeze);
        GameManager.instance.inv.GetMessage(LocalisationSystem.GetLocalisedValue("message4"));
        SoundManager.instance.PlaySe(Se.Click);
        isOn = true;
    }
    public void Off()
    {
        camera_.transform.gameObject.SetActive(false);
        screen.SetActive(false);
        noise_screen.SetActive(false);
        SoundManager.instance.PlaySe(Se.Click);
        isOn = false;
    }
    void StopUI(){
        screen.SetActive(false);
    }
    void OnEnable(){
        EventController.instance.OffComputerUI+=StopUI;
    }
}
