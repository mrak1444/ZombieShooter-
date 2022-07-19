using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieModelM : MonoBehaviourPunCallbacks, IZombie, IPunObservable
{
    [SerializeField] private int _health = 3;
    [SerializeField] private AudioSource _audioZombie;
    [SerializeField] private AudioClip[] _audioClips;

    private System.Random _rnd = new System.Random();
    private Transform _firstCheckpointTransform;
    private PhotonView _photonView;
    private int _maxHealth;
    private Transform _attackPoint;
    private float _attackWeightR = 0;
    private float _attackWeightL = 0;
    private NavMeshAgent _meshUnite;
    private Animator _anim;
    private Vector3 _nextPosition = new Vector3(0, 0, 0);
    private Vector3 _worldDeltaPosition;
    private bool _shouldMove;
    private bool _stopUniteCheckpointController = false;
    private bool _zombieDie = false;
    private bool _zombieRun = false;
    private bool _zombieAttack = false;
    private bool _falgAccessDeath = true;

    #region PropertiesModel

    public string Name { get => gameObject.name; }
    public bool FalgAccessDeath { get => _falgAccessDeath; set => _photonView.RPC("FalgAccessDeathMulti", RpcTarget.All, value); }
    public Vector3 ZombiePosition => transform.position;
    public Vector3 NextPosition { get => _nextPosition; set => _nextPosition = value; }
    public bool StopUnite 
    {
        set 
        { 
            if (PhotonNetwork.IsMasterClient) _meshUnite.isStopped = value; 
        } 
    }
    public int Health { get => _health; set => _photonView.RPC("HealthMulti", RpcTarget.All, value); }
    public bool StopUniteCheckpointController { get => _stopUniteCheckpointController; set => _stopUniteCheckpointController = value; }
    public Vector3 zombieForward => transform.forward;
    public bool ZombieDie { get => _zombieDie; set => _photonView.RPC("ZombieDieMulti", RpcTarget.All, value); }
    public bool ZombieRun
    {
        get => _zombieRun;
        set
        {
            _zombieRun = value;
            Rnd();
        }
    }
    public bool ZombieAttack
    {
        get => _zombieAttack;
        set
        {
            _zombieAttack = value;
            Rnd();
        }
    }
    public Transform AttackPoint { set => _attackPoint = value; }

    #endregion

    #region MethodModel

    [PunRPC]
    private void FalgAccessDeathMulti(bool falgAccessDeath)
    {
        _falgAccessDeath = falgAccessDeath;
    }

    [PunRPC]
    private void HealthMulti(int health)
    {
        _health = health;
        Rnd();
    }

    [PunRPC]
    private void ZombieDieMulti(bool zombieDie)
    {
        _zombieDie = zombieDie;
        Rnd();
    }

    #endregion

    private void Start()
    {
        _photonView = PhotonView.Get(this);
        _maxHealth = _health;

        if (PhotonNetwork.IsMasterClient)
        {
            _anim = GetComponent<Animator>();
            _anim.SetBool("Move", true);
            _meshUnite = GetComponent<NavMeshAgent>();
            _meshUnite.updatePosition = false;
            _meshUnite.destination = NextPosition;
            _meshUnite.isStopped = false;
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _meshUnite.destination = NextPosition;

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

            if (_zombieDie)
            {
                _anim.SetBool("Death", true);
                StopUnite = true;
            }

            if (!_meshUnite.isStopped)
            {
                _worldDeltaPosition = _meshUnite.nextPosition - transform.position;
                var num = Vector3.Dot(transform.forward, _worldDeltaPosition);
                _shouldMove = (num > 0 ? 1 : 0) > 0 && _meshUnite.remainingDistance > _meshUnite.radius && !_zombieAttack && !_zombieDie && !_zombieRun;
                _anim.SetBool("Move", _shouldMove);
            }
        }
    }

    private void Rnd()
    {
        _audioZombie.clip = _audioClips[_rnd.Next(_audioClips.Length)];
        _audioZombie.Play();
    }

    public void AssignPosition(Transform position)
    {
        _nextPosition = position.position;
        _firstCheckpointTransform = position;
    }

    IEnumerator DeathZombie()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnAnimatorMove()
    {
        if (PhotonNetwork.IsMasterClient) transform.position = _meshUnite.nextPosition;
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
        if (PhotonNetwork.IsMasterClient)
        {
            if (_anim)
            {
                if (_attackPoint != null)
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
    }

    public void Spawn(Vector3 spawnPoint)
    {
        _photonView.RPC("Spawn2", RpcTarget.All, spawnPoint);
    }

    [PunRPC]
    private void Spawn2(Vector3 spawnPoint)
    {
        StartCoroutine(SpawnZombie(spawnPoint));
    }

    IEnumerator SpawnZombie(Vector3 spawnPoint)
    {
        yield return new WaitForSeconds(5f);

        if (PhotonNetwork.IsMasterClient)
        {
            _meshUnite.enabled = false;
            _anim.SetBool("Death", false);
            _anim.SetBool("Move", true);
            transform.position = spawnPoint;
            _nextPosition = _firstCheckpointTransform.position;
            _meshUnite.enabled = true;
            _meshUnite.destination = NextPosition;
        }

        ZombieDie = false;
        _health = _maxHealth;
        StopUnite = false;
        FalgAccessDeath = true;
    }

    public void DisableZombie()
    {
        _photonView.RPC("DisableZombie2", RpcTarget.All);

    }

    [PunRPC]
    private void DisableZombie2()
    {
        if (gameObject.activeSelf) StartCoroutine(DeathZombie());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_health);
            stream.SendNext(_zombieDie);

        }
        else
        {
            this._health = (int)stream.ReceiveNext();
            this._zombieDie = (bool)stream.ReceiveNext();

        }
    }
}