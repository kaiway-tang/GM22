using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] int HP, maxHP, entityID;

    protected void Start()
    {
        maxHP = HP;
    }

    public void TakeDmg(int amount, int ignoreID = -1)
    {
        if (ignoreID == entityID) { return; }
        HP -= amount;
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }
    public void IncreaseMaxHP(int amount, bool increaseCurrentHP = true)
    {
        maxHP += amount;
        if (increaseCurrentHP)
        {
            HP += amount;
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
