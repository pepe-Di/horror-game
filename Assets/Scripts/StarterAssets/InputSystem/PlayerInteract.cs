using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public GameObject flashlight;
    private bool flash = false, locked=false;
    [SerializeField] private Material highlightMaterial;
    AudioSource audioSource; 
    private float volume = 0.3f;
    private FPersonController controller;
    private StarterAssetsInputs _input;
    private Animator _animator;
    private Transform player;
    Player player_;
    GameObject selected_item;
    Vector3 Ray_start_pos = new Vector3(0.5f, 0.5F, 0);
    Vector3 ray_ = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    public Camera mainCam;
    public GameObject Crosshair;
    public Image crosshair;
    public GameObject grab_cur;
    public GameObject eye_cur;
    private void Awake()
    {
        player_ = GetComponent<Player>();
        audioSource = gameObject.AddComponent<AudioSource>();
        controller = GetComponent<FPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        //player = GetComponent<Transform>();
        //mainCam = controller._mainCamera.GetComponent<Camera>();
    }
    void Start(){
        
        grab_cur.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chair")
        {
            Debug.Log("Press E to sit");
            //if (_input.interact) { }
        }
        if (other.tag == "Note")
        {
            Debug.Log("note trigger enter");
            //if (_input.interact) { }
        }
        if (other.tag == "Door")
        {
            Debug.Log("door trigger enter");
        }
    }
    public bool C_running = false, selected=false;
    int click = 0;
    Outline gm;
    private Transform _selection;
    private void FixedUpdate()
    {
        if (_input.f&&!locked)
        {
        Item i = player_.FindItem("flashlight");
        if(i==null) return;
            StartCoroutine(Flash());
            Player.instance.StopAllCoroutines();
            flashlight.SetActive(!flash);
            flash = !flash;
        }
    }
    IEnumerator Flash()
    {
        locked = true;
        yield return new WaitForSeconds(0.2f);
        locked = false;
    }
    private void Update()
    {
        if (_selection != null)
        {
            //var selectionOutline = _selection.GetComponent<Outline>(); 
            //selectionOutline.enabled = false;
            Crosshair.SetActive(true);
            grab_cur.SetActive(false);
            eye_cur.SetActive(false);
        }
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5F, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,1.5f))
        {   
            if (hit.transform.CompareTag("Untagged")){ return;}
            if(hit.transform.CompareTag("Look")){
            Crosshair.SetActive(false);
                eye_cur.SetActive(true);
                var selection = hit.transform;
                _selection = selection;
                if (_input.click)
                {
                    click++;
                    if (click == 1)
                    {
                        Item i = new Item(hit.transform.name);
                        GameManager.instance.StartDialogue(i.GetLookAt());
                        if(i.Grab){
                            StartCoroutine(Timer(hit.transform));
                        //eye_cur.SetActive(false);
                        }
                    }
                    else{
                        click=0;
                    }
                }
                return;
            }
            if (hit.transform.CompareTag("Item"))
            {
                //crosshair.sprite = Resources.Load<Sprite>("ui/grab");
                grab_cur.SetActive(true);
                var selection = hit.transform;
                //var selectionOutline = selection.GetComponent<Outline>();
                //if (selectionOutline != null)
                //{
                //    selectionOutline.enabled=true;
                //}
                _selection = selection;
                if (_input.click)
                {
                    click++;
                    if (click == 1)
                    {
                        if (player_.GetItem(hit.collider.gameObject))
                        {
                            try
                            {
                                hit.transform.GetComponentInParent<ItemPos>();
                                
                                QuestItem qi = hit.collider.GetComponent<QuestItem>();
                                if (qi != null)
                                {
                                    EventController.instance.EndQEvent(qi.id);
                                    EventController.instance.UpdateQEvent();
                                    Debug.Log("Qi " + qi.id);
                                }
                            }
                            catch { }
                            Destroy(hit.collider.gameObject);
                            SoundManager.instanñe.PlaySe(Se.Item);
                            //crosshair.sprite = Resources.Load<Sprite>("ui/Reticle");
                            grab_cur.SetActive(false);
                        }
                        else
                        {
                            GameManager.instance.inv.GetMessage(LocalisationSystem.TryGetLocalisedValue("message1"));
                        }
                        Debug.Log(hit.collider.gameObject.name);
                    }
                    else
                    {
                        click = 0;
                    }
                }
                    return;
            }
            if (hit.transform.CompareTag("Door"))
            {
                //crosshair.sprite = Resources.Load<Sprite>("ui/grab");
                grab_cur.SetActive(true);
                var selection = hit.transform;
                //var selectionOutline = selection.GetComponent<Outline>();
                //if (selectionOutline != null)
                //{
                //    selectionOutline.enabled=true;
                //}
                _selection = selection;
                if (_input.click)
                {
                    click++;
                    if (click == 1)
                    {
                        if(!C_running) 
                        {
                            Door d = hit.transform.GetComponentInChildren<Door>();
                            if(d.locked){
                                string key_name="key"+d.index;
                                Item key = player_.FindItem(key_name);
                                if(key==null){
                                    InventoryUI.instance.GetMessage(LocalisationSystem.GetLocalisedValue("message2"));
                                   click = 0; return;
                                }
                                else{
                                        string s = player_.GetSelectedItem().GetGmName();
                                    if(s==key_name){

                                    }
                                    else{
                                        InventoryUI.instance.GetMessage(LocalisationSystem.GetLocalisedValue("message2"));
                                       click = 0; return;
                                    }
                                }
                            }
                            StartCoroutine(Door(hit.transform));
                            }
                    }
                    else
                    {
                        click = 0;
                    }
                }
                    return;
            }
            if (hit.transform.CompareTag("Drawer")){
                grab_cur.SetActive(true);
                var selection = hit.transform;
                _selection = selection;
                if (_input.click)
                {
                    click++;
                Debug.Log("C_running = "+C_running);
                Debug.Log("Click = "+click);
                    if (click == 1)
                    {
                Debug.Log("click on drawer");
                    if(!C_running) 
                    {
                        Lock l = hit.transform.GetComponent<Lock>();
                        if(l.locked){
                                 string key_name="key"+l.index;
                                Item key = player_.FindItem(key_name);
                                if(key==null){
                                    InventoryUI.instance.GetMessage(LocalisationSystem.GetLocalisedValue("message2"));
                                    return;
                                }
                                else{
                                        string s = player_.GetSelectedItem().GetGmName();
                                    if(s==key_name){

                                    }
                                    else{
                                        InventoryUI.instance.GetMessage(LocalisationSystem.GetLocalisedValue("message2"));
                                        return;
                                    }
                            }
                        }
                            Debug.Log("StartCoroutine()");StartCoroutine(Drawer(hit.transform));click = 0;
                    }
                    
                }else
                    {
                        click = 0;
                    }
            }
                    return;
        }
    }

     void OnTriggerStay(Collider other)
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
                //Ray ray = controller._mainCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5F, 0));
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Door" && !C_running)
                //{
                //    StartCoroutine(Door(other.transform));
                //}
            }
        }
        if (other.tag == "Switch")
        {
            if (_input.click)
            {
                int layerMask = 1 << 5;
                Debug.Log("click"); layerMask = ~layerMask;
                Ray ray = controller._mainCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5F, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
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
        if (other.tag == "Item")
        {
            /* Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2) && hit.transform.tag == "Item" && !C_running)
            {
                if (!selected)
                {
                    hit.collider.gameObject.GetComponent<Outline>().enabled = true;
                    selected = true;
                    crosshair.sprite = Resources.Load<Sprite>("ui/grab");
                }
                else { }
                if (_input.click&& hit.collider.gameObject.GetComponent<Outline>().enabled)
                {
                    click++;
                    if (click == 1)
                    {
                        player_.GetItem(hit.collider.gameObject.name);
                        Debug.Log(hit.collider.gameObject.name);
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        click = 0;
                    }
                }
            }
            else
            {
                other.GetComponent<Outline>().enabled = false; 
                selected = false;
                crosshair.sprite = Resources.Load<Sprite>("ui/Reticle");
            } */
            //if (_input.click)
            //{
            //    if (Physics.Raycast(ray, out hit,1) && hit.transform.tag == "Note" && !C_running)
            //    {
            //        Debug.Log("click on note");
            //    }
            //}
        }
    }
    IEnumerator Switch() 
    {
        C_running = true; 
        yield return new WaitForEndOfFrame();
        C_running = false;
    }
    IEnumerator Drawer(Transform transform){
        Debug.Log("Drawer()");
        C_running = true;
        Lock l = transform.GetComponent<Lock>();
        float value = l.opened?-1f:1f;
        Vector3 forward = new Vector3(0,0,value);
        float f = transform.localPosition.z;
        if(l.opened){
            //transform.localPosition -= forward;
            //new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z-1f);
            
            while(transform.localPosition.z>f+value)
            {
                transform.localPosition += new Vector3(0, 0,0.1f*value);
                yield return new WaitForEndOfFrame();
                Debug.Log(transform.localPosition.z);

            }
        }
        else{
            while(transform.localPosition.z<f+value)
            {
                transform.localPosition += new Vector3(0, 0,0.1f*value);
                yield return new WaitForEndOfFrame();
                Debug.Log(transform.localPosition.z);

            }
           // transform.localPosition += forward;
            //new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z+1f);
        }
        l.opened=!l.opened;
        C_running = false;
    }
    IEnumerator Door(Transform door) 
    {
        C_running = true;
        Quaternion newRotation = new Quaternion(door.rotation.x, door.rotation.y, door.rotation.z, door.rotation.w);
        int val = 1;
        Door d = door.GetComponent<Door>();
        if (d.right_side) val = -1;
        if (d.opened) { audioSource.PlayOneShot(Resources.Load("Sounds/door2") as AudioClip, volume); newRotation *= Quaternion.Euler(0, -90*val, 0); }
        else { audioSource.PlayOneShot(Resources.Load("Sounds/door1") as AudioClip, volume); newRotation *= Quaternion.Euler(0, 90*val, 0); }
        d.opened = !d.opened;
        Debug.Log(door.GetComponent<Door>().opened);
        while (door.rotation!=newRotation) 
        {
            door.rotation = Quaternion.Slerp(door.rotation, newRotation, 20 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        C_running = false;click = 0;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(mainCam.transform.position, new Vector3(0.5f, 0.5F, 0));
    }
    IEnumerator Sit(Transform place) 
    {
        C_running = true;
        CharacterController c = player.GetComponent<CharacterController>();
        float step = 0f;
        Transform cam = controller._mainCamera.transform;
        if (_animator.GetBool("Sit"))
        {
            Transform sit_look = controller.Sit_Look;
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
        if (other.tag == "Item")
        {
            if (selected)
            {
                selected = false;
                other.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
    }
}

    IEnumerator Timer(Transform transform)
    {
        yield return new WaitForSeconds(0.1f);
        transform.tag = "Item";
    }
}

