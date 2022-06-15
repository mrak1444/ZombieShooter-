using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombiesGameObject;
    [SerializeField] private CheckpointModel _checkpointModel;
    [SerializeField] private UIControllerGame _uiController;
    [SerializeField] private int _maxZombies = 20;

    private GameObject[] _playerGameObject;
    private UniteCheckpointController uniteCheckpointController;
    private FindPlayerController _findPlayerController;
    private ObjectController _objectController;
    private List<IZombie> _zombie = new List<IZombie>();
    private List<IPlayer> _player = new List<IPlayer>();
    private Dictionary<string, IZombie> _zombie1 = new Dictionary<string, IZombie>();
    private Dictionary<string, IPlayer> _player1 = new Dictionary<string, IPlayer>();
    private RndCheckpoint _rndCheckpoint;

    void Start()
    {
        _playerGameObject = GameObject.FindGameObjectsWithTag("Player");

        _rndCheckpoint = new RndCheckpoint(_checkpointModel);

        uniteCheckpointController = new UniteCheckpointController(_zombie, _rndCheckpoint);
        _findPlayerController = new FindPlayerController(_player, _zombie, _rndCheckpoint);

        foreach (var zombie in _zombiesGameObject)
        {
            _zombie.Add(zombie.GetComponent<IZombie>());
            _zombie1.Add(zombie.name, zombie.GetComponent<IZombie>());
        }

        foreach (var player in _playerGameObject)
        {
            _player.Add(player.GetComponent<IPlayer>());
            _player1.Add(player.name, player.GetComponent<IPlayer>());
        }

        _uiController.Run(_player1["Swat"].Health, _maxZombies);

        _objectController = new ObjectController(_zombie1, _player1, _uiController, _maxZombies);

        Debug.Log($"Player name: {GameProfile.PlayerName}");
    }

    void Update()
    {
        uniteCheckpointController.Checking();
        _findPlayerController.Checking();
        _objectController.Checking();
    }
}
