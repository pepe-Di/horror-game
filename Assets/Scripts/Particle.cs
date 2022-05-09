using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    Animator animator;
    Transform transform_;
    [SerializeField] public float minX = -400f, maxX = 400f, maxY = 350f, minY = -334f,speed,max_speed=0.01f,min_speed=0.002f;
    [SerializeField] public float Ydelta = 200f, timer=0.1f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        transform_ = this.gameObject.transform;
        //Vector2 position = new Vector2(Random.Range(minX, maxX), Y);
        transform_.localPosition = new Vector2(Random.Range(minX, maxX), maxY);
        speed = Random.Range(min_speed,max_speed);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Fall());
        //StartCoroutine(End());
    }
    IEnumerator Fall()
    {
        float y_= maxY,x_= transform_.localPosition.x;
        float distanceY = Random.Range(minY,200f);
        while (true)
        {
            y_ -= speed;
            transform_.localPosition = new Vector2(x_, y_);
            yield return new WaitForEndOfFrame();
            if(transform_.localPosition.y< distanceY) StartCoroutine(End());
        }
    }
    IEnumerator End()
    {
        Image img = this.gameObject.GetComponent<Image>();
        animator.SetTrigger("end");
        float step = 0.001f;
        while(img.color.a>0f)
        {
            transform_.localScale =new Vector2(transform_.localScale.x+step,transform_.localScale.y+step);
            //img.color = new Color(c.r,c.g,c.b,255f-step/255f);
           // Debug.Log(c.r+img.color.a);
            //img.color = new Color(c.r,c.g,c.b,c.a-step);
            //if(transform_.localScale.x+step>100f) step=0;
            yield return new WaitForFixedUpdate();
        } 
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
