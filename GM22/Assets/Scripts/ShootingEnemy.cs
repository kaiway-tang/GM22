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
        if (GameManager.playerControllerScr == null) return;

        base.FixedUpdate();
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < attackingRange - 1)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            if (cd > 0)
            {
                cd -= Time.fixedDeltaTime;
                FaceTarget();
            }
            else
            {
                anim.SetTrigger("Attack");
                cd = Random.Range(cdRange[0], cdRange[1]);
            }
        }else if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < trackingRange)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            
            AddFwdVel(spd*.02f, spd);
            FaceTarget();
        }
        anim.SetBool("Walking", Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < trackingRange);
    }

    void Shoot()
    {
        Instantiate(projectile, trfm.position, trfm.rotation);
        rb.velocity = Vector3.zero;
    }
}
