using UnityEngine;
using UnityEngine.AI;

public interface IZombie
{
    Vector3 zombieForward { get; }
    Vector3 zombiePosition { get; }
    NavMeshAgent navMeshUnite { get; }
    Vector3 nextPosition { get; set; }
    bool StopUnite { set; }
    bool StopUniteCheckpointController { get;  set; }
    bool ZombieDie { get; set; }
    bool ZombieRun { get; set; }
    bool ZombieAttack { get; set; }
    int Health { get; set; } 
}