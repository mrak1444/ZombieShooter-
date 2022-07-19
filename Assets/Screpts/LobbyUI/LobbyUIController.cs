using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviourPunCallbacks
{
    [Header("Background sound")]
    [SerializeField] private AudioSource _backgroundSound;
    [SerializeField] private Button _quitGameButton;

    [Header("PlayerInfo")]
    [SerializeField] private GameObject _playerInfo;
    [SerializeField] private Button _backPlayerInfoButton;
    [SerializeField] private Button _okPlayerInfoButton;
    [SerializeField] private Button _buyGunPlayerInfoButton;
    [SerializeField] private Image _IconPlayerPlayerInfoImage;
    [SerializeField] private TMP_Text _namePlayerPlayerInfoTxt;
    [SerializeField] private TMP_Text _nameGunPlayerInfoTxt;

    [Header("Inventory")]
    [SerializeField] private GameObject _inventory;
    [SerializeField] private Sprite[] _gunIcons;
    [SerializeField] private Button[] _gunsInventoryButton;
    [SerializeField] private Image[] _gunsInventoryImage;
    [SerializeField] private Image _gunSelectedInventoryImage;
    [SerializeField] private TMP_Text _nameGunInventoryTxt;
    [SerializeField] private TMP_Text _damageInventoryTxt;
    [SerializeField] private TMP_Text _RangeInventoryTxt;
    [SerializeField] private TMP_Text _priceInventoryTxt;
    [SerializeField] private Button _AddInventoryButton;
    [SerializeField] private Button _buyYesInventoryButton;
    [SerializeField] private GameObject _buyNoInventoryButton;
    [SerializeField] private Button _backInventoryButton;
    [SerializeField] private TMP_Text _moneyInventoryTxt;

    [Header("GameMode")]
    [SerializeField] private GameObject _gameMode;
    [SerializeField] private GameObject _allAccount;
    [SerializeField] private Button _singleplayerGameModeButton;
    [SerializeField] private Button _multiplayerGameModeButton;

    [Header("MultiplayerMenu")]
    [SerializeField] private GameObject _multiplayerMenu;
    [SerializeField] private Button _createRoomeMultiplayerMenuButton;
    [SerializeField] private Button _backMultiplayerMenuButton;
    [SerializeField] private TMP_Text[] _buttonsRoomsTxt;
    [SerializeField] private Button[] _buttonsRooms;

    [Header("MultiplayerStartGame")]
    [SerializeField] private GameObject _multiplayerStartGame;
    [SerializeField] private TMP_Text[] _playersTxtMultiplayerStartGame;
    [SerializeField] private Button[] _PlayersButtonsMultiplayerStartGame;
    [SerializeField] private Button _startGameMultiplayerStartGameButton;
    [SerializeField] private GameObject _waitInfoMultiplayerStartGame;

    private PhotonView _photonView;
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
    private Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();
    private List<ItemInstance> _inventoryList = new List<ItemInstance>();
    private string _nameForBuy;
    private int _damageForBuy;
    private int _rangeForBuy;
    private string _itemIdForBuy;
    private int _priceForBuy;
    private bool _clickButtonRooms = false;
    private bool _updateRooms = false;
    private RoomOptions _roomOptions = new RoomOptions();
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private List<Player> _players = new List<Player>();
    private List<RoomInfo> _roomList = new List<RoomInfo>();


    private void Start()
    {
        _multiplayerMenu.SetActive(false);
        _clickButtonRooms = false;
        _updateRooms = false;

        _photonView = PhotonView.Get(this);

        _quitGameButton.onClick.AddListener(QuitGameButton);

        //Start
        GetAccountSuccess(GameProfile.ResultGetAccountInfo);
        GetInventorySuccess(GameProfile.ResultGetUserInventory);
        GetCatalogSuccess(GameProfile.ResultGetCatalogItems);

        //PlayerInfo
        _backPlayerInfoButton.onClick.AddListener(BackPlayerInfoButton);
        _okPlayerInfoButton.onClick.AddListener(OkPlayerInfoButton);
        _buyGunPlayerInfoButton.onClick.AddListener(BuyGunPlayerInfoButton);

        //Inventory
        _AddInventoryButton.onClick.AddListener(AddInventoryButton);
        _buyYesInventoryButton.onClick.AddListener(BuyYesInventoryButton);
        _backInventoryButton.onClick.AddListener(BackInventoryButton);

        //GameMode
        _singleplayerGameModeButton.onClick.AddListener(SingleplayerGameModeButton);
        _multiplayerGameModeButton.onClick.AddListener(MultiplayerGameModeButton);

        //MultiplayerMenu
        _createRoomeMultiplayerMenuButton.onClick.AddListener(CreateRoomeMultiplayerMenuButton);
        _backMultiplayerMenuButton.onClick.AddListener(BackMultiplayerMenuButton);

        //MultiplayerStartGame
        _startGameMultiplayerStartGameButton.onClick.AddListener(StartGameMultiplayerStartGameButton);
    }

    private void QuitGameButton()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (_clickButtonRooms && _updateRooms) 
        { 
            _multiplayerMenu.SetActive(true);
            _clickButtonRooms = false;
        }
    }

    #region [Start]

    private void GetAccountSuccess(GetAccountInfoResult result)
    {
        _namePlayerPlayerInfoTxt.text = result.AccountInfo.Username;
        GameProfile.PlayerName = result.AccountInfo.Username;
    }

    private void GetInventorySuccess(GetUserInventoryResult obj)
    {
        _inventoryList = obj.Inventory;
        _virtualCurrency = obj.VirtualCurrency;
        _moneyInventoryTxt.text = _virtualCurrency["GD"].ToString();
    }

    private void GetCatalogSuccess(GetCatalogItemsResult obj)
    {
        HandleCatalog(obj.Catalog);
    }

    #endregion

    #region [PlayerInfo]

    private void BackPlayerInfoButton()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }

    private void OkPlayerInfoButton()
    {
        _gameMode.SetActive(true);
        _playerInfo.SetActive(false);
    }

    private void BuyGunPlayerInfoButton()
    {
        _inventory.SetActive(true);
        _playerInfo.SetActive(false);
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _namePlayerPlayerInfoTxt.text = result.AccountInfo.Username;
        GameProfile.PlayerName = result.AccountInfo.Username;
    }

    #endregion

    #region [Inventory]

    private void AddInventoryButton()
    {
        _nameGunPlayerInfoTxt.text = _nameForBuy;
        _inventory.SetActive(false);
        _playerInfo.SetActive(true);
    }

    private void BuyYesInventoryButton()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "2",
            ItemId = _itemIdForBuy,
            Price = _priceForBuy,
            VirtualCurrency = "GD"
        }, LogSuccess => 
        {
            _nameGunPlayerInfoTxt.text = _nameForBuy;
        }
        , LogFailure => 
        {
            Debug.LogError("Gun not buyed");
        });

        _inventory.SetActive(false);
        _playerInfo.SetActive(true);
    }

    private void BackInventoryButton()
    {
        _inventory.SetActive(false);
        _playerInfo.SetActive(true);
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult obj)
    {
        HandleCatalog(obj.Catalog);
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
        }

        CatalogFormation();
    }

    private void CatalogFormation()
    {
        var ii = _catalog.Count < 4 ? _catalog.Count : 4;

        for (int i = 0; i < ii; i++)
        {
            _gunsInventoryImage[i].sprite = _gunIcons[i];
            var cat = _catalog["gun" + (i + 1).ToString()];
            var spr = _gunIcons[i];
            _gunsInventoryButton[i].onClick.AddListener(delegate { ClickButtonsInventory((CatalogItem)cat, (Sprite) spr); });
        }
    }

    private void ClickButtonsInventory(CatalogItem cat, Sprite spr)
    {
        _gunSelectedInventoryImage.sprite = spr;
        _nameGunInventoryTxt.text = cat.DisplayName;
        _nameForBuy = cat.DisplayName;
        _damageInventoryTxt.text = CustomDataInfo.Info(cat.CustomData).damage.ToString();
        _damageForBuy = CustomDataInfo.Info(cat.CustomData).damage;
        _RangeInventoryTxt.text = CustomDataInfo.Info(cat.CustomData).range.ToString();
        _rangeForBuy = CustomDataInfo.Info(cat.CustomData).range;
        _priceInventoryTxt.text = cat.VirtualCurrencyPrices["GD"].ToString();
        _priceForBuy = (int)cat.VirtualCurrencyPrices["GD"];
        _itemIdForBuy = cat.ItemId;

        if (_inventoryList.Count != 0)
        {
            for (int i = 0; i < _inventoryList.Count; i++)
            {
                if (cat.ItemId.Equals(_inventoryList[i].ItemId))
                {
                    _AddInventoryButton.gameObject.SetActive(true);
                    _buyNoInventoryButton.SetActive(false);
                    _buyYesInventoryButton.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    if (_virtualCurrency["GD"] >= cat.VirtualCurrencyPrices["GD"])
                    {
                        _AddInventoryButton.gameObject.SetActive(false);
                        _buyNoInventoryButton.SetActive(false);
                        _buyYesInventoryButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        _AddInventoryButton.gameObject.SetActive(false);
                        _buyNoInventoryButton.SetActive(true);
                        _buyYesInventoryButton.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (_virtualCurrency["GD"] >= cat.VirtualCurrencyPrices["GD"])
            {
                _AddInventoryButton.gameObject.SetActive(false);
                _buyNoInventoryButton.SetActive(false);
                _buyYesInventoryButton.gameObject.SetActive(true);
            }
            else
            {
                _AddInventoryButton.gameObject.SetActive(false);
                _buyNoInventoryButton.SetActive(true);
                _buyYesInventoryButton.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region [GameMode]

    private void SingleplayerGameModeButton()
    {
        _backgroundSound.Stop();

        GameProfile.GunId = _itemIdForBuy;
        GameProfile.GunDamage = _damageForBuy;
        GameProfile.GunRange = _rangeForBuy;
        GameProfile.GameMode = GameMode.Singeplayer;

        SceneManager.LoadScene("Game", LoadSceneMode.Single);

        _allAccount.SetActive(false);
        _gameMode.SetActive(false);
    }

    private void MultiplayerGameModeButton()
    {
        GameProfile.GunId = _itemIdForBuy;
        GameProfile.GunDamage = _damageForBuy;
        GameProfile.GunRange = _rangeForBuy;
        GameProfile.GameMode = GameMode.Multiplayer;
        _clickButtonRooms = true;
        StartPhotoneServer(_namePlayerPlayerInfoTxt.text);
        
    }

    #endregion

    #region [Multiplayer Menu]

    private void BackMultiplayerMenuButton()
    {
        _multiplayerMenu.SetActive(false);
        _playerInfo.SetActive(true);
    }

    private void CreateRoomeMultiplayerMenuButton()
    {
        bool flagRoomName = false;
        string nameRoom = "";
        int n = 0;
        
        do
        {
            flagRoomName = false;
            n++;
            nameRoom = $"{_namePlayerPlayerInfoTxt.text}_{n}";

            foreach (var room in _roomList)
            {
                if (nameRoom == room.Name)
                {
                    flagRoomName = true;
                }
            }
        } 
        while (flagRoomName);

        PhotonNetwork.CreateRoom(nameRoom);  //дописать
        Debug.Log($"Create room name - {nameRoom}");
    }

    private void StartPhotoneServer(string PlayerName)
    {
        PhotonNetwork.NickName = PlayerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photone! OnConnectedToMaster");
        PhotonNetwork.JoinLobby(_customLobby);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinedLobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomList = roomList;
        
        if(roomList.Count != 0)
        {
            for (int i = 0; i < _buttonsRoomsTxt.Length; i++)
            {
                if(roomList.Count > i)
                {
                    _buttonsRoomsTxt[i].text = roomList[i].Name;
                    var n = roomList[i].Name;
                    _buttonsRooms[i].onClick.AddListener(delegate { ClickButtonsJoinRoom((string)n); });
                }
            }
        }
        _gameMode.SetActive(false);
        
        _updateRooms = true;
    }

    private void ClickButtonsJoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnJoinedRoom()
    {
        _multiplayerMenu.SetActive(false);
        _multiplayerStartGame.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            _startGameMultiplayerStartGameButton.gameObject.SetActive(true);
            _waitInfoMultiplayerStartGame.SetActive(false);
        }
        else
        {
            _startGameMultiplayerStartGameButton.gameObject.SetActive(false);
            _waitInfoMultiplayerStartGame.SetActive(true);
        }
    }

    #endregion

    #region [Multiplayer StartGame]

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _players.Add(newPlayer);

        if (_players.Count != 0)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _playersTxtMultiplayerStartGame[i].text = _players[i].NickName;
            }
        }
    }

    private void StartGameMultiplayerStartGameButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        if (PhotonNetwork.IsMasterClient)
        {
             _photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartGame()
    {
        _backgroundSound.Stop();
        _allAccount.SetActive(false);
        _gameMode.SetActive(false);
        _multiplayerStartGame.SetActive(false);
        GameProfile.FlagGameOff.Value = false;

        PhotonNetwork.LoadLevel("Game");
    }

    #endregion
}

public class CustomDataInfo
{
    public int damage;
    public int range;

    public static CustomDataInfo Info (string jsonString)
    {
        return JsonUtility.FromJson<CustomDataInfo>(jsonString);
    }
}
