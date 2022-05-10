using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource seAS, bgAS;
    [SerializeField] public List<BgClip> bgClips;
    [SerializeField]public List<SeClip> seClips;
    public static SoundManager instance;
    bool isPlaying;
    Bg clip;
    public bool IsPlaying 
    {
        get { return isPlaying; }
        set {}
    }
    void Awake()
    {
        var objs = gameObject.GetComponents<AudioSource>();
        seAS = objs[0]; 
        bgAS = objs[1];
        if (instance != null) Destroy(this);
        else instance = this;
    }

    public void PlaySe(Se se)
    {
        seAS.PlayOneShot(seClips.Where(c=>c.name == se).FirstOrDefault().clip);
    }
    public void StopBg(){
        bgAS.Stop();
    }
    public void PlayBg(Bg bg)
    {
        if(clip==bg) return;
        StopBg();
        //bgAS.loop = true;
        clip = bg;
        bgAS.PlayOneShot(bgClips.Where(c => c.name == bg).FirstOrDefault().clip);
        bgAS.loop = true;
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
    noise
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