using UnityEngine;

public interface IPlayer
{
    int Health { get; set; }
    Vector3 PlayerPosition { get; }
}