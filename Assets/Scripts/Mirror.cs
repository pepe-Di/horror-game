using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    Camera camera;
    Rigidbody rb;
    GameObject gm;
    bool is_entered=false;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Player")
        //{
        //    is_entered = true;
        //    camera.transform.SetParent(other.transform);
        //    //camera.transform.rotation.
        //    Matrix4x4 mat = Camera.main.projectionMatrix;
        //    mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        //    Camera.main.projectionMatrix = mat;
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Player")
        //{
        //    is_entered = false;
        //    camera.transform.SetParent(this.transform);
        //}
    }
    // Update is called once per frame
    void Update()
    {
        //if (is_entered)
        //{
        //    camera.transform = 
        //}
    }
}
