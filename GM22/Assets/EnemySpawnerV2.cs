using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerV2 : MonoBehaviour
{
    [SerializeField] int phase;
    [SerializeField] GameObject[] defaultCrabs;
    [SerializeField] GameObject[] crabsPrefabs;
    [SerializeField] Transform spawnPoint;
    public static EnemySpawnerV2 spawnerScr;
    bool spawnedCrabs;
    public int time, kills, spawnQue, spawnRate;
    // Start is called before the first frame update
    void Start()
    {
        spawnRate = 10;
        InvokeRepeating("perSec",1,1);
        spawnerScr = GetComponent<EnemySpawnerV2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (phase == 0)
        {
            if (!defaultCrabs[0])
            {
                phase = 1;
            }
        }
        else if (phase == 1)
        {
            if (!spawnedCrabs)
            {
                defaultCrabs[1].SetActive(true);
                defaultCrabs[2].SetActive(true);
                spawnedCrabs = true;
            }
            if (!defaultCrabs[1] || !defaultCrabs[2])
            {
                spawnedCrabs = false;
                phase = 2;
            }
        }
        else if (phase == 2)
        {
            if (!spawnedCrabs)
            {
                defaultCrabs[3].SetActive(true);
                defaultCrabs[4].SetActive(true);
                defaultCrabs[5].SetActive(true);
                spawnedCrabs = true;
            }
            if (!defaultCrabs[3] && !defaultCrabs[4] && !defaultCrabs[5])
            {
                spawnedCrabs = false;
                phase = 3;
            }
        }
        else if (phase == 3)
        {
            
        }
    }

    void perSec()
    {
        time++;
        if (phase > 2)
        {
            if (time % spawnRate == 0)
            {
                if (spawnQue > 0)
                {
                    if (kills > 12)
                    {
                        spawnEnemy(Random.Range(0, 4));
                    }
                    else
                    if (kills > 8)
                    {
                        spawnEnemy(Random.Range(0, 3));
                    }
                    else
                    {
                        spawnEnemy(Random.Range(0, 2));
                    }
                    spawnQue--;
                }
            }
            if (time%spawnRate == 0)
            {
                spawnQue++;
                if (spawnRate > 2)
                {
                    spawnRate--;
                }
            }
        }
    }

    public void enemyDied()
    {
        kills++;
        if (kills < 4) { return; }
        spawnQue++;
    }

    public void spawnEnemy(int type)
    {
        Instantiate(crabsPrefabs[type], spawnPoint.position, Quaternion.identity);
    }
}
