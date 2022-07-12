using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IPlayer
{
    [SerializeField] private int _health = 20;
    [SerializeField] private Transform _attackPoint;

    private Animator _anim;
    private bool _playerDie = false;
    private bool _playerOff = false;
    private bool _playerIsMine;
    private int _numberKilledZombie = 0;

    public string Name { get => gameObject.name; }
    public int Health { get => _health; set => _health = value; }
    public bool PlayerOff 
    { 
        get => _playerOff;
        set
        {
            _playerOff = value;
            if (_playerOff)
            {
                StartCoroutine(DeathZombie(2f));
            }
        }  
    }

    public bool PlayerDie
    {
        get => _playerDie;
        set
        {
            _playerDie = value;
            if (_playerDie)
            {
                _anim.SetBool("Death", true);
                StartCoroutine(DeathZombie(5f));
            }
        }
    }

    public Vector3 PlayerPosition => transform.position;

    public Transform AttackPoint => _attackPoint;

    public bool PlayerIsMine => false;

    public int NumberKilledZombie { get => _numberKilledZombie; set => _numberKilledZombie = value; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    IEnumerator DeathZombie(float n)
    {
        yield return new WaitForSeconds(n);
        gameObject.SetActive(false);
    }
}
