using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _singlePlayer;
    [SerializeField] private GameObject _multyPlayer;
    [SerializeField] private Transform _spawnPlayer;
    [SerializeField] private ZombiesGameObjectModel[] _zombiesGameObjectModel;
    [SerializeField] private CheckpointModel _checkpointModel;
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private UIControllerGame _uiController;

    private GameObject[] _zombiesGameObject;
    private int _maxZombies;
    private GameObject[] _playerGameObject;
    private UniteCheckpointController _uniteCheckpointController;
    private FindPlayerController _findPlayerController;
    private ObjectController _objectController;
    private List<IZombie> _zombie = new List<IZombie>();
    private List<IPlayer> _player = new List<IPlayer>();
    private Dictionary<string, IZombie> _zombie1 = new Dictionary<string, IZombie>();
    private Dictionary<string, IPlayer> _player1 = new Dictionary<string, IPlayer>();
    private RndCheckpoint _rndCheckpoint;
    private bool _multyFlag = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (GameProfile.GameMode == GameMode.Singeplayer) SingeplayerStart();
        else if (GameProfile.GameMode == GameMode.Multiplayer) MultiplayerStart();
        else Debug.LogError("Error GameMode");
    }

    private void SingeplayerStart()
    {
        _multyFlag = false;

        _maxZombies = GameProfile.MaxZombies;

        _playerGameObject = new GameObject[1];
        _playerGameObject[0] = Instantiate(_singlePlayer, _spawnPlayer.position, Quaternion.identity);

        _rndCheckpoint = new RndCheckpoint(_checkpointModel);

        _uniteCheckpointController = new UniteCheckpointController(_zombie, _rndCheckpoint);
        _findPlayerController = new FindPlayerController(_player, _zombie);

        _zombiesGameObject = Instantiate(_zombiesGameObjectModel[0], new Vector3(0, 0, 0), Quaternion.identity).ZombiesGameObject;

        foreach (var zombie in _zombiesGameObject)
        {
            var z = zombie.GetComponent<IZombie>();
            z.AssignPosition(_rndCheckpoint.RND());
            _zombie.Add(z);
            _zombie1.Add(zombie.name, z);
        }

        foreach (var player in _playerGameObject)
        {
            _player.Add(player.GetComponent<IPlayer>());
            _player1.Add(player.name, player.GetComponent<IPlayer>());
        }

        _uiController.Run(_player1[_playerGameObject[0].name].Health, _maxZombies);

        _objectController = new ObjectController(_playerGameObject[0].name, _zombie1, _player1, _uiController, _maxZombies, _spawnPoints);

        Debug.Log($"Player name: {GameProfile.PlayerName}");
    }

    private void MultiplayerStart()
    {
        _multyFlag = true;

        _maxZombies = GameProfile.MaxZombies;

        var p = PhotonNetwork.Instantiate(_multyPlayer.name, _spawnPlayer.position, Quaternion.identity);
        p.name = GameProfile.PlayerName;
        _player.Add(p.GetComponent<IPlayer>());
        _player1.Add(p.name, p.GetComponent<IPlayer>());

        if (PhotonNetwork.IsMasterClient) _zombiesGameObject = PhotonNetwork.Instantiate(_zombiesGameObjectModel[1].name, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<ZombiesGameObjectModel>().ZombiesGameObject;

        StartCoroutine(Start2(p));
    }

    private IEnumerator Start2(GameObject p)
    {
        yield return new WaitForSeconds(0.8f);
        _playerGameObject = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in _playerGameObject)
        {
            if (player.name != p.name)
            {
                _player.Add(player.GetComponent<IPlayer>());
                _player1.Add(player.name, player.GetComponent<IPlayer>());
            }
        }

        if (PhotonNetwork.IsMasterClient) _rndCheckpoint = new RndCheckpoint(_checkpointModel);

        if (!PhotonNetwork.IsMasterClient) _zombiesGameObject = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (var zombie in _zombiesGameObject)
        {
            var z = zombie.GetComponent<IZombie>();
            if (PhotonNetwork.IsMasterClient) z.AssignPosition(_rndCheckpoint.RND());
            _zombie.Add(z);
            _zombie1.Add(zombie.name, z);
            if (!PhotonNetwork.IsMasterClient) Debug.Log($"zombie name - {zombie.name}");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _uniteCheckpointController = new UniteCheckpointController(_zombie, _rndCheckpoint);
            _findPlayerController = new FindPlayerController(_player, _zombie);
        }

        _uiController.Run(_player1[p.name].Health, _maxZombies);

        _objectController = new ObjectController(p.name, _zombie1, _player1, _uiController, _maxZombies, _spawnPoints);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Confined;
                _uiController.ESCPanel.SetActive(true);
            }
            else 
            { 
                Cursor.lockState = CursorLockMode.Locked;
                _uiController.ESCPanel.SetActive(false);
            }
        }


        if (_multyFlag)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if(_uniteCheckpointController != null) _uniteCheckpointController.Checking();
                if(_findPlayerController != null) _findPlayerController.Checking();
            }
        }
        else
        {
            if (_uniteCheckpointController != null) _uniteCheckpointController.Checking();
            if (_findPlayerController != null) _findPlayerController.Checking();
        }
        

        if(_objectController != null) _objectController.Checking();
    }
}
