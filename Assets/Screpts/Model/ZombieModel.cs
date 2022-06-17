using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieModel : MonoBehaviour, IZombie
{
    [SerializeField] private Transform _firstCheckpointTransform;
    [SerializeField] private int _health = 3;

    private int _maxHealth;
    private Transform _attackPoint;
    private float _attackWeightR = 0;
    private float _attackWeightL = 0;
    private NavMeshAgent _meshUnite;
    private Animator _anim;
    private Vector3 _nextPosition;
    private Vector3 _worldDeltaPosition;
    private bool _shouldMove;
    private bool _stopUniteCheckpointController = false;
    private bool _zombieDie = false;
    private bool _zombieRun = false;
    private bool _zombieAttack = false;
    private bool _zombieActive = true;
    private bool _falgAccessDeath = true;


    public bool FalgAccessDeath { get => _falgAccessDeath; set => _falgAccessDeath = value; }
    public Vector3 zombiePosition => transform.position;
    public NavMeshAgent navMeshUnite => _meshUnite;
    public Vector3 nextPosition { get => _nextPosition; set => _nextPosition = value; }
    public bool StopUnite { set => _meshUnite.isStopped = value; }
    public int Health { get => _health; set => _health = value; }

    public bool StopUniteCheckpointController { get => _stopUniteCheckpointController; set => _stopUniteCheckpointController = value; }

    public Vector3 zombieForward => transform.forward;

    public bool ZombieDie 
    { 
        get => _zombieDie;
        set
        { 
            _zombieDie = value;
            if (_zombieDie)
            {
                _anim.SetBool("Death", true);
                StopUnite = true;
            }
        }
    }

    public bool ZombieRun 
    { 
        get => _zombieRun;
        set
        {
            _zombieRun = value;
            if (_zombieRun)
            {
                _anim.SetBool("Run", true);
                _meshUnite.speed = 3f;
            }
            else
            {
                _anim.SetBool("Run", false);
                _meshUnite.speed = 1f;
            }
        }
    }
    public bool ZombieAttack 
    { 
        get => _zombieAttack;
        set
        {
            _zombieAttack = value;
            if (_zombieAttack)
            {
                _anim.SetLayerWeight(1, 1);
                _anim.SetBool("Attack", true);
            }
            else
            {
                _anim.SetLayerWeight(1, 0);
                _anim.SetBool("Attack", false);
            }
        }
    }

    public Transform AttackPoint { set => _attackPoint = value; }

    private void Start()
    {
        _maxHealth = _health;
        _meshUnite = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _meshUnite.updatePosition = false;
        _nextPosition = _firstCheckpointTransform.position;
        _meshUnite.destination = nextPosition;
        _meshUnite.isStopped = false;
        _anim.SetBool("Move", true);
    }

    private void Update()
    {
        if (!_meshUnite.isStopped)
        {
            _worldDeltaPosition = _meshUnite.nextPosition - transform.position;
            var num = Vector3.Dot(transform.forward, _worldDeltaPosition);
            _shouldMove = (num > 0 ? 1 : 0) > 0 && _meshUnite.remainingDistance > _meshUnite.radius && !_zombieAttack && !_zombieDie && !_zombieRun;
            _anim.SetBool("Move", _shouldMove);
        }
    }

    IEnumerator DeathZombie()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnAnimatorMove()
    {
        transform.position = _meshUnite.nextPosition;
    }

    public void AttackWeightR(float weight)
    {
        _attackWeightR = weight;
    }

    public void AttackWeightL(float weight)
    {
        _attackWeightL = weight;
    }

    private void OnAnimatorIK()
    {
        if (_anim)
        {
            if(_attackPoint != null)
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _attackWeightR);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _attackWeightR);
                _anim.SetIKPosition(AvatarIKGoal.RightHand, _attackPoint.position);
                _anim.SetIKRotation(AvatarIKGoal.RightHand, _attackPoint.rotation);

                _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _attackWeightL);
                _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _attackWeightL);
                _anim.SetIKPosition(AvatarIKGoal.LeftHand, _attackPoint.position);
                _anim.SetIKRotation(AvatarIKGoal.LeftHand, _attackPoint.rotation);
            }
        }
    }

    public void Spawn(Vector3 spawnPoint)
    {
        
        StartCoroutine(SpawnZombie(spawnPoint));
    }

    IEnumerator SpawnZombie(Vector3 spawnPoint)
    {
        
        yield return new WaitForSeconds(5f);
        _meshUnite.enabled = false;
        transform.position = spawnPoint;
        ZombieDie = false;
        _anim.SetBool("Death", false);
        _anim.SetBool("Move", true);
        _health = _maxHealth;
        _meshUnite.enabled = true;
        StopUnite = false;
        _nextPosition = _firstCheckpointTransform.position;
        _meshUnite.destination = nextPosition;
        FalgAccessDeath = true;
    }

    public void DisableZombie()
    {
        if(gameObject.activeSelf) StartCoroutine(DeathZombie());
    }
}
