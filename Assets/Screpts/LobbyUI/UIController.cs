using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviourPunCallbacks //MonoBehaviour
{
    [Header("Account")]
    [SerializeField] private GameObject _account;
    [SerializeField] private Button _signInAccountButton;
    [SerializeField] private Button _createAccountButton;

    [Header("SignIn")]
    [SerializeField] private GameObject _signIn;
    [SerializeField] private Button _backSignInButton;
    [SerializeField] private Button _signInSignInButton;
    [SerializeField] private InputField _usernameSignIn;
    [SerializeField] private InputField _passwordSignIn;

    [Header("CreateAccount")]
    [SerializeField] private GameObject _createAccount;
    [SerializeField] private Button _backCreateAccountButton;
    [SerializeField] private Button _okCreateAccountButton;
    [SerializeField] private InputField _emailCreateAccount;
    [SerializeField] private InputField _usernameCreateAccount;
    [SerializeField] private InputField _passwordCreateAccount;

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

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
    private Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();
    private List<ItemInstance> _inventoryList = new List<ItemInstance>();
    private string _nameForBuy;
    private int _damageForBuy;
    private int _rangeForBuy;
    private string _itemIdForBuy;
    private int _priceForBuy;
    private bool _connectedToPhotone = false;
    private RoomOptions _roomOptions = new RoomOptions();
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);


    private void Start()
    {
        GameProfile.FlagGameOff.SubscribeOnChange(GameOff);

        //Account
        _signInAccountButton.onClick.AddListener(SignInAccountButton);
        _createAccountButton.onClick.AddListener(CreateAccountButton);

        //SignIn
        _backSignInButton.onClick.AddListener(BackSignInButton);
        _signInSignInButton.onClick.AddListener(SignInSignInButton);

        //CreateAccount
        _backCreateAccountButton.onClick.AddListener(BackCreateAccountButton);
        _okCreateAccountButton.onClick.AddListener(OkCreateAccountButton);

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
    }

    private void GameOff(bool obj)
    {
        if (obj)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetInventorySuccess, OnFailure);

            _playerInfo.SetActive(true);
            _allAccount.SetActive(true);
            //SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            SceneManager.UnloadSceneAsync("Game");
        }
    }



    #region [Account]
    private void SignInAccountButton()
    {
        _signIn.SetActive(true);
        _account.SetActive(false);
    }

    private void CreateAccountButton()
    {
        _createAccount.SetActive(true);
        _account.SetActive(false);
    }

    #endregion

    #region [SignIn]

    private void BackSignInButton()
    {
        _account.SetActive(true);
        _signIn.SetActive(false);
    }

    private void SignInSignInButton()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _usernameSignIn.text,
            Password = _passwordSignIn.text
        }, result =>
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetInventorySuccess, OnFailure);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
            
            Debug.Log($"Success: {_usernameSignIn.text}");

            _playerInfo.SetActive(true);
            _signIn.SetActive(false);
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });

        //подключается к серверам поле удачного подключения открывает панель PlayerInfo

    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetInventorySuccess(GetUserInventoryResult obj)
    {
        _inventoryList = obj.Inventory;
        _virtualCurrency = obj.VirtualCurrency;
        _moneyInventoryTxt.text = _virtualCurrency["GD"].ToString();
    }

    #endregion

    #region [CreateAccount]

    private void BackCreateAccountButton()
    {
        _account.SetActive(true);
        _createAccount.SetActive(false);
    }

    private void OkCreateAccountButton()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _usernameCreateAccount.text,
            Email = _emailCreateAccount.text,
            Password = _passwordCreateAccount.text,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            _account.SetActive(true);
            _createAccount.SetActive(false);
            Debug.Log($"Success: {_usernameCreateAccount.text}");
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });

        //создает нового пользователя в playfab и выходит назад в меню

    }

    #endregion

    #region [PlayerInfo]

    private void BackPlayerInfoButton()
    {
        _account.SetActive(true);
        _playerInfo.SetActive(false);
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
        //StartPhotoneServer(result.AccountInfo.Username);
    }

    #endregion

    #region [Inventory]

    private void AddInventoryButton()
    {
        _nameGunPlayerInfoTxt.text = _nameForBuy;
        Debug.Log(_itemIdForBuy);
        //добавляется оруужие в меню и выходит назад в меню
        _inventory.SetActive(false);
        _playerInfo.SetActive(true);
    }

    private void BuyYesInventoryButton()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            // In your game, this should just be a constant matching your primary catalog
            CatalogVersion = "2",
            ItemId = _itemIdForBuy,
            Price = _priceForBuy,
            VirtualCurrency = "GD"
        }, LogSuccess => 
        {
            _nameGunPlayerInfoTxt.text = _nameForBuy;
            Debug.Log("Gun buyed");
        }
        , LogFailure => 
        {
            Debug.LogError("Gun not buyed");
        });



        //покупается оружие и добавляется на сервер клиету и выходит назад в меню
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
        Debug.Log($"Catalog was loaded successfully!");
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
        }

        CatalogFormation();
    }

    private void CatalogFormation()
    {
        var ii = _catalog.Count < 4 ? _catalog.Count : 4;

        for (int i = 0; i < ii; i++)
        {
            _gunsInventoryImage[i].sprite = _gunIcons[i];
            //Debug.Log(_catalog["gun"+(i+1).ToString()].DisplayName);
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
                if (cat.ItemId.Equals(_inventoryList[i].ItemId))     //if(cat.ItemId == _inventoryList[i].ItemId)
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
        GameProfile.GunId = _itemIdForBuy;
        GameProfile.GunDamage = _damageForBuy;
        GameProfile.GunRange = _rangeForBuy;

        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        _allAccount.SetActive(false);
        _gameMode.SetActive(false);
        GameProfile.FlagGameOff.Value = false;
    }

    #endregion

    #region [Photone Start]

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
        _connectedToPhotone = true;
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
