using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerV2 : MonoBehaviour
{
    [SerializeField] int phase;
    [SerializeField] GameObject[] defaultCrabs;
    [SerializeField] GameObject[] crabsPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
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
            defaultCrabs[1].SetActive(true);
            defaultCrabs[2].SetActive(true);
            if (!defaultCrabs[1] || !defaultCrabs[2])
            {
                phase = 2;
            }
        }
        else if (phase == 2)
        {

        }
    }
}
