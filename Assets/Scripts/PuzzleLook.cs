using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLook : MonoBehaviour
{
    public GameObject ipos;
    List<string> answer=new List<string>{"0","1","2","3"};
    List<string> player_answer = new List<string>();
    public List<SpriteRenderer> greed;
    public Camera camera_;
    public GameObject buttons;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        EventController.instance.OffPuzzleUI+=OffPuzzleUI;
        buttons.SetActive(false);
        camera_.transform.gameObject.SetActive(false);
        yield return new WaitUntil(()=>Player.instance.loaded);
        Item i =GameManager.instance.player_.FindItem("notepad");
        if(i!=null)Destroy(this.gameObject);
        //if(ipos.GetComponentInChildren<GameObject>()==null)Destroy(this.gameObject);
    }
    public void OnButtonClick(GameObject button){
        SoundManager.instance.PlaySe(Se.Click);
        if(player_answer!=null){
            if(player_answer.Count>4) return;
        }
        player_answer.Add(button.name);
        greed[player_answer.LastIndexOf(button.name)].sprite=Resources.Load<Sprite>("sprites/symbols/"+button.name);
        if(player_answer.Count==4){
            if(CheckForAnswer())
            {
                EventController.instance.StartDialogueEvent("puzzleComplete");
                Destroy(this.gameObject);
            }
            else EventController.instance.StartDialogueEvent("puzzleFail");
            player_answer.Add("");
        }
    }
    bool CheckForAnswer()
    {
        int i=0;
        foreach(string s in answer){
            if(s!=player_answer[i]) return false;
            i++;
        }
        return true;
    }
    public void ChangeLook()
    {
        foreach(SpriteRenderer spr in greed){
            spr.sprite = null;
        }
        buttons.SetActive(true);
        camera_.transform.gameObject.SetActive(true);
        EventController.instance.StartCameraEvent(false);
        EventController.instance.ChangeStateEvent(State.Freeze);
        GameManager.instance.inv.GetMessage(LocalisationSystem.GetLocalisedValue("message4"));
        EventController.instance.StartDialogueEvent("idk");
        SoundManager.instance.PlaySe(Se.Click);
        GameManager.instance.CursorLock(false);
    }
    public void OffPuzzleUI(){
        buttons.SetActive(false);
        player_answer.Clear();
    }
}
