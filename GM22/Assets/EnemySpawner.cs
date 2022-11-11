using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnLocations;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject beacon;

    [NonSerialized] public List<GameObject> enemies = new();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(x => x == null);
        
        beacon.SetActive(enemies.Count > 0);
    }

    public void Spawn()
    {
        foreach (var t in spawnLocations)
        {
            enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], t.transform.position,
                Quaternion.identity));
        }
    }
    
    public void Spawn(int a, int b, int c)
    {
        int i = 0;
        for (; i < spawnLocations.Length; i++)
        {
            enemies.Add(Instantiate(enemyPrefabs[0], spawnLocations[i].transform.position,
                Quaternion.identity));
        }for (; i < spawnLocations.Length; i++)
        {
            enemies.Add(Instantiate(enemyPrefabs[1], spawnLocations[i].transform.position,
                Quaternion.identity));
        }for (; i < spawnLocations.Length; i++)
        {
            enemies.Add(Instantiate(enemyPrefabs[2], spawnLocations[i].transform.position,
                Quaternion.identity));
        }
    }
}
