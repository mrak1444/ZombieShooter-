using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordControl : MonoBehaviour
{
    private Animator _anim;
    private float _speedMultiplier = 0.1f;
    private Vector2 _velocity = Vector2.zero;
    private bool _shouldMove;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _velocity.x = _speedMultiplier + Input.GetAxis("Horizontal");
        _velocity.y = _speedMultiplier + Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _velocity.y *= 2;
        }

        _shouldMove = _velocity.sqrMagnitude > Mathf.Epsilon;
        _anim.SetBool("Move", _shouldMove);
        _anim.SetFloat("velX", _velocity.x);
        _anim.SetFloat("velY", _velocity.y);
    }
}
