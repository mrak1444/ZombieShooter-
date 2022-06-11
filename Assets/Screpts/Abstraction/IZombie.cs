using UnityEngine;
using UnityEngine.AI;

public interface IZombie
{
    string ZombieName { get; }
    Vector3 zombieForward { get; }
    Vector3 zombiePosition { get; }
    NavMeshAgent navMeshUnite { get; }
    Vector3 nextPosition { get; set; }
    bool StopUnite { get; set; }
    bool StopUniteCheckpointController { get;  set; }
    bool ZombieDie { get; set; }
    bool ZombieRun { get; set; }
    bool ZombieAttack { get; set; }
    int Health { get; set; } 
    Transform AttackPoint { set; }
    void AttackWeightR(float weight);
    void AttackWeightL(float weight);
}