using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    //ASSUME LEAST EXPENSIVE IS FIRST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public List<Enemy> enemyPrefabs;
    public List<int> enemySpawnCost;
    public List<float> enemyChoicePriority;

    [Header("Add your own spawn points")]
    public List<Transform> spawnPoints;

    private List<Enemy> spawnedEnemies;

    public int wave = 0;        //wave number
    public int waveValue;   //how many points to use spawning wave

    public WaveStatus waveStatus = WaveStatus.UNKNOWN;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);

        spawnedEnemies = new List<Enemy>();
    }

    private void Update()
    {
        if (waveStatus == WaveStatus.INPROGRESS)
        {
            if (CheckIfWaveFinished())
                SpawnWave();
        }
    }

    private bool CheckIfWaveFinished()
    {
        foreach(Enemy e in spawnedEnemies)
        {
            if (e != null)
                return false;
        }
        return true;
    }

    private void SpawnWave()
    {
        wave++;
        spawnedEnemies.Clear();
        SetWaveValue();

        int failsafe = 0;
        while(waveValue > 0)
        {
            failsafe++;
            List<int> availableEnemyIndexes = GetEnemyIndexesForValue(waveValue);
            float random = Random.Range(0.0f, 1.0f);
            for(int i = availableEnemyIndexes.Count - 1; i >= 0; i--)
            {
                if(random <= enemyChoicePriority[availableEnemyIndexes[i]])
                {
                    SpawnEnemy(i);
                    break;
                }
            }

            if (failsafe > 1000)
                break;
        }
    }

    private void SpawnEnemy(int i)
    {
        spawnedEnemies.Add(Instantiate(enemyPrefabs[i], spawnPoints[0].position, Quaternion.identity, null));
        waveValue -= enemySpawnCost[i];
    }

    private void SetWaveValue()
    {
        waveValue = 1;
    }

    private List<int> GetEnemyIndexesForValue(int value)
    {
        List<int> result = new List<int>();
        for(int i = 0; i < enemySpawnCost.Count; i++)
        {
            if (waveValue >= enemySpawnCost[i])
                result.Add(i);
        }
        return result;
    }
}

public enum WaveStatus
{
    UNKNOWN = 0,
    INPROGRESS = 1,
    WAITING = 2,
}