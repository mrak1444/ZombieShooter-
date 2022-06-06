using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArmModel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.Health = 1;
        }
    }
}
