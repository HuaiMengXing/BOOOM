using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance => instance;

    public float moveSpeed;
    public float CrouchSpeed;
    public float JumpSpeed;
    public float gravity;
    public float X_rotationSpeed;
    public float Y_rotationSpeed;

    private CameraMove _camera;
    private CharacterController _characterController;

    private Vector3 move;
    private float lookHight;
    private bool isOnGround;
    private Vector3 velocity;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _camera = Camera.main.GetComponent<CameraMove>();
        _characterController = GetComponent<CharacterController>();
        lookHight = _camera.lookBodyHight;
        velocity = Vector3.zero;
        isOnGround = true;
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        Move();
        rotationLook();
        CrouchAndJump();

        if (Input.GetKeyDown(KeyCode.F))
            _camera.Shake();
    }

    //移动
    public void Move()
    {
        move.z = Input.GetAxis("Vertical");
        move.x = Input.GetAxis("Horizontal");
        move.y = 0;
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
        isOnGround = Physics.Raycast(transform.position, -transform.up, 1.5f,1<< LayerMask.NameToLayer("Ground"));
        if (Input.GetKey(KeyCode.LeftControl) && isOnGround)
        {
            if (_camera.offsetPos.y > -0.3f)
                _camera.offsetPos.y -= Time.deltaTime * CrouchSpeed;
            if(_camera.offsetPos.y <= -0.3f)
                _camera.offsetPos.y = -0.3f;
        }          
        else if (_camera.offsetPos.y < 1 && isOnGround)
        {
            _camera.offsetPos.y += Time.deltaTime * CrouchSpeed;
            if(_camera.offsetPos.y >= 1)
                _camera.offsetPos.y = 1;
        }

        if (_camera.offsetPos.y >= 1 && Input.GetButtonDown("Jump") && isOnGround)
        {
            print("sssss" + isOnGround);
            velocity.y = JumpSpeed;
            isOnGround = false;
        }
        
        velocity.y -= gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
