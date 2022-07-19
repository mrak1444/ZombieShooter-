using UnityEngine;

public class AttackArmModel : MonoBehaviour
{
    [SerializeField] private GameObject _zombieObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameProfile.DamageForPlayer.Value = new SubscriptionData(other.gameObject.name, _zombieObj.name, 1);
        }
    }
}
