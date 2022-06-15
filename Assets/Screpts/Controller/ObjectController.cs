using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController
{
    private Dictionary<string, IZombie> _zombie;
    private Dictionary<string, IPlayer> _player;
    private UIControllerGame _uiController;
    private int _killZombie = 0;
    private int _maxZombies;

    public ObjectController(Dictionary<string, IZombie> zombie, Dictionary<string, IPlayer> player, UIControllerGame uiController, int maxZombies)
    {
        _player = player;
        _zombie = zombie;
        _uiController = uiController;
        _maxZombies = maxZombies;

        GameProfile.DamageForPlayer.SubscribeOnChange(DamageForPlayer);
        GameProfile.DamageForZombie.SubscribeOnChange(DamageForZombie);
    }

    private void DamageForZombie(SubscriptionData obj)
    {
        _zombie[obj.ZombieId].Health -= obj.Damage;
        if(_zombie[obj.ZombieId].Health <= 0 && !_zombie[obj.ZombieId].ZombieDie)
        {
            _killZombie += 1;
            _zombie[obj.ZombieId].ZombieDie = true;
            _uiController.KillZombie(_killZombie);
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
        if(_killZombie == _maxZombies || _player["Swat"].PlayerDie)
        {
            _uiController.EndGameKillZombie(_killZombie);   //переделать на мультиплеер
        }
    }
}
