using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
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

    private void Start()
    {
        animator = playerCamera.gameObject.GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    IEnumerator Zoom(int i) 
    {
        C_running = true;
        GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, i, Time.deltaTime * smooth);
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
}
