using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteCheckpointController
{
    private List<IZombie> _zombie;
    private RndCheckpoint _rndCheckpoint;

    public UniteCheckpointController(List<IZombie> zombie, RndCheckpoint rndCheckpoint)
    {
        _zombie = zombie;
        _rndCheckpoint = rndCheckpoint;
    }

    public void Checking()
    {
        if (_zombie.Count == 0)
        {
            return;
        }

        foreach (var zombie in _zombie)
        {
            if (zombie.zombiePosition.x >= zombie.nextPosition.x - 1 && zombie.zombiePosition.x <= zombie.nextPosition.x + 1 && zombie.zombiePosition.z >= zombie.nextPosition.z - 1 && zombie.zombiePosition.z <= zombie.nextPosition.z + 1)
            {
                zombie.nextPosition = _rndCheckpoint.RND().position;
                zombie.navMeshUnite.destination = zombie.nextPosition;
            }
        }
    }
}
