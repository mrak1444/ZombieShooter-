using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController
{
    private GameObject[] _spawnPoints;
    private Dictionary<string, IZombie> _zombie;
    private Dictionary<string, IPlayer> _player;
    private UIControllerGame _uiController;
    private int _killZombieAll = 0;
    private int _killZombiePlayer = 0;
    private int _maxZombies;
    private System.Random _rnd = new System.Random();
    private bool _flagEndGame;

    public ObjectController(Dictionary<string, IZombie> zombie, Dictionary<string, IPlayer> player, 
        UIControllerGame uiController, int maxZombies, GameObject[] spawnPoints)
    {
        _spawnPoints = spawnPoints;
        _player = player;
        _zombie = zombie;
        _uiController = uiController;
        _maxZombies = maxZombies;

        GameProfile.DamageForPlayer.SubscribeOnChange(DamageForPlayer);
        GameProfile.DamageForZombie.SubscribeOnChange(DamageForZombie);

        _flagEndGame = true;
    }

    private void DamageForZombie(SubscriptionData obj)
    {
        _zombie[obj.ZombieId].Health -= obj.Damage;
        if(_zombie[obj.ZombieId].Health <= 0 && !_zombie[obj.ZombieId].ZombieDie)
        {
            _killZombieAll += 1;
            _zombie[obj.ZombieId].ZombieDie = true;
            _uiController.KillZombie(_killZombieAll);

            if(obj.PlayerId == "Swat") // переделать под мультиплеер
            {
                _killZombiePlayer += 1;
            }
        }
    }

    private void DamageForPlayer(SubscriptionData obj)
    {
        _player[obj.PlayerId].Health -= obj.Damage;
        _uiController.HealthPlayer(_player[obj.PlayerId].Health);

        if (_player[obj.PlayerId].Health <= 0 && !_player[obj.PlayerId].PlayerDie)
        {
            _player[obj.PlayerId].PlayerDie = true;
        }
    }

    public void Checking()
    {
        if(_killZombieAll == _maxZombies || _player["Swat"].PlayerDie)
        {
            if (_flagEndGame)
            {
                _flagEndGame = false;
                _uiController.EndGameKillZombie(_killZombiePlayer);   //переделать на мультиплеер
            }
            
        }

        foreach (var z in _zombie)
        {
            if (z.Value.ZombieDie && z.Value.FalgAccessDeath)
            {
                z.Value.FalgAccessDeath = false;

                if ((_killZombieAll + _zombie.Count) <= _maxZombies)
                {
                    z.Value.Spawn(_spawnPoints[_rnd.Next(_spawnPoints.Length)].transform.position);
                }
                else //if ((_killZombie + _zombie.Count) > _maxZombies)
                {
                    z.Value.DisableZombie();
                }
                
            }
        }
    }
}
