using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProfile
{
    private static string _playerName;
    private static string _gunId;
    private static int _gunDamage;
    private static int _gunRange;

    public static string PlayerName { get => _playerName; set => _playerName = value; }
    public static string GunId { get => _gunId; set => _gunId = value; }
    public static int GunDamage { get => _gunDamage; set => _gunDamage = value; }
    public static int GunRange { get => _gunRange; set => _gunRange = value; }
}
