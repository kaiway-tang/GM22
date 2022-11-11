using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] int[] cdRange; //cooldown range [min, max]
    int cd;
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cd > 0)
        {
            cd--;
            FaceTarget();
        }
        else
        {
            Shoot();
            cd = Random.Range(cdRange[0], cdRange[1]);
        }
    }

    void Shoot()
    {
        Instantiate(projectile, trfm.position, trfm.rotation);
    }
}
