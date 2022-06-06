using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IPlayer
{
    [SerializeField] private int _health = 20;
    [SerializeField] private Transform _attackPoint;

    private Animator _anim;

    public int Health 
    { 
        get => _health;
        set
        {
             _health -= value;
            Debug.Log(_health);
            if(_health <= 0)
            {
                _anim.SetBool("Death", true);
                StartCoroutine(DeathZombie());
            }
        }
    }

    public Vector3 PlayerPosition => transform.position;

    public Transform AttackPoint => _attackPoint;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    IEnumerator DeathZombie()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
