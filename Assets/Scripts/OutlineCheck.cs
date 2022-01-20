using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCheck : MonoBehaviour
{
    public GameObject CameraPlayer;
    public AudioClip HitSound;
    private float timer;
    private bool CanPlaySound;
    //if GameObject is Character
    public bool isCharacter;
    public GameObject Character_Outline;

    // Use this for initialization
    void Start()
    {
        CameraPlayer = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPlaySound == false)
        {
            timer += 1 * Time.deltaTime;
            if (timer >= 0.3f)
            {
                CanPlaySound = true;
                timer = 0;
            }
        }
        if (isCharacter == false)
        {
            if (CameraPlayer.GetComponent<Outline_ray>().Object_Hit == gameObject)
            {
                if (CameraPlayer.GetComponent<Outline_ray>().IsActive == true)
                {
                    gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<Outline>().enabled = false;
                }
            }
            else
            {
                gameObject.GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            if (CameraPlayer.GetComponent<Outline_ray>().Object_Hit == gameObject)
            {
                if (CameraPlayer.GetComponent<Outline_ray>().IsActive == true)
                {
                    Character_Outline.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Character_Outline.GetComponent<Outline>().enabled = false;
                }
            }
            else
            {
                Character_Outline.GetComponent<Outline>().enabled = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (CanPlaySound == true)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(HitSound);
            CanPlaySound = false;
        }
    }
}
