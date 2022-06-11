using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombiesGameObject;
    //[SerializeField] private GameObject _zombiePref;
    [SerializeField] private CheckpointModel _checkpointModel;
    [SerializeField] private GameObject[] _spawnPointZombie;
    [SerializeField] private int _zombieOnScene = 20;

    private SpawnController _spawnController;
    private GameObject[] _playerGameObject;
    private UniteCheckpointController uniteCheckpointController;
    private FindPlayerController _findPlayerController;
    private List<IZombie> _zombie = new List<IZombie>();
    private List<IPlayer> _player = new List<IPlayer>();
    private RndCheckpoint _rndCheckpoint;
    private int _maxZombie;

    void Start()
    {
        _maxZombie = GameProfile.MaxZombie;

        _playerGameObject = GameObject.FindGameObjectsWithTag("Player");

        _rndCheckpoint = new RndCheckpoint(_checkpointModel);

        //_spawnController = new SpawnController(_zombiePref, _spawnPointZombie, _rndCheckpoint);
        uniteCheckpointController = new UniteCheckpointController(_zombie, _rndCheckpoint);
        _findPlayerController = new FindPlayerController(_player, _zombie, _rndCheckpoint);

        foreach (var zombie in _zombiesGameObject)
        {
            _zombie.Add(zombie.GetComponent<IZombie>());
        }

        foreach (var player in _playerGameObject)
        {
            _player.Add(player.GetComponent<IPlayer>());
        }

        Debug.Log($"Player name: {GameProfile.PlayerName}");
    }

    void Update()
    {
        uniteCheckpointController.Checking();
        _findPlayerController.Checking();

        /*if (_zombie.Count < _zombieOnScene && _maxZombie != 0)
        {
            Debug.Log($"{_zombie.Count < _zombieOnScene}, {_zombie.Count} < {_zombieOnScene}");
            _zombie.Add(_spawnController.SpawnZombie());
            _maxZombie -= 1;
        }*/
    }
}
