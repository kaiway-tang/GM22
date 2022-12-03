using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] GameObject col;
    [SerializeField] float duration = 6f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLaser());
    }

    IEnumerator SpawnLaser()
    {
        float time = 0;
        while (time < duration)
        {
            col.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            col.SetActive(false);
            time += 0.3f;
        }
        Destroy(gameObject);
    }
}
