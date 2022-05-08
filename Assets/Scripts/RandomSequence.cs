using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSequence : MonoBehaviour
{
    public List<GameObject> lockers;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var lockers = GetComponentsInChildren<GameObject>();
        yield return new WaitUntil(()=>GameManager.instance.loaded);
       // Random.InitState(GameManager.instance.seed);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
