using UnityEngine;
using TMPro;

public class PanelPopup : MonoBehaviour
{
    [SerializeField] private GameObserver gameObserver;
    [SerializeField] private GameObject EndGamePanel;
    [SerializeField] private TMP_Text congratsText;

    private void Awake()
    {
        gameObserver.OnPlayerDied += OnGameLost;
        gameObserver.OnPlayerWon += OnGameWin;
        gameObserver.OnReload += Reloading;
    }
    private void OnGameLost() 
    {
        congratsText.text = "You Lost!";
        this.gameObject.SetActive(true);
    }
    private void OnGameWin() 
    {
        congratsText.text = "You Won!";
        this.gameObject.SetActive(true);
    }

    private void Reloading()
    {
        this.gameObject.SetActive(false);
    }
}