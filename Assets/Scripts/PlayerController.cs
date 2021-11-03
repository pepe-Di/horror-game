using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform transform, start_transform;
    public Vector3 position;
    void Start()
    {
         transform = GetComponent<Transform>(); start_transform = transform;
        position = transform.position;
         animator = GetComponent<Animator>();
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("isWalking", isWalking);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (animator.GetBool("isWalking")) animator.SetBool("isRunning", true);
            else animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) animator.SetBool("isRunning", false);
        transform.position = position;
    }
}
