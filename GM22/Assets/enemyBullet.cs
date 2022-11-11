using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : Attack
{
    [SerializeField] Transform trfm;
    [SerializeField] float spd;
    [SerializeField] GameObject bulletHitFX;

    private void Start()
    {
        GameManager.FacePlayer(trfm, true);
        Destroy(gameObject, 4);
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
