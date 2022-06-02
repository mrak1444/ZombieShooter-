using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordControl : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Animator _anim;
    private CharacterController _controller;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _mouserotation = Vector2.zero;
    private bool _shouldMove;
    private Vector3 _rotat = new Vector3(0, 0, 0);

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float _rotationSpeed = 180;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotation();
    }

    private void Move()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");
        _velocity.y = Input.GetAxis("Jump");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _velocity.z *= 2;
        }

        groundedPlayer = _controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (_velocity.y > 0 && groundedPlayer)
        {
            playerVelocity.y += 10; //Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        //playerVelocity.y += gravityValue * Time.deltaTime;

        _shouldMove = _velocity.x > 0.1f || _velocity.x < -0.1f || _velocity.z > 0.1f || _velocity.z < -0.1f;

        _anim.SetBool("Move", _shouldMove);
        _anim.SetFloat("velX", _velocity.x);
        _anim.SetFloat("velY", _velocity.z);

        playerVelocity = transform.right * _velocity.x * playerSpeed + transform.forward * _velocity.z * playerSpeed + transform.up * _velocity.y;

        _controller.SimpleMove(playerVelocity);
    }

    private void Rotation()
    {
        var num = new Vector3(Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime, 0);

        this.transform.Rotate(new Vector3(0, num.y, 0));

        cam.transform.Rotate(new Vector3(-num.x, 0, 0));
    }
}
