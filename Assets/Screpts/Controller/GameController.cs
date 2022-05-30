using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombiesGameObject;
    [SerializeField] private CheckpointModel _checkpointModel;

    private UniteCheckpointController uniteCheckpointController;
    private List<IZombie> _zombie = new List<IZombie>();
    private RndCheckpoint _rndCheckpoint;

    void Start()
    {
        _rndCheckpoint = new RndCheckpoint(_checkpointModel);

        uniteCheckpointController = new UniteCheckpointController(_zombie, _rndCheckpoint);

        foreach (var zombie in _zombiesGameObject)
        {
            _zombie.Add(zombie.GetComponent<IZombie>());
        }
    }

    void Update()
    {
        uniteCheckpointController.Checking();
    }
}
