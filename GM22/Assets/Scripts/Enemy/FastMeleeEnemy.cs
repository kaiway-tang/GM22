using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMeleeEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] private Animator anim;
    [SerializeField] private float rushCooldown; 
    private bool rushing;
    private float rushTimer;
    protected new void Start()
    {
        base.Start();
        rushTimer = rushCooldown;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        if (GameManager.playerControllerScr == null) return;

        base.FixedUpdate();
        if (Vector3.SqrMagnitude(GameManager.playerControllerScr.trfm.position - trfm.position) < attackingRange - 1)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            if (rushTimer < 0)
            {
                rushing = !rushing;
                if (!rushing)
                {
                    cd = Random.Range(100f, 100f);
                    rushTimer = rushCooldown;
                }
                else
                {
                    cd = Random.Range(0.2f, 0.2f);
                    rushTimer = 2;
                }
            }
            if (cd > 0 && !rushing)
            {
                anim.speed = 1;
                cd -= Time.fixedDeltaTime;
                rushTimer -= Time.fixedDeltaTime;
                transform.LookAt(GameManager.playerControllerScr.transform);
                rb.velocity = transform.right * spd * 0.5f + new Vector3(0, rb.velocity.y, 0);
            }else if (cd > 0 && rushing)
            {
                anim.speed = 2;
                cd -= Time.fixedDeltaTime;
                rushTimer -= Time.fixedDeltaTime;
                FaceTarget();
                FaceTarget();
                FaceTarget();
                FaceTarget();
                FaceTarget();
                FaceTarget();
                AddFwdVel(spd*.02f, spd * 2f);
            }
            else
            {
                anim.SetTrigger("Attack");
                if(!rushing) cd = Random.Range(100f, 100f);
                else cd = Random.Range(0.2f, 0.2f);
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
