using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public sealed class ObjSpawner : MonoBehaviour
{
    [SerializeField] private int medkitAmount = 2;
    [SerializeField] private int enemiesIncrement = 2;
    private int currentEnemiesAmount = 0;
    private int deadEnemies = 0;
    [SerializeField] private UnitData enemyData;
    [SerializeField] private EnemyUnit enemyUnit;
    [SerializeField] private Medkit medkit;
    [SerializeField] private Vector3[] enemySpawnPositions;
    [SerializeField] private Vector3[] medkitSpawnPositions;
    [SerializeField] private Camera cashedCam;
    [SerializeField] private GameObserver gameObserver;
    private List<EnemyUnit> enemyUnits;
    private List<Medkit> medkits;
    private WaitForSeconds medkitCooldown = new WaitForSeconds(30f);
    private WaitForSeconds enemiesWaveCooldown = new WaitForSeconds(3f);
    
    private void Awake()
    {
        enemyUnits = new List<EnemyUnit>();
        medkits = new List<Medkit>();
        gameObserver.OnPlayerDied += OnGameEnded;
        gameObserver.OnPlayerWon += OnGameEnded;
        gameObserver.OnReload += OnGameReload;
    }

    private void Start()
    {
        InitMedkit();
        InitEnemies();
    }

    private void InitEnemies() 
    {
        for (int i = currentEnemiesAmount; i < currentEnemiesAmount + enemiesIncrement; i++)
        {
            var obj = Instantiate(enemyUnit, enemySpawnPositions[i], Quaternion.identity);
            obj.UnitData = enemyData;
            obj.SetDefaults(cashedCam);
            obj.transform.SetParent(this.transform);
            obj.onEnemyDied = NextWave;
            enemyUnits.Add(obj);
        }
        currentEnemiesAmount = currentEnemiesAmount + enemiesIncrement > enemySpawnPositions.Length ? enemySpawnPositions.Length : currentEnemiesAmount + enemiesIncrement;
    }

    private void NextWave()
    {
        deadEnemies++;
        gameObserver.KilledEnemy?.Invoke();
        if (deadEnemies == currentEnemiesAmount)
        {
            if (currentEnemiesAmount < enemySpawnPositions.Length)
                StartCoroutine(SpawnNextWave());
            else
            {
                gameObserver.OnPlayerWon();
            }
            deadEnemies = 0;
        }
    }

    IEnumerator SpawnNextWave() 
    {
        yield return enemiesWaveCooldown;
        //works until 10 unique enemies are spawned
        if (enemyUnits.Count != enemySpawnPositions.Length)
        {
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                enemyUnits[i].SetDefaults();
                enemyUnits[i].transform.position = enemySpawnPositions[i];
            }            
            InitEnemies();
        }
        //reuse
        else
        {
            for (int i = currentEnemiesAmount; i < currentEnemiesAmount + enemiesIncrement; i++)
            {
                enemyUnits[i].transform.position = enemySpawnPositions[i];
                enemyUnits[i].SetDefaults();
            }
            currentEnemiesAmount = currentEnemiesAmount + enemiesIncrement > enemySpawnPositions.Length ? enemySpawnPositions.Length : currentEnemiesAmount + enemiesIncrement;
        }           
    }
    #region observerMethods
    private void OnGameEnded() 
    {
        StopAllCoroutines();
        foreach (var item in enemyUnits)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in medkits)
        {
            item.gameObject.SetActive(false);
        }
    }
    private void OnGameReload() 
    {
        deadEnemies = 0;
        currentEnemiesAmount = 0;
        StartCoroutine(SpawnNextWave());
        StartCoroutine(RespawnMedkits());
    }
    #endregion

    #region medkitSpawn
    private void InitMedkit()
    {
        for (int i = 0; i < medkitAmount; i++)
        {
            var obj = Instantiate(medkit, GetRandomPos(medkitSpawnPositions), Quaternion.identity);
            obj.transform.SetParent(this.transform);
            medkits.Add(obj);
        }
        StartCoroutine(RespawnMedkits());
    }
    IEnumerator RespawnMedkits()
    {
        yield return medkitCooldown;
        foreach (var item in medkits) 
        {
            item.gameObject.SetActive(true);
            item.transform.position = GetRandomPos(medkitSpawnPositions);
        }
        StartCoroutine(RespawnMedkits()); // --------!!!Watchout recoursion!!!--------
    }

    private Vector3 GetRandomPos(Vector3[] objPositions)
    {
        var index = Random.Range(0, objPositions.Length);
        return objPositions[index];
    }
    #endregion
}