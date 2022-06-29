using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProfile
{
    private static string _playerName;
    private static string _gunId;
    private static int _gunDamage;
    private static int _gunRange;
    private static int _maxZombies = 30;
    private static int _players;
    private static GameMode _gameMode;

    public static string PlayerName { get => _playerName; set => _playerName = value; }
    public static string GunId { get => _gunId; set => _gunId = value; }
    public static int GunDamage { get => _gunDamage; set => _gunDamage = value; }
    public static int GunRange { get => _gunRange; set => _gunRange = value; }

    public static SubscriptionProperty<SubscriptionData> DamageForZombie = new SubscriptionProperty<SubscriptionData>();

    public static SubscriptionProperty<SubscriptionData> DamageForPlayer = new SubscriptionProperty<SubscriptionData>();

    public static SubscriptionProperty<bool> FlagGameOff = new SubscriptionProperty<bool>();
    public static int MaxZombies { get => _maxZombies; set => _maxZombies = value; }
    public static GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public static int Players { get => Players; set => _players = value; }
}
