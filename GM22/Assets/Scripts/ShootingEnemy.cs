using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < trackingRange)
        {
            AddFwdVel(spd*.02f, spd);
            FaceTarget();
        }
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < attackingRange)
        {
            if (cd > 0)
            {
                cd--;
            }
            else
            {
                Shoot();
                cd = Random.Range(cdRange[0], cdRange[1]);
            }
        }
    }

    void Shoot()
    {
        Instantiate(projectile, trfm.position, trfm.rotation);
    }
}
