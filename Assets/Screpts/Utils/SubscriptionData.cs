using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscriptionData
{
    public string PlayerId;
    public string ZombieId;
    public int Damage;

    public SubscriptionData (string playerId, string zombieId, int damage)
    {
        PlayerId = playerId;
        ZombieId = zombieId;
        Damage = damage;
    }
}
