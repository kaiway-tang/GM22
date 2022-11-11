using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] private Animator anim;
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < attackingRange)
        {
            if (cd > 0)
            {
                cd -= Time.fixedDeltaTime;
            }
            else
            {
                Shoot();
                anim.SetTrigger("Attack");
                cd = Random.Range(cdRange[0], cdRange[1]);
            }
        }
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < trackingRange)
        {
            AddFwdVel(spd*.02f, spd);
            FaceTarget();
            
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.Play("Idle");
            
            anim.SetBool("Walking", false);
        }
    }

    void Shoot()
    {
        Instantiate(projectile, trfm.position, trfm.rotation);
    }
}
