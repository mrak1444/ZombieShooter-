using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiNavigationControl : MonoBehaviour
{
    [SerializeField] private GameObject _checkPoint;

    private Animator _anim;
    private NavMeshAgent _agent;
    private Vector3 _worldDeltaPosition;
    private Vector3 _groundDeltaPocition;
    private Vector2 _velocity = Vector2.zero;
    private bool _shouldMove;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.destination = _checkPoint.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        _worldDeltaPosition = _agent.nextPosition - transform.position;
        _groundDeltaPocition.x = Vector3.Dot(transform.right, _worldDeltaPosition);
        _groundDeltaPocition.y = Vector3.Dot(transform.forward, _worldDeltaPosition);

        //_velocity = (Time.deltaTime > 1e-5f) ? _groundDeltaPocition / Time.deltaTime : Vector2.zero;
        if (Time.deltaTime > 1e-5f)
            _velocity = _groundDeltaPocition / Time.deltaTime;
        else
            _velocity = Vector2.zero;

        _shouldMove = _velocity.magnitude > 0.025 && _agent.remainingDistance > _agent.radius;

        _anim.SetBool("Move", _shouldMove);
        _anim.SetFloat("velX", _velocity.x);
        _anim.SetFloat("velY", _velocity.y);
    }

    private void OnAnimatorMove()
    {
        transform.position = _agent.nextPosition;
    }
}
