using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [SerializeField] private GameObserver gameObserver;
    [SerializeField] private PlayerUnit player;
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button defBtn;

    private void Awake()
    {
        gameObserver.OnPlayerDied = HidePanel;
        gameObserver.OnPlayerWon = HidePanel;
        gameObserver.OnReload = Reload;
    }
    private void HidePanel() 
    {
        this.gameObject.SetActive(false);
    }
    private void Reload() 
    {
        this.gameObject.SetActive(true);
    }

    public void ToogleButtons(bool value) 
    {
        attackBtn.interactable = value;
        defBtn.interactable = value;
    }

    public void Defending() 
    {
        player.Defend();
    }
    public void Attack() 
    {
        player.Attack();
    }
}