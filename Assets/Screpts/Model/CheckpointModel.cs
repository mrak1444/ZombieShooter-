using UnityEngine;

public class CheckpointModel : MonoBehaviour
{
    [SerializeField] private Transform[] _checkpoints;

    public Transform[] Checkpoints => _checkpoints;
}
