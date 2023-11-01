using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance => instance;

    public bool cursor;
    [Header("移动速度")]
    public float walkMoveSpeed;
    public float runMoveSpeed;
    public float crouchMoveSpeed;
    [Space]
    [Header("力")]
    public float CrouchSpeed;
    public float JumpSpeed;
    public float gravity;
    [Space]
    [Header("旋转速度")]
    public float X_rotationSpeed;
    public float Y_rotationSpeed;
    [Header("音效文件")]
    public AudioSource _audioHeartbeat;
    public AudioSource _audioFootsteps;
    public AudioClip footsteps_Default;
    public AudioClip footsteps_Fast;
    public AudioClip Heartbeat_Default;
    public AudioClip Heartbeat_Fast;

    [HideInInspector]
    public bool changeRooms;
    [HideInInspector]
    public bool hideing = false;
    [HideInInspector]
    public bool diary = false;
    [HideInInspector]
    public Vector3 hidePlayerPos;
    [HideInInspector]
    public CameraMove _camera;
    private CharacterController _characterController;

    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public Vector3 move;
    [HideInInspector]
    public bool death = false;
    private float lookHight;
    private bool isOnGround;
    private bool isCeiling;
    private Vector3 velocity;
    private float runTime;
    private bool closeSound = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if(cursor)
            Cursor.lockState = CursorLockMode.Locked;

        moveSpeed = walkMoveSpeed;
        _camera = Camera.main.GetComponent<CameraMove>();
        _characterController = GetComponent<CharacterController>();
        lookHight = _camera.lookBodyHight;
        velocity = Vector3.zero;
        isOnGround = true;
        runTime = 0;
        _audioHeartbeat.clip = Heartbeat_Default;
        _audioHeartbeat.Play();
        _audioFootsteps.clip = footsteps_Default;
    }

    void Update()
    {
        if (Time.timeScale == 0 || death)
        {
            closeSound = true;
            if (closeSound && _audioHeartbeat.isPlaying)
                _audioHeartbeat.Pause();
            if (closeSound && _audioFootsteps.isPlaying)
                _audioFootsteps.Pause();
            return;
        }
        else if (closeSound)
        {
            closeSound = false;
            _audioHeartbeat.UnPause();
            _audioFootsteps.UnPause();
        }

        
        if(move == Vector3.zero || hideing)
        {
            if (_audioHeartbeat.clip == Heartbeat_Fast)
            {
                _audioHeartbeat.volume = 1;
                _audioHeartbeat.clip = Heartbeat_Default;
                _audioHeartbeat.Play();
            }
            _audioFootsteps.Stop();
        }        
        else if(!_audioFootsteps.isPlaying)
            _audioFootsteps.Play();

        if (diary)
            return;

        if (!changeRooms && !hideing)
            Move();

        rotationLook();

        if (!hideing)
            CrouchAndJump();
    }

    //移动
    public void Move()
    {
        move.z = Input.GetAxis("Vertical");
        move.x = Input.GetAxis("Horizontal");
        move.y = 0;

        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
            runTime += Time.deltaTime;
            if (moveSpeed < runMoveSpeed)
                moveSpeed += Time.deltaTime * 3.5f;
            if(moveSpeed > runMoveSpeed)
                moveSpeed = runMoveSpeed;
            if(_audioFootsteps.clip == footsteps_Default && runTime > 1f)//普通变急跑
            {                
                _audioFootsteps.clip = footsteps_Fast;
                _audioFootsteps.Play();
            }
            if (_audioHeartbeat.clip == Heartbeat_Default && runTime > 2f)//普通变急喘
            {
                _audioHeartbeat.clip = Heartbeat_Fast;
                _audioHeartbeat.Play();
            }

        }           
        else if (moveSpeed > walkMoveSpeed)
        {
            if (_audioHeartbeat.clip == Heartbeat_Fast &&_audioHeartbeat.isPlaying)
                _audioHeartbeat.volume -= Time.deltaTime;

            moveSpeed -= Time.deltaTime * 2.5f;
            if (moveSpeed < walkMoveSpeed)
            {
                moveSpeed = walkMoveSpeed;
                runTime = 0;
                if (_audioHeartbeat.clip == Heartbeat_Fast)//急喘变普通
                {
                    _audioHeartbeat.volume = 1;
                    _audioHeartbeat.clip = Heartbeat_Default;
                    _audioHeartbeat.Play();
                }
                if (_audioFootsteps.clip == footsteps_Fast)//急跑变普通
                {
                    _audioFootsteps.clip = footsteps_Default;
                    _audioFootsteps.Play();
                }
            }
                
        }
        _characterController.Move(transform.TransformDirection(move) * moveSpeed * Time.deltaTime);      
    }

    public void rotationLook()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * X_rotationSpeed);
        lookHight += Input.GetAxis("Mouse Y") * Y_rotationSpeed;
        _camera.lookBodyHight = Mathf.Clamp(lookHight, -1, 1.9f);
    }

    public void CrouchAndJump()
    {
        isOnGround = Physics.Raycast(transform.position, -transform.up, 1.9f,1<< LayerMask.NameToLayer("Ground"));
        isCeiling = Physics.Raycast(transform.position, transform.up, 2.2f, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetKey(KeyCode.LeftControl) && isOnGround)
        {
            if (_camera.offsetPos.y > -0.3f)
                _camera.offsetPos.y -= Time.deltaTime * CrouchSpeed;
            if(_camera.offsetPos.y <= -0.3f)
            {
                _camera.offsetPos.y = -0.3f;
                moveSpeed = crouchMoveSpeed;
            }
                
        }          
        else if (_camera.offsetPos.y < 1 && isOnGround)
        {
            _camera.offsetPos.y += Time.deltaTime * CrouchSpeed;
            if(_camera.offsetPos.y >= 1)
            {
                _camera.offsetPos.y = 1;
                moveSpeed = walkMoveSpeed;
            }
                
        }

        if (_camera.offsetPos.y >= 1 && Input.GetButtonDown("Jump") && isOnGround && !isCeiling)
        {
            velocity.y = JumpSpeed;
            isOnGround = false;
        }
        
        velocity.y -= gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
