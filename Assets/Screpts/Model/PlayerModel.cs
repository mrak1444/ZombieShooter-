using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IPlayer
{
    [SerializeField] private int _health = 20;

    public int Health { get => _health; set => _health -= value; }

    public Vector3 PlayerPosition => transform.position;
}
