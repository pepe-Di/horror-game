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
    }

    public void PlaySe(Se se)
    {
        seAS.PlayOneShot(seClips.Where(c=>c.name == se).FirstOrDefault().clip);
    }
    public void PauseBg(){
        bgAS.Pause();
    }
    public void StopBg(){
        bgAS.Stop();
    }
    public void StopAltBg(){
        altBg.Stop();
    }
    public void ResumeLastBg(){
        if(clip==lastClip) return;
        if(altBg.isPlaying) StopAltBg();
        bgAS.Play();
        clip = lastClip;
    }
    public void PlayBg(Bg bg)
    {
        if(lastClip==bg||clip==bg) return;
        if(bgAS.isPlaying) StopBg();
        if(altBg.isPlaying) StopAltBg();
        clip = bg;
        lastClip = bg;
        bgAS.PlayOneShot(bgClips.Where(c => c.name == bg).FirstOrDefault().clip);
    }
    //bool altBg_playing=false,bg_playing=false
    public void PlayAltBg(Bg bg)
    {
        if(lastClip==bg||clip==bg) return;
        if(bgAS.isPlaying) PauseBg();
        if(altBg.isPlaying) StopAltBg();
        clip = bg;
        altBg.PlayOneShot(bgClips.Where(c => c.name == bg).FirstOrDefault().clip);
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
    Click2
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