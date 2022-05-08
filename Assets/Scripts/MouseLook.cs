using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float multiplier =1f;
    float mouseX, mouseY;
    private StarterAssetsInputs _input;

    [SerializeField] Transform playerCamera;
    Animator animator;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;
    int zoom = 20;
    int normal = 60;
    float smooth = 5f;
    private bool isZoomed = false;
    Camera _camera;
    public void SensChange(float value)
    {
        sensitivityX = 8f * value; 
        sensitivityY= 0.5f * value*0.2f;
    }
    private void Awake()
    {
        
    }
    public void ChangeCameraLook(bool frame_mode)
    {
        if (frame_mode) normal = 90;
        else normal = 60;
    }
    private void Start()
    {
        _camera = playerCamera.GetComponent<Camera>();
        
        EventController.instance.CameraEvent+=ChangeCameraLook;
        animator = playerCamera.gameObject.GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; EventController.instance.FrameEvent += ChangeCameraLook;
    }
    bool freeze = false;
    void FreezeCamera(bool b){
        Debug.Log("FreezeCamera");
        animator.SetBool("paused", b);
        
        if (b){ EventController.instance.ChangeStateEvent(State.Freeze);
        freeze = true; this.enabled = false;}
        else {EventController.instance.ChangeStateEvent(State.Idle);//??
            freeze = false;
        }
    }
    IEnumerator WaitForTransform(){
        yield return new WaitUntil(()=>b);
        this.enabled = false;
    }
    public void ChangeCameraLook(Transform transform, bool freeze){
        
        Debug.Log("ChangeCameraLook func");
        StartCoroutine(ChangeCamLook(transform, freeze));
    }
    bool b = false;
    IEnumerator ChangeCamLook(Transform t, bool freeze){
        float step;
        Debug.Log("ChangeCamLookStart");
        while (_camera.transform.position != t.position)
            {
                step = 1.5f * Time.deltaTime;
                _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, t.position, step);
                yield return new WaitForEndOfFrame();
                Debug.Log("");
            }
        Debug.Log("ChangeCamLook");
            FreezeCamera(freeze);
    }
    void SetCamera(bool b){
        GetComponent<PlayerInteract>().enabled = b;
        _camera.transform.gameObject.SetActive(b);
        Debug.Log("bru");
    }
    IEnumerator Zoom(int i) 
    {
        C_running = true;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, i, Time.deltaTime * smooth);
        yield return new WaitForEndOfFrame();
        C_running = false;
    }
    bool C_running = false;
    private void Update()
    {
        if (_input.zoom) 
        {
            isZoomed = !isZoomed;
            _input.zoom = false;
        }
        if (isZoomed && !C_running) StartCoroutine(Zoom(zoom));
        else if (!C_running) StartCoroutine(Zoom(normal));
        transform.Rotate(Vector3.up, _input.mouseX * sensitivityX*Time.deltaTime);
        xRotation -= _input.mouseY* sensitivityY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }
    IEnumerator CameraMove()
    {
        C_running = true;
        Transform transform_ = playerCamera.transform;
        while (_input.move != Vector2.zero)
        {
            while (playerCamera.position != new Vector3(playerCamera.position.x, playerCamera.position.y + 1f, playerCamera.position.z))
            {
                playerCamera.position = Vector3.MoveTowards(playerCamera.position, new Vector3(playerCamera.position.x, playerCamera.position.y+1f, playerCamera.position.z), 1.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while (playerCamera.position != new Vector3(playerCamera.position.x, playerCamera.position.y - 1f, playerCamera.position.z))
            {
                playerCamera.position = Vector3.MoveTowards(playerCamera.position, new Vector3(playerCamera.position.x, playerCamera.position.y - 1f, playerCamera.position.z), 1.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        } 
        while (playerCamera.position != transform_.position)
        {
            playerCamera.position = Vector3.MoveTowards(playerCamera.position, transform_.position, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        C_running = false;
    }
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
    void OnEnable(){
        EventController.instance.CameraEvent+=SetCamera;
        EventController.instance.FreezeCamera+=FreezeCamera;
    }
    void OnDisable(){
        EventController.instance.CameraEvent-=SetCamera;
        EventController.instance.FreezeCamera-=FreezeCamera;
    }
}
