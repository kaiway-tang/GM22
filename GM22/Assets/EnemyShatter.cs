using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShatter : MonoBehaviour
{
    [SerializeField] GameObject shatteredEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        { // collided with player
            Instantiate(shatteredEnemy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
