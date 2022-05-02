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
    public void PlayBg(Bg bg)
    {
        bgAS.PlayOneShot(bgClips.Where(c => c.name == bg).FirstOrDefault().clip);
    }
}
public enum Bg
{
    scene1,
    scene2
}
public enum Se
{
    Item,
    Click,
    Open,
    Close,
    Kick
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