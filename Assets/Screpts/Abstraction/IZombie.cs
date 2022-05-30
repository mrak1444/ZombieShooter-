using UnityEngine;
using UnityEngine.AI;

public interface IZombie
{
    Vector3 zombiePosition { get; }
    NavMeshAgent navMeshUnite { get; }
    Vector3 nextPosition { get; set; }
    bool StopUnite { set; }
}