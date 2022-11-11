using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int damage;

    protected void OnTriggerEnter(Collider col)
    {
        Hit(col);
    }

    protected bool Hit(Collider col)
    {
        if (col.gameObject.layer == 6 || col.gameObject.layer == 8)
        {
            col.GetComponent<HPEntity>().TakeDmg(damage);
            return true;
        }
        else if (col.gameObject.layer == 11)
        {
            col.GetComponent<HPEntity>().TakeDmg(damage);
            return true;
        }
        return false;
    }
}
