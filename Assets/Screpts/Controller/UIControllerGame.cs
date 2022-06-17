using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthPlayer;
    [SerializeField] private TMP_Text _KillZombie;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _cameraPlayer;
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

    public void EndGameKillZombie(int killZombies)
    {
        StartCoroutine(EndGame(killZombies));
        
    }

    IEnumerator EndGame(int killZombies)
    {
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.Confined;
        _endGamePanel.SetActive(true);
        _camera.SetActive(true);
        _cameraPlayer.SetActive(false);
        _infoEndGame.text = $"You kill {killZombies} zombies.";
    }

    private void QuitGame()
    {
        GameProfile.FlagGameOff.Value = true;
    }
}
