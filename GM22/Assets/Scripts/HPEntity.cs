using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] public int HP, maxHP, entityID;
    [SerializeField] GameObject hitFX;
    [SerializeField] public Transform trfm;

    protected void Start()
    {
        maxHP = HP;
    }

    public void TakeDmg(int amount, int ignoreID = -1)
    {
        if (ignoreID == entityID) { return; }
        if (hitFX) { Instantiate(hitFX, trfm.position, trfm.rotation); }
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
