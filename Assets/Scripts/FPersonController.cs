using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPersonController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 2.4f;
    [Tooltip("Crouch speed of the character in m/s")]
    public float CrouchSpeed = 1.0f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    [SerializeField] CharacterController controller;
    Vector2 horizontalInput;
    public Transform Standing_look;
    public Transform Crouching_Look;
    public Transform Sit_Look;
    [SerializeField] float jumpHeight = 3.5f;
    bool jump;
    private float _speed;
    private float _animationBlend = 0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [SerializeField] float gravity = -30f; // -9.81
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    public GameObject _mainCamera;
    private StarterAssetsInputs _input;
    public Animator _animator;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDCrouch;
    private bool _hasAnimator;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    bool C_running = false;
    Sprite Crosshair;
    private Canvas canvas;
    public float speed;
    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        canvas.pixelPerfect = true;
        canvas.transform.position = Vector3.zero;

        Image crossHair = new GameObject("Crosshair").AddComponent<Image>();
        crossHair.sprite = Crosshair;
        crossHair.rectTransform.sizeDelta = new Vector2(10, 10);
        crossHair.transform.SetParent(canvas.transform);
        crossHair.transform.position = Vector3.zero;
        _animator = GetComponent<Animator>();
        _hasAnimator = TryGetComponent(out _animator);
        controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDCrouch = Animator.StringToHash("Crouch");
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }
        float targetSpeed = MoveSpeed;
        if (_input.crouch) targetSpeed = CrouchSpeed;
        else if (_input.sprint)
        {
            targetSpeed = SprintSpeed;
        }
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;
        speed = targetSpeed;
        //float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        //float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        Vector3 horizontalVelocity = (transform.right * _input.move.x + transform.forward * _input.move.y) * targetSpeed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (!_input.crouch && _input.jump)
        {
            if (isGrounded)
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
                StartCoroutine(Jump());
            }
            _input.jump = false; _animator.SetBool(_animIDJump, false);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        if (_hasAnimator)
        {
            if (_input.crouch)
            {
                if (!C_running && !_animator.GetBool(_animIDCrouch))
                {
                    StartCoroutine(ChangeLook(true));
                }
                _animator.SetBool(_animIDCrouch, true);
            }
            else if (_animator.GetBool(_animIDCrouch))
            {
                _animator.SetBool(_animIDCrouch, false);
                StartCoroutine(ChangeLook(false));
            }
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }
    IEnumerator Jump() 
    {
        verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator ChangeLook(bool b)
    {
        C_running = true;
        float step = 0f;
        if (b)
        {
            while (_mainCamera.transform.position != Crouching_Look.position)
            {
                step = 1.5f * Time.deltaTime;
                _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, Crouching_Look.position, step);
                yield return new WaitForEndOfFrame();
            }
        }
        if (!b)
        {
            while (_mainCamera.transform.position != Standing_look.position)
            {
                step = 1.5f * Time.deltaTime;
                _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, Standing_look.position, step);
                yield return new WaitForEndOfFrame();
            }
        }
        C_running = false;
    }
}
