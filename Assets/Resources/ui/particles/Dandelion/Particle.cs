using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    Transform transform_;
    [SerializeField] public float minX = -400f, maxX = 400f, maxY = 350f, minY = -334f,speed,max_speed=0.0001f,min_speed=0.00002f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        transform_ = this.gameObject.transform;
        //Vector2 position = new Vector2(Random.Range(minX, maxX), Y);
        transform_.localPosition = new Vector2(Random.Range(minX, maxX), maxY);
        speed = Random.Range(min_speed,max_speed);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Fall());
    }
    IEnumerator Fall()
    {
        float y_= maxY,x_= transform_.localPosition.x;
        while (transform_.localPosition.y> minY)
        {
            y_ -= speed;
            transform_.localPosition = new Vector2(x_, y_);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
