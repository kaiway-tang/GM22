using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] int HP, maxHP, entityID;

    protected void _Start()
    {
        maxHP = HP;
    }

    protected void TakeDmg(int amount, int ignoreID = -1)
    {
        if (ignoreID == entityID) { return; }
        HP -= amount;
        if (HP <= 0)
        {
            Die();
        }
    }

    protected void Heal(int amount)
    {
        HP += amount;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
