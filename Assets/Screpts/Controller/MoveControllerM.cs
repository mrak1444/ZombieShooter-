using Photon.Pun;
using UnityEngine;

public class MoveControllerM : MonoBehaviourPun
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject _gunPoint;

    private Animator _anim;
    private CharacterController _controller;
    private Vector3 _velocity = Vector3.zero;
    private bool _shouldMove = false;
    private bool _shouldJump = false;

    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private float _rotationSpeed = 180;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    private float _vertSpeed;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _vertSpeed = minFall;
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
            Rotation();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Jump();
        }
    }

    private void Move()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _velocity.z *= 2;
        }

        _shouldMove = _velocity.x > 0.1f || _velocity.x < -0.1f || _velocity.z > 0.1f || _velocity.z < -0.1f;

        _anim.SetBool("Move", _shouldMove);
        _anim.SetFloat("velX", _velocity.x);
        _anim.SetFloat("velY", _velocity.z);

        playerVelocity = transform.right * _velocity.x * playerSpeed + transform.forward * _velocity.z * playerSpeed + transform.up * _velocity.y;

        _controller.SimpleMove(playerVelocity);
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
                _shouldJump = true;
            }
            else
            {
                _vertSpeed = minFall;
                _shouldJump = false;
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
        }

        if (_vertSpeed < terminalVelocity)
        {
            _vertSpeed = terminalVelocity;
        }

        var movement = new Vector3(0, 0, 0);
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _controller.Move(movement);

        _anim.SetBool("Jump", _shouldJump);
    }

    private void Rotation()
    {
        var num = new Vector3(Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime, 0);
        var vect = new Vector3(-num.x, 0, 0);

        this.transform.Rotate(new Vector3(0, num.y, 0));

        cam.transform.Rotate(vect);
        _gunPoint.transform.Rotate(vect);
    }
}
