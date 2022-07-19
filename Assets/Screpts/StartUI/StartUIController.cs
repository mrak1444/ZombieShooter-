using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIController : MonoBehaviourPunCallbacks
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

    private bool _flag1, _flag2, _flag3 = false; 


    private void Start()
    {
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
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
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
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest()
            {
                VirtualCurrency = "GD",
                Amount = 1000
            };
            _account.SetActive(true);
            _createAccount.SetActive(false);
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }

    #endregion

    private void Update()
    {
        if (_flag1 && _flag2 && _flag3) SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
