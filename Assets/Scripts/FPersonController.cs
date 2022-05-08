using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPersonController : MonoBehaviour
{
    const float height = 1.74f, crouch_height=0.6f,Ycenter = 0.8f,Ycrouch=0.3f;
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 2.4f;
    [Tooltip("Crouch speed of the character in m/s")]
    public float CrouchSpeed = 0.4f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    [SerializeField] CharacterController controller;
    Vector2 horizontalInput;
    public Transform Standing_look;
    public Transform Crouching_Look;
    public Transform Sit_Look;
    [SerializeField] float jumpHeight = 3.5f;
    bool jump;
    public bool is_crouch= false;
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
    public StateController stateController;
    public Image cross;
    public GameObject _mainCamera;
    private StarterAssetsInputs _input;
    public Animator _animator; 
    public Animator cam_animator;
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
    Player player;
    float value = 1f;
    PlayerStats stats;
    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    public void ChangeState(State state){
        Debug.Log("ChangeState");
        stateController.ChangeState(state);
    }
     //Color bg,fg;
    // Image img;
    private void Start()
    {
        
        EventController.instance.StateEvent+=ChangeState;
        stateController.ChangeState(State.Idle);
        cam_animator = _mainCamera.GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        player = GetComponent<Player>();
        canvas = GameObject.FindObjectOfType<Canvas>();
        canvas.pixelPerfect = true;
        canvas.transform.position = Vector3.zero;

        Image crossHair = new GameObject("Crosshair").AddComponent<Image>();
       // crossHair.sprite = Crosshair;
        //crossHair.sprite = Resources.Load("ui/Reticle")as Sprite;
        //crossHair.rectTransform.sizeDelta = new Vector2(10, 10);
        //crossHair.transform.SetParent(canvas.transform);
        //crossHair.transform.position = Vector3.zero;

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
    bool warn=false;
    IEnumerator ChangeStaminaColor()
    {
        Debug.Log("ChangeStaminaColor()");
        
        Image img = stats.stamina_bar.GetComponent<Image>();
        Color bg = DataManager.instance.GetPalette().bg.color;
        Color fg = DataManager.instance.GetPalette().fg.color;
        /*while(warn)
        {   
            img.color = bg;
            yield return new WaitForSecondsRealtime(0.2f);
            img.color = fg;
        }*/
        if(warn)img.color = bg;
        else img.color = fg;
        yield return new WaitForSecondsRealtime(0.2f);
        Debug.Log("ChangeStaminaColor() end");
    }
    bool stop = false, press=false; float limit=10f;
    IEnumerator Wait(){press = true;
    
        yield return new WaitForSeconds(0.5f);
        press = false;
    }
    private void Update()
    {
       //Debug.DrawRay(controller.transform.position, Vector3.up, Color.cyan);
        if(stateController.state == State.Pause) return;
        if(stateController.state == State.Freeze) return;
        if(_input.middleMouse&&!press){
            SoundManager.instance.PlaySe(Se.Kick);
            StartCoroutine(Wait());
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5F, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,1.5f))
            {
                if(hit.rigidbody!=null){
                   Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                   // hit.rigidbody.isKinematic = false;
                    hit.rigidbody.AddForce(transform.forward*150);
                    //hit.rigidbody.isKinematic = true;
                }
            }
        }
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }
        else
        {
            stateController.ChangeState(State.Jump);
        }
        float targetSpeed = MoveSpeed;
        if (_input.move == Vector2.zero || _animator.GetBool("Sit")) 
        {
            targetSpeed = 0.0f;
            if (_animator.GetBool("Sit"))
            {
                stateController.ChangeState(State.Sit);
            }
            else
            {
                if(isGrounded&&!is_crouch)
                    stateController.ChangeState(State.Idle);
            }
        }
        else
        {
            if (isGrounded&&!is_crouch)
                stateController.ChangeState(State.Walk);
        }
        if (_input.crouch)
        {
            if (_input.move != Vector2.zero) targetSpeed = CrouchSpeed;
            if(!is_crouch) stateController.ChangeState(State.Crouch);
            //
        }
        else {
            //check ray
            stateController.ChangeState(State.Idle);
        }
        if (_input.sprint&& targetSpeed!=0&&!is_crouch)
        {
            if (stop) 
            { 
                limit = 500f;
                if(!warn) {
                    warn=true;
                StartCoroutine(ChangeStaminaColor());
                }
                //stats.stam_anim.SetBool("warn",true); 
            }
            if (isGrounded && player.stamina > limit && !_input.crouch)
            {
                targetSpeed = SprintSpeed;
                stateController.ChangeState(State.Sprint);
                player.stamina -= 10f;
                stats.ChangeStaminaBar(player.stamina);
                if (stop)
                {
                    if(warn) {
                    warn=false;
                StartCoroutine(ChangeStaminaColor());
                }
                    stop = !stop;
                    limit = 10f;
                   // stats.stam_anim.SetBool("warn", false);
                }
            }
        }
        if (player.stamina<player.max_stamina)
        {
            if (player.stamina <= 10) stop = true;
            if (targetSpeed == 0) player.stamina += 4f;
            else player.stamina += 2f;
            stats.ChangeStaminaBar(player.stamina);
        }
        speed = targetSpeed;
        cam_animator.SetFloat("speed", targetSpeed);
       // if(is_crouch&&stateController.state != State.Crouch)stateController.ChangeState(State.Crouch);
        if (stateController.state == State.Crouch&&!is_crouch){
            cam_animator.SetBool("crouch", true);is_crouch=true;
            controller.height = crouch_height;
            controller.center =new Vector3(0,Ycrouch,0);
        }
        else if(cam_animator.GetBool("crouch")&&stateController.state != State.Crouch) {
            //check ray first
            if(CheckRayHit()){
                stateController.ChangeState(State.Crouch);
            }
            else{
            cam_animator.SetBool("crouch", false);
            is_crouch=false;
            controller.height = height;
            controller.center =new Vector3(0,Ycenter,0);
            }
            
            }
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * SpeedChangeRate);
        Vector3 horizontalVelocity = (transform.right * _input.move.x + transform.forward * _input.move.y) * (speed + player.speed_modifier);
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (!_input.crouch && _input.jump&&!_animator.GetBool("Sit"))
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
                   // StartCoroutine(ChangeLook(true));
                }
                _animator.SetBool(_animIDCrouch, true);
            }
            else if (_animator.GetBool(_animIDCrouch)&&!is_crouch)
            {
                //if(!C_running)
               //if(!is_crouch) 
            // if(!C_running)  StartCoroutine(ChangeLook(false));
              // if(!is_crouch) 
              _animator.SetBool(_animIDCrouch, false);
            }
           // if(stateController.state!=State.Crouch){
            //    Debug.Log("hui!!!!!!");
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

          //  }
        }
    }
    public bool CheckRayHit()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.GetComponent<Camera>().ScreenPointToRay(Vector3.up);
        if (Physics.Raycast(_mainCamera.transform.position, Vector3.up, out hit,0.7f))
        {
                //if(this.gameObject.transform.position.y - hit.collider.transform.position.y < 0.1f)
                //{
            Debug.Log(hit.collider.name);
               // _animator.SetBool(_animIDCrouch, true);
           // is_crouch = true;
            Debug.DrawLine(controller.transform.position, hit.point, Color.cyan);
            return true;
        }
        else{
            return false;
        }
    }
    IEnumerator Jump()
    {
        stateController.ChangeState(State.Jump);
        Debug.Log("jump");
        verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator ChangeLookEnd(){
        Debug.Log("ChangeLookEnd");
        C_running = true;
        float step = 0f;
       // if (b)
      //  {
            //controller.height = crouch_height;
            controller.height = crouch_height;
            controller.center =new Vector3(0,Ycrouch,0);
            while (_mainCamera.transform.position != Crouching_Look.position)
            {
                step = 1.5f * Time.deltaTime;
                _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, Crouching_Look.position, step);
                yield return new WaitForEndOfFrame();
            }
      //  }
      is_crouch = false;
        C_running = false;
        Debug.Log("ChangeLookEndEnd");
    }
    IEnumerator ChangeLook(bool b)
    {
        Debug.Log("ChangeLook Start");
        C_running = true;
        float step = 0f;
        if (b)
        {
            //controller.height = crouch_height;
            controller.height = crouch_height;
            controller.center =new Vector3(0,Ycrouch,0);
            while (_mainCamera.transform.position != Crouching_Look.position)
            {
                step = 1.5f * Time.deltaTime;
                _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, Crouching_Look.position, step);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (!b)
        {
            RaycastHit hit;
            Ray ray = _mainCamera.GetComponent<Camera>().ScreenPointToRay(Vector3.up);
            if (Physics.Raycast(_mainCamera.transform.position, Vector3.up, out hit,0.7f))
            {
                //if(this.gameObject.transform.position.y - hit.collider.transform.position.y < 0.1f)
                //{
                    Debug.Log(hit.collider.name);
               // _animator.SetBool(_animIDCrouch, true);
                is_crouch = true;
                Debug.DrawLine(controller.transform.position, hit.point, Color.cyan);
      //  C_running = false;
               // StopAllCoroutines();
               // }
               yield return new WaitForSeconds(0.1f);
            }
            else
            {   
                Debug.Log("huy");
                controller.height = height;
                /* while(controller.height < height){
                    controller.height+=0.1f;
                    yield return new WaitForEndOfFrame();
                } */
                controller.center =new Vector3(0,Ycenter,0);
                while (_mainCamera.transform.position != Standing_look.position)
                {
                    step = 1.5f * Time.deltaTime;
                    _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, Standing_look.position, step);
                    yield return new WaitForSeconds(0.1f);
                }
                is_crouch = false;
            }
        }
        //if(!is_crouch) _animator.SetBool(_animIDCrouch, false);
        C_running = false;
        Debug.Log("ChangeLook-End");
    }
}
