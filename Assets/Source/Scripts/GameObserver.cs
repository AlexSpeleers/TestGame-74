using System;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
    public Action OnPlayerDied, OnPlayerWon, OnReload, KilledEnemy;
    private void Awake()
    {
        Application.targetFrameRate = 45;
    }
}