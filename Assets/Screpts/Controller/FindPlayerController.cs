using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerController
{
    private List<IZombie> _zombie;
    private List<IPlayer> _player;
    private RndCheckpoint _rndCheckpoint;

    public FindPlayerController(List<IPlayer> player, List<IZombie> zombie, RndCheckpoint rndCheckpoint)
    {
        _player = player;
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
            foreach (var player in _player)
            {
                var vecPZ = player.PlayerPosition - zombie.ZombiePosition;

                if (!zombie.ZombieDie)
                {
                    if (Vector3.Angle(vecPZ, zombie.zombieForward) <= 50f)
                    {
                        if (Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) < 40f && Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) > 2f)
                        {
                            zombie.StopUniteCheckpointController = true;
                            //zombie.navMeshUnite.destination = player.PlayerPosition;
                            zombie.NextPosition = player.PlayerPosition;
                            zombie.ZombieRun = true;
                            zombie.ZombieAttack = false;
                            zombie.StopUnite = false;
                            zombie.AttackPoint = null;
                        }
                        else if(Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) <= 2f && Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) > 1f)
                        {
                            zombie.ZombieAttack = true;
                            zombie.StopUnite = false;
                            zombie.AttackPoint = player.AttackPoint;
                        }
                        else if(Vector3.Distance(zombie.ZombiePosition, player.PlayerPosition) <= 1f)
                        {
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
}
