using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerController
{
    private List<IZombie> _zombie;
    private List<IPlayer> _player;
    private float _distanceZP = 0;
    private RndCheckpoint _rndCheckpoint;

    public FindPlayerController(List<IPlayer> player, List<IZombie> zombie, RndCheckpoint rndCheckpoint)
    {
        _player = player;
        _zombie = zombie;
        _rndCheckpoint = rndCheckpoint;

        Debug.Log($"player - {_player} / zombie - {_zombie}");
    }

    public void Checking()
    {
        if (_zombie.Count == 0)
        {
            return;
        }

        foreach (var zombie in _zombie)
        {
            var d = Vector3.Distance(zombie.ZombiePosition, _player[0].PlayerPosition);
            var player = _player[0];

            for (int i = 0; i < _player.Count-1; i++)
            {
                if (d > Vector3.Distance(zombie.ZombiePosition, _player[i + 1].PlayerPosition))
                {
                    d = Vector3.Distance(zombie.ZombiePosition, _player[i + 1].PlayerPosition);
                    player = _player[i + 1];
                }
            }

            var vecPZ = player.PlayerPosition - zombie.ZombiePosition;

            if (!zombie.ZombieDie)
            {
                if (Vector3.Angle(vecPZ, zombie.zombieForward) <= 55f)
                {
                    if (Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) < 20f && Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) > 3f)
                    {
                        zombie.StopUniteCheckpointController = true;
                        //zombie.navMeshUnite.destination = player.PlayerPosition;
                        zombie.NextPosition = player.PlayerPosition;
                        zombie.ZombieRun = true;
                        zombie.ZombieAttack = false;
                        zombie.StopUnite = false;
                        zombie.AttackPoint = null;
                    }
                    else if (Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) <= 3f && Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) > 1f)
                    {
                        zombie.ZombieRun = false;
                        zombie.ZombieAttack = true;
                        zombie.StopUnite = false;
                        zombie.AttackPoint = player.AttackPoint;
                    }
                    else if (Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) <= 1f)
                    {
                        zombie.ZombieAttack = true;
                        zombie.StopUnite = true;
                        zombie.ZombieRun = false;
                        zombie.AttackPoint = null;
                    }
                    else
                    {
                        zombie.StopUniteCheckpointController = false;
                        //zombie.navMeshUnite.destination = zombie.NextPosition;
                        zombie.NextPosition = player.PlayerPosition;
                        zombie.ZombieRun = false;
                        zombie.ZombieAttack = false;
                        zombie.StopUnite = false;
                        zombie.AttackPoint = null;
                    }
                }
                else
                {
                    if (Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) < 5f)
                    {
                        zombie.StopUniteCheckpointController = true;
                        //zombie.navMeshUnite.destination = player.PlayerPosition;
                        zombie.NextPosition = player.PlayerPosition;
                        zombie.ZombieRun = true;
                        zombie.ZombieAttack = false;
                        zombie.StopUnite = false;
                        zombie.AttackPoint = null;
                    }
                    else
                    {
                        zombie.StopUniteCheckpointController = false;
                        //zombie.navMeshUnite.destination = zombie.NextPosition;
                        zombie.NextPosition = player.PlayerPosition;
                        zombie.ZombieAttack = false;
                        zombie.StopUnite = false;
                        zombie.ZombieRun = false;
                        zombie.AttackPoint = null;
                    }
                }
            }
        }
    }
}
