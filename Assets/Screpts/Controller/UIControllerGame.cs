using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIControllerGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthPlayer;
    [SerializeField] private TMP_Text _KillZombie;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _camera;
    [SerializeField] private TMP_Text _infoEndGame;
    [SerializeField] private Button QuitGameButton;
    [SerializeField] private GameObject _escPanel;
    [SerializeField] private Button _escQuitGameButton;

    public GameObject ESCPanel { get => _escPanel; set => _escPanel = value; }

    private int _zombies;

    private void Start()
    {
        QuitGameButton.onClick.AddListener(QuitGame);
        _escQuitGameButton.onClick.AddListener(ESCQuitGame);
    }

    private void ESCQuitGame()
    {
        _escPanel.SetActive(false);
        GameProfile.EndGameFlag = true;
    }

    public void Run(int health, int zombies)
    {
        _zombies = zombies;
        _healthPlayer.text = health.ToString();
        _KillZombie.text = $"0/{_zombies.ToString()}";
    }

    public void HealthPlayer(int health)
    {
        _healthPlayer.text = health.ToString();
        _info.text = "You've been damaged";
    }

    public void KillZombie(int kill)
    {
        _KillZombie.text = $"{kill.ToString()}/{_zombies.ToString()}";
        _info.text = "You kill zombie";
    }

    public void EndGameKillZombie(int killZombies, Dictionary<string, IPlayer> _player, string namePlayer)
    {
        int knp = 0;
        string nameWinner = "Null";

        foreach (var player in _player)
        {
            if (knp < player.Value.NumberKilledZombie) 
            { 
                knp = player.Value.NumberKilledZombie;
                nameWinner = player.Key;
            }
        }

        StartCoroutine(EndGame(killZombies, nameWinner, namePlayer));
    }

    IEnumerator EndGame(int killZombies, string nameWinner, string namePlayer)
    {
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.Confined;
        _endGamePanel.SetActive(true);
        _camera.SetActive(true);
        _infoEndGame.text = $"You kill {killZombies} zombies.";
        yield return new WaitForSeconds(1f);
        _infoEndGame.text += $"\nYou get {killZombies * 10} money.";
        yield return new WaitForSeconds(1f);
        _infoEndGame.text += $"\nFirst place was taken by {nameWinner}.";

        AddUserVirtualCurrencyRequest request;

        if (namePlayer == nameWinner)
        {
            request = new AddUserVirtualCurrencyRequest()
            {
                VirtualCurrency = "GD",
                Amount = (killZombies * 10) + 500 
            };
        }
        else
        {
            request = new AddUserVirtualCurrencyRequest()
            {
                VirtualCurrency = "GD",
                Amount = killZombies * 10
            };
        }
        
        PlayFabClientAPI.AddUserVirtualCurrency
            (request,
            result =>
            {
                Debug.Log(result);
            }, error =>
            {
                Debug.LogError(error);
            });

        
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        GameProfile.ResultGetAccountInfo = result;
    }

    private void OnGetInventorySuccess(GetUserInventoryResult obj)
    {
        GameProfile.ResultGetUserInventory = obj;
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult obj)
    {
        GameProfile.ResultGetCatalogItems = obj;
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void QuitGame()
    {
        if (GameProfile.GameMode == GameMode.Multiplayer) 
        { 
            PhotonNetwork.LeaveRoom();

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetInventorySuccess, OnFailure);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
        }
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
