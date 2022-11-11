using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShatter : MonoBehaviour
{
    [SerializeField] GameObject shatteredEnemy;
    [SerializeField] GameObject core;
    //bool died = false;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 6 && !died)
    //    { // collided with player
    //        died = true;
    //        Instantiate(shatteredEnemy, transform.position, Quaternion.identity);
    //        Instantiate(core, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    //        Destroy(gameObject);
    //    }
    //}

    public void Shatter()
    {
        Instantiate(shatteredEnemy, transform.position, Quaternion.identity);
        Instantiate(core, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
}
