using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> transformList = new List<Transform>();

        foreach (Transform child in transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(1000f, child.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)), 10f, Random.Range(-10, 10));
        }
        
    }
}
