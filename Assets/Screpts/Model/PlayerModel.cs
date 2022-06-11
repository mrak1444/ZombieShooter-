using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IPlayer
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerHealth;

    private Animator _anim;
    private int _health = 5;

    public int Health 
    { 
        get => _health;
        set
        {
             _health -= value;
            _playerHealth.text = _health.ToString();
            if (_health <= 0)
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
        _playerName.text = GameProfile.PlayerName;
        _health = GameProfile.PlayerHealth;
        _playerHealth.text = _health.ToString();
    }

    IEnumerator DeathZombie()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
