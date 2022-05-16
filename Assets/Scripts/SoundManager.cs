using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource seAS, bgAS, altBg;
    [SerializeField] public List<BgClip> bgClips;
    [SerializeField]public List<SeClip> seClips;
    public static SoundManager instance;
   public bool playLock=false;
    bool isPlaying;
    public Bg clip, lastClip;
    public bool IsPlaying 
    {
        get { return isPlaying; }
        set {}
    }
    void Awake()
    {
        var objs = gameObject.GetComponents<AudioSource>();
        //seAS = objs[0]; 
       // bgAS = objs[1];
       // altBg = objs[2];
        if (instance != null) Destroy(this);
        else instance = this;
        music_volume = bgAS.volume;
    }
    public float music_volume;
    IEnumerator FadeOutStop(AudioSource au){
        stop_run=true;
        Debug.Log("fade out stop volume");
       // float vol = au.volume;
        int i=0;
        while(au.volume>0){
            if(i>10){break;}
            Debug.Log("volume: "+au.volume);
            au.volume -= 0.1f;
            yield return new WaitForEndOfFrame();
           // vol-=0.1f;
           i++;
        }
        au.Stop();
        stop_run = false;
    }
    IEnumerator FadeOutPause(AudioSource au){
        pause_run = true;
        Debug.Log("fade out pause volume");
        //float vol = au.volume;
        int i=0;
        while(au.volume>0){
             if(i>10){break;}
            Debug.Log("volume: "+au.volume);
            au.volume -= 0.1f;
            yield return new WaitForEndOfFrame();
            i++;
           // vol-=0.1f;
        }
        au.Pause();
        pause_run = false;
    }
    bool fadein_run=false,stop_run=false,pause_run=false,alt_run=false;
    IEnumerator FadeIn(AudioSource au)
    {
        fadein_run=true;
        Debug.Log("fade in volume");
        au.Play();
      //  int i=0;
       // float vol=0f;
        while(au.volume<music_volume){
          //  while(alt_run){ }
        ///    if(i>10){break;}
            Debug.Log("volume: "+au.volume);
            au.volume += 0.1f;
            yield return new WaitForEndOfFrame();
         //   i++;
            //vol+=0.1f;
        }
        au.volume = music_volume;
        fadein_run=false;
    }
    public void PlaySe(Se se)
    {
        seAS.PlayOneShot(seClips.Where(c=>c.name == se).FirstOrDefault().clip);
    }
    public void PauseBg(){
        if(!pause_run)StartCoroutine(FadeOutPause(bgAS));
    }
    public void StopBg(){
        if(!stop_run)StartCoroutine(FadeOutStop(bgAS));
    }
    public void StopAltBg(){
        if(!stop_run)StartCoroutine(FadeOutStop(altBg));
    }
    public void ResumeLastBg(){
        if(clip==lastClip) return;
        if(altBg.isPlaying) StopAltBg();
        if(!fadein_run) StartCoroutine(FadeIn(bgAS));
        clip = lastClip;
    }
    public void PlayBg(Bg bg)
    {
        if(lastClip==bg||clip==bg||playLock) return;
        if(bgAS.isPlaying) StopBg();
        if(altBg.isPlaying) StopAltBg();
        clip = bg;
        lastClip = bg;
        bgAS.clip = bgClips.Where(c => c.name == bg).FirstOrDefault().clip;
        if(!fadein_run) StartCoroutine(FadeIn(bgAS));
    }
    IEnumerator ChangeAltBg(Bg bg){
        alt_run=true;
        while(altBg.volume>0){
            //if(i>10){break;}
            Debug.Log("volume: "+altBg.volume);
            altBg.volume -= 0.1f;
            yield return new WaitForEndOfFrame();
           // vol-=0.1f;
          // i++;
        }
        clip = bg;
        altBg.clip = bgClips.Where(c => c.name == bg).FirstOrDefault().clip;
        altBg.Play();
        //int i=0;
       // float vol=0f;
        while(altBg.volume<music_volume){
        ///    if(i>10){break;}
            Debug.Log("volume: "+altBg.volume);
            altBg.volume += 0.1f;
            yield return new WaitForEndOfFrame();
         //   i++;
            //vol+=0.1f;
        }
        altBg.volume = music_volume;
        alt_run = false;
    }
    public void PlayAltBg(Bg bg)
    {
        if(lastClip==bg||clip==bg||playLock) return;
        if(bgAS.isPlaying) PauseBg();
        if(altBg.isPlaying) {
            if(alt_run) return;
            StartCoroutine(ChangeAltBg(bg)); 
            return;
            }
        clip = bg;
        altBg.clip = bgClips.Where(c => c.name == bg).FirstOrDefault().clip;
        if(!fadein_run) StartCoroutine(FadeIn(altBg));
    }
}
public enum Bg
{
    scene1,
    scene2,
    menu,
    spooky,
    endBg,
    fnaf,
    noise,
    gameover
}
public enum Se
{
    Item,
    Click,
    Open,
    Close,
    Kick,
    Screamer,
    Click2,
    Quest,
    Woah,
    Kiss
}
[System.Serializable]
public class SeClip
{
    public Se name;
    public AudioClip clip;
    SeClip() { }
}
[System.Serializable]
public class BgClip
{
    public Bg name;
    public AudioClip clip;
    BgClip() { }
}