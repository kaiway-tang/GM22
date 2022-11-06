using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> transformList = new List<Transform>();

        Vector3 dir = Vector3.zero;

        foreach (Transform child in transform)
        {
            dir.x = Random.Range(-2, 2);
            dir.y = Random.Range(-2, 2);
            dir.z = Random.Range(-2, 2);

            child.GetComponent<Rigidbody>().AddExplosionForce(300f, child.position + dir, 10f, Random.Range(-10, 10));
        }
        
    }
}
