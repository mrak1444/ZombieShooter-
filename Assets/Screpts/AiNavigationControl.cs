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
        var num = Vector3.Dot(transform.forward, _worldDeltaPosition);

        _shouldMove = (num > 0 ? 1 : 0) > 0.025 && _agent.remainingDistance > _agent.radius;

        _anim.SetBool("Move", _shouldMove);
    }

    private void OnAnimatorMove()
    {
        transform.position = _agent.nextPosition;
    }
}
