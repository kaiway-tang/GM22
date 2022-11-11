using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int damage;

    protected void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6 || col.gameObject.layer == 8)
        {
            col.GetComponent<HPEntity>().TakeDmg(damage);
        }
    }
}
