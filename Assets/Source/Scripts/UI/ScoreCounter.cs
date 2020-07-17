using UnityEngine;
using TMPro;
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreTXT;
    [SerializeField] private GameObserver gameObserver;
    private int score = 0;
    private void Awake()
    {
        gameObserver.KilledEnemy += IncrementScore;
        gameObserver.OnReload += Reload;
        ScoreTXT.text = $"Score: {score}";
    }

    private void IncrementScore() 
    {
        score++;
        ScoreTXT.text = $"Score: {score}";
    }
    private void Reload() 
    {
        score = 0;
        ScoreTXT.text = $"Score: {score}";
    }
}