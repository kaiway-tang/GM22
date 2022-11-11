using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    public int HP, maxHP, entityID;
    const int enemy = 0, player = 1;
    [SerializeField] GameObject hitFX;
    public Transform trfm;
    public float damageReduction;
    public bool invulnerable;

    protected void Start()
    {
        maxHP = HP;
    }

    public void TakeDmg(int amount, int ignoreID = -1)
    {
        if (ignoreID == entityID || invulnerable) { return; }
        if (hitFX) { Instantiate(hitFX, trfm.position, trfm.rotation); }

        if (entityID == enemy)
        {
            RageManager.AddRage(10);
        }
        else if (entityID == player)
        {
            RageManager.AddRage(-20);
        }

        HP -= Mathf.RoundToInt(amount*(1-damageReduction));
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

    bool died = false;
    public void Die(bool dropCore = false)
    {
        if (entityID == enemy && !died)
        {
            died = true;
            GetComponent<EnemyShatter>().Shatter(dropCore);
        }
        Destroy(gameObject);
    }
}
