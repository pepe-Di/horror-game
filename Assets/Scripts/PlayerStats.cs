using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public RectTransform stamina_bar, hp_bar;
    public Animator stam_anim;
    public float max_stam,max_hp, max_stam_width, max_hp_width;
    void Start()
    {
        //stam_anim = stamina_bar.gameObject.GetComponent<Animator>();
        max_hp = GetComponent<Player>().max_hp;
        max_stam = GetComponent<Player>().max_stamina;
        max_stam_width = stamina_bar.sizeDelta.x;
        max_hp_width = hp_bar.sizeDelta.x;
        hp_bar.gameObject.SetActive(false);
        stamina_bar.gameObject.SetActive(false);
    }
    public void ChangeParams(float hp, float stamina)
    {
        stamina_bar.sizeDelta = new Vector2(max_stam_width *stamina/max_stam, stamina_bar.sizeDelta.y);
        hp_bar.sizeDelta = new Vector2(max_hp_width*hp/max_hp, hp_bar.sizeDelta.y);
    }
    public void ChangeStaminaBar(float stamina)
    {
        if (stamina >= max_stam) stamina_bar.gameObject.SetActive(false); 
        else
        {
            stamina_bar.gameObject.SetActive(true);
            stamina_bar.sizeDelta = new Vector2(max_stam_width * stamina / max_stam, stamina_bar.sizeDelta.y);
        }
    }
    IEnumerator EndAnim()
    {
        yield return null;
    }
    public void ChangeHpBar(float hp)
    {
        if (hp >= max_hp) hp_bar.gameObject.SetActive(false);
        else
        {
            hp_bar.gameObject.SetActive(true);
            hp_bar.sizeDelta = new Vector2(max_hp_width * hp / max_hp, hp_bar.sizeDelta.y);
        }
    }
}

