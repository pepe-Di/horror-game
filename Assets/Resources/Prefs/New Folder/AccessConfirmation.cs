using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccessConfirmation : MonoBehaviour
{
    public GameObject screen;
    public Text startTxt, inpTxt, endTxt;
    public InputField input;
    //public StarterAssetsInputs _input;
    bool c= false;
    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(false);
        endTxt.text = "";
        startTxt.text = "";
        inpTxt.text = "";
        input.text="";
        input.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(c){

        }
    }
    void OnEnable(){
        if(!c&&!screen.activeSelf) StartCoroutine(StartAnim());
        Debug.Log("onenable");
    }
    IEnumerator StartAnim(){
        c=true;
        yield return new WaitForSeconds(1.5f);
        string startMessage = LocalisationSystem.GetLocalisedValue("enterPass");
        for (int i = 0; i < startMessage.Length; i++) { 
            startTxt.text+=startMessage[i]; 
            SoundManager.instance.PlaySe(Se.Item);
            yield return new WaitForSeconds(0.2f);
        } 
        inpTxt.text = ">";
        SoundManager.instance.PlaySe(Se.Item);
        c= false;
        input.enabled = true;
        input.Select();
        input.onEndEdit.AddListener(delegate{GetAccess();});
    }
    void GetAccess(){
        if(input.text=="") return;
        input.enabled = false;
        if(input.text=="exile"||input.text=="EXILE"||input.text=="Exile"){
            if(!c) StartCoroutine(EndAnim(LocalisationSystem.GetLocalisedValue("successPass"), true));
        }
        else{
            if(!c) StartCoroutine(EndAnim(LocalisationSystem.GetLocalisedValue("wrongPass"),false));
        }
    }
    IEnumerator EndAnim(string endMessage, bool pass){
        c=true;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < endMessage.Length; i++) { 
            endTxt.text+=endMessage[i]; 
            SoundManager.instance.PlaySe(Se.Item);
            yield return new WaitForSeconds(0.2f);
        } 
        if(pass){
            screen.SetActive(true);
            this.enabled = false;
        }
        else{
            GetComponentInParent<PasswordInput>().Off();
        }
        c= false;
    }
    void OnDisable(){
        StopAllCoroutines(); c=false;Debug.Log("ondis");
        endTxt.text = "";
        startTxt.text = "";
        inpTxt.text = "";
        input.text="";
        input.enabled = false;
    }
}
