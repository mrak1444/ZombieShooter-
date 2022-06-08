using Newtonsoft.Json.Linq;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
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
    [SerializeField] private Button _buyYesInventoryButton;
    [SerializeField] private Button _backInventoryButton;
    [SerializeField] private TMP_Text _moneyInventoryTxt;

    [Header("GameMode")]
    [SerializeField] private GameObject _gameMode;
    [SerializeField] private Button _singleplayerGameModeButton;
    [SerializeField] private Button _multiplayerGameModeButton;

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();


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

        //PlayerInfo
        _backPlayerInfoButton.onClick.AddListener(BackPlayerInfoButton);
        _okPlayerInfoButton.onClick.AddListener(OkPlayerInfoButton);
        _buyGunPlayerInfoButton.onClick.AddListener(BuyGunPlayerInfoButton);

        //Inventory
        _buyYesInventoryButton.onClick.AddListener(BuyYesInventoryButton);
        _backInventoryButton.onClick.AddListener(BackInventoryButton);
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
            //PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetInventorySuccess, OnFailure);
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

    /*private void OnGetInventorySuccess(GetUserInventoryResult obj)
    {
        //_nameGunPlayerInfoTxt
        var inv = obj.Inventory;
        if(inv.Count == 0)
        {
            _nameGunPlayerInfoTxt.text = "No gun";
        }
        else
        {
            _nameGunPlayerInfoTxt.text = inv[0].DisplayName;
        }
    }*/

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
    }

    #endregion

    #region [Inventory]

    private void BuyYesInventoryButton()
    {
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
            _gunsInventoryButton[i].onClick.AddListener(delegate { ClickButtons((CatalogItem)cat, (Sprite) spr); });
        }
    }

    private void ClickButtons(CatalogItem cat, Sprite spr)
    {
        _gunSelectedInventoryImage.sprite = spr;
        _nameGunInventoryTxt.text = cat.DisplayName;
        _damageInventoryTxt.text = CustomDataInfo.Info(cat.CustomData).damage.ToString();
        _RangeInventoryTxt.text = CustomDataInfo.Info(cat.CustomData).range.ToString();
        _priceInventoryTxt.text = cat.VirtualCurrencyPrices["GD"].ToString();
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
