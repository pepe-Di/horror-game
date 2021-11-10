using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private ThirdPersonController controller;
    private StarterAssetsInputs _input;
    private Animator _animator;
    private Transform player;
    private void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        player = GetComponent<Transform>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chair")
        {
            Debug.Log("Press E to sit");
            if (_input.interact) { }
        }
    }
    public bool C_running = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Chair")
        {
            if (_input.interact)
            {
                Debug.Log("pressing e");
                if (!C_running && !_animator.GetBool("Crouch") && !_animator.GetBool("Sit"))
                {
                    _animator.SetBool("Sit", true);
                    StartCoroutine(Sit(other.GetComponent<Transform>()));
                }
                else if (!C_running && _animator.GetBool("Sit"))
                {
                    Debug.Log("press e");
                    _animator.SetBool("Sit", false);
                    StartCoroutine(Sit(other.GetComponent<Transform>()));
                }
            }
        }
        if (other.tag == "Door")
        {
            if (_input.click)
            {
                Ray ray = controller._mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Door" && !C_running)
                {
                    StartCoroutine(Door(other.transform));
                }
            }
        }
        if (other.tag == "Switch")
        {
            if (_input.click)
            {
                Debug.Log("click");
                Ray ray = new Ray(controller._mainCamera.transform.position, controller.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    Debug.Log(hit.transform.name);
                }
                 if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Switch")
                {
                    other.GetComponent<Switch>().Switching();
                }
            }
        }
    }

    IEnumerator Switch() 
    {
        C_running = true; 
        yield return new WaitForEndOfFrame();
        
        C_running = false;
    }

    IEnumerator Door(Transform door) 
    {
        C_running = true;
        Quaternion newRotation = new Quaternion(door.rotation.x, door.rotation.y, door.rotation.z, door.rotation.w);
        if (door.GetComponent<Door>().opened) { newRotation *= Quaternion.Euler(0, -90, 0); }
        else { newRotation *= Quaternion.Euler(0, 90, 0); }
        door.GetComponent<Door>().opened = !door.GetComponent<Door>().opened;
        Debug.Log(door.GetComponent<Door>().opened);
        while (door.rotation!=newRotation) 
        {
            door.rotation = Quaternion.Slerp(door.rotation, newRotation, 20 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        C_running = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
        IEnumerator Sit(Transform place) 
    {
        C_running = true;
        CharacterController c = player.GetComponent<CharacterController>();
        float step = 0f;
        Transform cam = controller.CinemachineCameraTarget.transform;
        if (_animator.GetBool("Sit"))
        {
            Transform sit_look = controller.Crouching_Look;
            c.enabled = false;
            place.GetComponent<BoxCollider>().enabled = false;
            player.rotation = place.rotation;
            while (player.position != place.position)
            {
                step = 1.5f * Time.deltaTime;
                player.position = Vector3.MoveTowards(player.position, place.position, step);
                yield return new WaitForEndOfFrame();
            }
            step = 0;
            while (cam.position != sit_look.position)
            {
                step = 1.5f * Time.deltaTime;
                cam.position = Vector3.MoveTowards(cam.position, sit_look.position, step);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            c.enabled = false;
            Transform stand_look = controller.Standing_look;
            Vector3 dir = place.position;
            dir.x += 0.4f;
            dir.y += 0.2f;
            while (player.position != dir)
            {
                step = 1.5f * Time.deltaTime;
                player.position = Vector3.MoveTowards(player.position, dir, step);
                yield return new WaitForEndOfFrame();
            }
            step = 0;
            while (cam.position != stand_look.position)
            {
                step = 1.5f * Time.deltaTime;
                cam.position = Vector3.MoveTowards(cam.position, stand_look.position, step);
                yield return new WaitForEndOfFrame();
            }
            c.enabled = true;
            place.GetComponent<BoxCollider>().enabled = true;
        }
        c.enabled = true;
        C_running = false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Chair")
        {
            Debug.Log("exit area");
        }
    }
}
