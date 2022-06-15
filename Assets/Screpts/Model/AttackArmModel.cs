using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArmModel : MonoBehaviour
{
    [SerializeField] private GameObject _zombieObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //var player = other.gameObject.GetComponent<IPlayer>();
            //player.Health = 1;
            GameProfile.DamageForPlayer.Value = new SubscriptionData(other.gameObject.name, _zombieObj.name, 1);
        }
    }
}
