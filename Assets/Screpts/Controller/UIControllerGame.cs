using System;
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
    //[SerializeField] private GameObject _cameraPlayer;
    [SerializeField] private TMP_Text _infoEndGame;
    [SerializeField] private Button QuitGameButton;

    private int _zombies;

    private void Start()
    {
        QuitGameButton.onClick.AddListener(QuitGame);
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
        //_cameraPlayer.SetActive(false);
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

    private void QuitGame()
    {
        if (GameProfile.GameMode == GameMode.Multiplayer) PhotonNetwork.LeaveRoom();  // проверить на работаспособность
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
