using UnityEngine;
using UnityEngine.AI;

public class ZombieModel : MonoBehaviour, IZombie
{
    [SerializeField] private Transform _firstCheckpointTransform;

    private NavMeshAgent _meshUnite;
    private Animator _anim;
    private Vector3 _nextPosition;
    private Vector3 _worldDeltaPosition;
    private bool _shouldMove;

    public Vector3 zombiePosition => transform.position;
    public NavMeshAgent navMeshUnite => _meshUnite;
    public Vector3 nextPosition { get => _nextPosition; set => _nextPosition = value; }
    public bool StopUnite { set => _meshUnite.isStopped = value; }

    private void Start()
    {
        _meshUnite = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _meshUnite.updatePosition = false;
        _nextPosition = _firstCheckpointTransform.position;
        _meshUnite.destination = nextPosition;
        _meshUnite.isStopped = false;
    }

    private void Update()
    {
        if (!_meshUnite.isStopped)
        {
            _worldDeltaPosition = _meshUnite.nextPosition - transform.position;
            var num = Vector3.Dot(transform.forward, _worldDeltaPosition);
            _shouldMove = (num > 0 ? 1 : 0) > 0 && _meshUnite.remainingDistance > _meshUnite.radius;
            _anim.SetBool("Move", _shouldMove);
        }
    }

    private void OnAnimatorMove()
    {
        transform.position = _meshUnite.nextPosition;
    }
}
