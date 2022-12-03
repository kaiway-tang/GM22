using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : Attack
{
    [SerializeField] Transform trfm;
    [SerializeField] float spd, life;
    [SerializeField] GameObject bulletHitFX;
    [SerializeField] GameObject strike;
    [SerializeField] bool groundFissure = false;
    [SerializeField] [Tooltip("Set to true to spawn strike as a beam")] bool beam = false;

    private void Start()
    {
        GameManager.FacePlayer(trfm, true);
        if (groundFissure)
        {
            Instantiate(strike, trfm.position, trfm.rotation);
            GameObject l1 = Instantiate(strike, trfm.position, trfm.rotation);
            GameObject r1 = Instantiate(strike, trfm.position, trfm.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
        }
        if (beam)
        {
            Instantiate(strike, trfm.position, trfm.rotation);
        }
        Destroy(gameObject, life);
    }

    private void FixedUpdate()
    {
        trfm.position += trfm.forward * spd;
    }

    private new void OnTriggerEnter(Collider col)
    {
        if (base.Hit(col))
        {
            Instantiate(bulletHitFX, trfm.position, trfm.rotation);

            Destroy(gameObject);
        }
    }
}
