using System.Collections.Generic;

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
            if (zombie.StopUniteCheckpointController)
            {
                if (zombie.ZombiePosition.x >= zombie.NextPosition.x - 1 && zombie.ZombiePosition.x <= zombie.NextPosition.x + 1 && zombie.ZombiePosition.z >= zombie.NextPosition.z - 1 && zombie.ZombiePosition.z <= zombie.NextPosition.z + 1)
                {
                    zombie.NextPosition = _rndCheckpoint.RND().position;
                }
            }
        }
    }
}
