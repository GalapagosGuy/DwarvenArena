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
    private int currentSpawnerIndex = 0;

    private List<Enemy> spawnedEnemies;

    public int wave = 0;        //wave number
    public int waveValue;   //how many points to use spawning wave

    public bool debugSpawnNow = false;
    public float timeToNextWave = 5f;
    private float currentTime = 0;
    private PackageThrower packageThrower;
    public WaveStatus waveStatus = WaveStatus.UNKNOWN;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        spawnedEnemies = new List<Enemy>();
        if(!debugSpawnNow)
            currentTime = timeToNextWave;

        packageThrower = FindObjectOfType<PackageThrower>();
        if (packageThrower == null)
            Debug.Log("PACKAGE THROWER NOT FOUND");
    }

    private void Update()
    {
        if (waveStatus == WaveStatus.INPROGRESS)
        {
            if (CheckIfWaveFinished())
            {
                waveStatus = WaveStatus.WAITING;
                currentTime = timeToNextWave;
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                waveStatus = WaveStatus.INPROGRESS;
                SpawnWave();
            }
        }
    }

    private bool CheckIfWaveFinished()
    {
        foreach(Enemy e in spawnedEnemies)
        {
            if (e != null)
                return false;
        }
        if(wave == 1)
            packageThrower.SetNumberOfPackagesToThrow(3);
        else
            packageThrower.SetNumberOfPackagesToThrow(Random.Range(1 + (int)(wave / 3), 5 + (int)(wave / 3)));
        return true;
    }

    private void SpawnWave()
    {
        wave++;
        UIManager.Instance.UpdateWaveText();
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

    public void SkipWave()
    {
        currentTime = 0;
    }

    private void SpawnEnemy(int i)
    {
        spawnedEnemies.Add(Instantiate(enemyPrefabs[i], spawnPoints[currentSpawnerIndex].position, Quaternion.identity, null));
        waveValue -= enemySpawnCost[i];
        currentSpawnerIndex++;
        if (currentSpawnerIndex > spawnPoints.Count - 1)
            currentSpawnerIndex = 0;
    }

    private void SetWaveValue()
    {
        waveValue = wave * 2;
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