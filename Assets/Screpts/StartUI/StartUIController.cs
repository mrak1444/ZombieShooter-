using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIController : MonoBehaviourPunCallbacks //MonoBehaviour
{
    [Header("Background sound")]
    [SerializeField] private AudioSource _backgroundSound;


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

    private PhotonView _photonView;
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
    private List<Player> _players = new List<Player>();
    private List<RoomInfo> _roomList = new List<RoomInfo>();
    private bool _flag1, _flag2, _flag3 = false; 


    private void Start()
    {
        _photonView = PhotonView.Get(this);

        //Account
        _signInAccountButton.onClick.AddListener(SignInAccountButton);
        _createAccountButton.onClick.AddListener(CreateAccountButton);

        //SignIn
        _backSignInButton.onClick.AddListener(BackSignInButton);
        _signInSignInButton.onClick.AddListener(SignInSignInButton);

        //CreateAccount
        _backCreateAccountButton.onClick.AddListener(BackCreateAccountButton);
        _okCreateAccountButton.onClick.AddListener(OkCreateAccountButton);
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

            //_playerInfo.SetActive(true);
            //_signIn.SetActive(false);
            
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

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        GameProfile.ResultGetAccountInfo = result;
        _flag1 = true;
    }

    private void OnGetInventorySuccess(GetUserInventoryResult obj)
    {
        GameProfile.ResultGetUserInventory = obj;
        _flag2 = true;
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult obj)
    {
        GameProfile.ResultGetCatalogItems = obj;
        _flag3 = true;
    }

    #endregion

    private void Update()
    {
        if (_flag1 && _flag2 && _flag3) SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

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
}
