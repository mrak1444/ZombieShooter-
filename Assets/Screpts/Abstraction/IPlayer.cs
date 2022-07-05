using UnityEngine;

public interface IPlayer
{
    int Health { get; set; }
    Vector3 PlayerPosition { get; }
    Transform AttackPoint { get; }
    bool PlayerDie { get; set; }
    bool PlayerOff { get; set; }
    bool PlayerIsMine { get; }
    int NumberKilledZombie { get; set; }
}