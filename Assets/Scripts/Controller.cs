using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    float velocity_ = 0.0f;
    public Transform cam;
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    void Start()
    {
        //animator = GetComponent<Animator>();
    }
    bool clicking = false; public float ClickDuration = 0.2f;
    float totalDownTime = 0; public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        if (!animator.GetBool("attack")) animator.SetBool("isMoving", isWalking);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")|| animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) speed = 6f;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) speed = 3f;
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack5")) speed = 1f;
        else speed = 0.1f;
        if (direction.magnitude >= 0.1f)
        {
            float targerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targerAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targerAngle, 0f) * Vector3.forward; 
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        if (controller.isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = 7f;
        }
        else
        {
            velocity.y += -10f * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            //Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    Interactable interacable = hit.collider.GetComponent<Interactable>();
            //    if (interacable != null) 
            //    {
            //        SetFocus(interacable);
            //    }
            //}
        }
    }
    IEnumerator Wait(float secs, string anim) 
    {
        yield return new WaitForSeconds(secs);
        animator.SetBool(anim, false); 
    }
}
